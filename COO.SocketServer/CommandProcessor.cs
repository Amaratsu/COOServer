using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

/* 
 * This file only contains Process methods.
 */

namespace COO.SocketServer
{
    public partial class ChatServer
    { 
        static readonly ConcurrentQueue<Action> ConQue = new ConcurrentQueue<Action>();

        /* 
     * The method gets called once every 8ms (120 times per second) from void Main.
     * It tries to read an Action from the queue and Invoke it on the main thread. It avoids concurrency issues.
     */
        public static async Task ProcessorLoop()
        {        
            while (true)
            {
                process_queue:
                if (ConQue.TryDequeue(out Action result))
                {
                    await Task.Run(result);
                    goto process_queue;
                }
                await Task.Delay(8);
            }
        }

        public static void ProcessLogin(string charName, Socket connection)
        {
            if (charName == "i am a game server") // Dear Gamedev! Change it to something else, it acts as a passphrase for the server to connect as one.
            {
                GameServers.Add(connection);
                InitializeGroupsOnServer();
                Console.WriteLine("A server logged in.");
            }
            else
            {
                PlayerLogin(connection, charName);
                Console.WriteLine("Player logged in: " + charName);
            }
        }

        public static void ProcessGeneralMessage(string message, Socket connection)
        {
            Player thisPlayer = GetPlayer(connection);
            if (thisPlayer == null) return;
            Console.WriteLine(thisPlayer.Name + ": " + message);

            // crop it if you don't want it too long, or log it to a file, or apply censorship filter on it, etc...
            byte[] distributeMessage = MergeByteArrays(ToBytes(Command.GeneralMessage),
                WriteMmoString(thisPlayer.Name), // who messages
                WriteMmoString(message)); // the message itself

            foreach (Player player in Players)
            {
                player.SendOrKick(distributeMessage);
            }
        }

        public static void ProcessPrivateMessage(string playerMessage, string targetPlayerName, Socket connection)
        {
            Player thisPlayer = GetPlayer(connection);
            if (thisPlayer == null) return;
            Console.WriteLine($"{thisPlayer.Name} to {targetPlayerName}: {playerMessage}");

            Player targetPlayer = Players.SingleOrDefault(x => String.Compare(x.Name, targetPlayerName, StringComparison.OrdinalIgnoreCase) == 0); //case insensitive
            if (targetPlayer != null)
            {
                byte[] message = MergeByteArrays(ToBytes(Command.PrivateMessage),
                    WriteMmoString(thisPlayer.Name), // who messages
                    WriteMmoString(playerMessage)); // the message itself

                // Altering message so that it appears as "To TargetPlayer: Message" to the player who sent it
                byte[] alteredMessage = MergeByteArrays(ToBytes(Command.PrivateMessage),
                    WriteMmoString("To " + targetPlayerName), // to whom
                    WriteMmoString(playerMessage)); // the message itself

                targetPlayer.SendOrKick(message);
                thisPlayer.SendOrKick(alteredMessage);
            }
        }

        public static void ProcessGroupMessage(string playerMessage, Socket connection)
        {
            Player thisPlayer = GetPlayer(connection);
            if (thisPlayer == null || thisPlayer.Group == null) return;
            Console.WriteLine($"[Group] {thisPlayer.Name}: {playerMessage}");

            byte[] message = MergeByteArrays(ToBytes(Command.GroupMessage),
                WriteMmoString(thisPlayer.Name), // who messages
                WriteMmoString(playerMessage)); // the message itself

            foreach (Player groupMember in thisPlayer.Group.GroupMembers)
                groupMember.SendOrKick(message);
        }

        public static void ProcessClanMessage(string playerMessage, Socket connection)
        {
            Player thisPlayer = GetPlayer(connection);
            if (thisPlayer == null || thisPlayer.ClanId == 0) return;
            Console.WriteLine($"[Clan] {thisPlayer.Name}: {playerMessage}");

            byte[] message = MergeByteArrays(ToBytes(Command.ClanMessage),
                WriteMmoString(thisPlayer.Name), // who messages
                WriteMmoString(playerMessage)); // the message itself

            foreach (Player member in Players.Where(x => x.ClanId == thisPlayer.ClanId))
                member.SendOrKick(message);
        }

        public static void ProcessGroupInvite(string targetPlayerName, Socket connection)
        {
            Player thisPlayer = GetPlayer(connection);
            if (thisPlayer == null) return;
            Player targetPlayer = GetPlayerByName(targetPlayerName);
            if (targetPlayer == null || targetPlayer == thisPlayer) return;

            //@TODO: return reason for invitation failure to the inviting player
            if (thisPlayer.Group != null && thisPlayer.Group.Leader != thisPlayer) return; //if the inviting player is not the group's leader
            if (thisPlayer.Group != null && thisPlayer.Group.GroupMembers.Count >= MaxGroupSize) return; //check max group size
            if (targetPlayer.Group != null || targetPlayer.PendingInvite != null) return; //if the player is already in a group or being invited

            CreatePendingInvite(targetPlayer, thisPlayer, false);

            byte[] message = MergeByteArrays(ToBytes(Command.GroupInvite),
                WriteMmoString(thisPlayer.Name)); // who invites
            targetPlayer.SendOrKick(message);
        }

        public static void ProcessAcceptInvite(Socket connection)
        {
            Player thisPlayer = GetPlayer(connection);
            if (thisPlayer == null) return;
            Player invitingPlayer = thisPlayer.PendingInvite;
            if (invitingPlayer == null) return;        

            if (thisPlayer.HasPendingClanInvite) //for clan invite
            {
                if (thisPlayer.ClanId != 0 || invitingPlayer.ClanId == 0)
                    return;

                ClearInviteBeforeTick(thisPlayer);
                _ = SendPlayerJoinedClanAsync(thisPlayer, invitingPlayer); // async php request, not awaited
            }
            else  //for group invite
            {
                if (thisPlayer.Group != null) return;
                if (invitingPlayer.Group != null && invitingPlayer.Group.GroupMembers.Count >= MaxGroupSize) return; // @TODO: send "inviting player's group is already at max capacity"

                if (invitingPlayer.Group == null) //create a new group
                {
                    Group newGroup = new Group(invitingPlayer, thisPlayer);
                    Groups.Add(newGroup);
                }
                else //add the new player to the existing group
                {
                    invitingPlayer.Group.GroupMembers.Add(thisPlayer);
                    thisPlayer.Group = invitingPlayer.Group;
                }

                ClearInviteBeforeTick(thisPlayer);
                SendGroupUpdateToServers();
            }
        }

        public static void ProcessDeclineInvite(Socket connection)
        {
            Player thisPlayer = GetPlayer(connection);
            if (thisPlayer == null) return;
        
            Player invitingPlayer = thisPlayer.PendingInvite;
            if (invitingPlayer != null)
            {
                byte[] message = MergeByteArrays(ToBytes(Command.DeclineInvite),
                    WriteMmoString(thisPlayer.Name)); // who refused the invite
                invitingPlayer.SendOrKick(message);
            }
            ClearInviteBeforeTick(thisPlayer);
        }

        public static void ProcessClanCreate(string clanName, Socket connection)
        {
            Player thisPlayer = GetPlayer(connection);
            if (thisPlayer == null) return;
            clanName = SanitizeStringForXss(clanName, 25); // removes special characters and trims characters after 25 symbols
            _ = SendCreateClanAsync(thisPlayer, clanName);
        }

        public static void ProcessClanInvite(string invitedPlayer, Socket connection)
        {
            Player thisPlayer = GetPlayer(connection);
            if (thisPlayer == null) return;
            if (thisPlayer.ClanId == 0) return;
            if (!thisPlayer.IsClanLeader) return;

            //find the player that we want to invite
            Player targetPlayer = Players.SingleOrDefault(x => x.Name == invitedPlayer);
            if (targetPlayer == null || targetPlayer == thisPlayer || targetPlayer.ClanId != 0) return;
            if (targetPlayer.PendingInvite != null) return; // he's currently considering an invite, @TODO: send a message back saying it

            CreatePendingInvite(targetPlayer, thisPlayer, true);

            string clanName = ClanNames.ContainsKey(thisPlayer.ClanId) ? ClanNames[thisPlayer.ClanId] : "Undefined";
            byte[] message = MergeByteArrays(ToBytes(Command.ClanInvite),
                WriteMmoString(thisPlayer.Name),
                WriteMmoString(clanName));

            targetPlayer.SendOrKick(message); //forward the Clan Invite message to the target player
        }

        public static void ProcessClanDisband(Socket connection)
        {
            Player thisPlayer = GetPlayer(connection);
            if (thisPlayer == null || thisPlayer.ClanId == 0 || !thisPlayer.IsClanLeader) return;
            _ = SendDisbandClanAsync(thisPlayer);
        }

        public static void ProcessClanLeave(Socket connection)
        {
            Player thisPlayer = GetPlayer(connection);        
            if (thisPlayer == null || thisPlayer.ClanId == 0 || thisPlayer.IsClanLeader) return; //for now, the leader can't leave the clan
            Console.WriteLine($"Clan leave: {thisPlayer.Name}");
            _ = SendClanLeaveAsync(thisPlayer);
        }

        public static void ProcessGroupLeave(Socket connection)
        {
            Console.WriteLine("Received group leave!");
            Player thisPlayer = GetPlayer(connection);
            RemoveFromGroup(thisPlayer);
        }

        public static void ProcessClanKick(string targetPlayerName, Socket connection)
        {
            Player thisPlayer = GetPlayer(connection);
            if (thisPlayer == null || thisPlayer.ClanId == 0 || !thisPlayer.IsClanLeader) return;

            //find the player that we want to kick
            Player targetPlayer = Players.SingleOrDefault(x => x.Name == targetPlayerName);
            if (targetPlayer == null || thisPlayer.ClanId != targetPlayer.ClanId) return;

            _ = SendClanKickAsync(targetPlayer);
        }

        public static void ProcessGroupKick(string targetPlayerName, Socket connection)
        {
            Player thisPlayer = GetPlayer(connection);
            bool isGameServer = GameServers.Contains(connection);

            // if initiating party is player and player is not in group, etc...
            if (!isGameServer && (thisPlayer == null || thisPlayer.Group == null || thisPlayer.Group.Leader != thisPlayer)) return;

            //find the player that we want to kick
            Player targetPlayer = Players.SingleOrDefault(x => x.Name == targetPlayerName);
            if (targetPlayer == null || targetPlayer.Group == null || (!isGameServer && (targetPlayer.Group != thisPlayer.Group))) return;

            Console.WriteLine($"Kicking from group: {targetPlayerName}");

            if (isGameServer) //kick issued by server (because of disconnect), kick after a delay of X seconds
            {
                CreatePendingKick(targetPlayer);
            }
            else
            {
                RemoveFromGroup(targetPlayer); //kick issued by group leader, kick immediately                                                                 
                //notify the player that they were kicked from the clan
                byte[] message = MergeByteArrays(ToBytes(Command.GroupKick));
                targetPlayer.SendOrKick(message);
            }

        }
    }
}