using System;
using System.Linq;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;

/* 
 * Only contains methods that manage players/groups/etc, and that are expected to be executed on the main thread
 */
namespace COO.SocketServer
{
    public partial class ChatServer
    {
        public static Player GetPlayer(Socket connection)
        {
            return connection != null ? Players.SingleOrDefault(x => x.Socket == connection) : null;
        }

        public static Player GetPlayerByName(string targetPlayerName)
        {
            return Players.SingleOrDefault(x => x.Name == targetPlayerName);
        }

        public static void PlayerLogin(Socket conn, string characterName)
        {
            Player newPlayer = new Player(conn, characterName);

            // if it's a character that's in some clan, get his id
            if (CharacterIds.ContainsKey(newPlayer.Name))
            {
                newPlayer.Id = CharacterIds[newPlayer.Name];

                // if a clan for this character id is found, assign clan to character
                if (CharacterClans.ContainsKey(newPlayer.Id))
                    newPlayer.ClanId = CharacterClans[newPlayer.Id];

                // set him as clan leader if he's in the list of clan leaders
                if (ClanLeaders.Contains(newPlayer.Id))
                    newPlayer.IsClanLeader = true;
            }

            // if it's a reconnecting character, remove him from list of online characters        
            Players.RemoveAll(x => x.Name == newPlayer.Name);
            // add him to list of online characters
            Players.Add(newPlayer);

            // update this player in groups if he was in one before reconnecting
            Groups.ForEach(group => group.UpdateReconnectedPlayer(newPlayer));

            if (PendingKicks.ContainsKey(newPlayer.Name)) //cancel automatic kick
            {
                PendingKicks[newPlayer.Name].Stop();
                PendingKicks.Remove(newPlayer.Name);
            }
        }

        public static void KickPlayer(Player pl)
        {
            if (pl == null)
                return;

            Console.WriteLine($"Player disconnected: {pl.Name}");
            if (pl.Group != null)
                RemoveFromGroup(pl);

            Players.Remove(pl);
        }

        public static void KickPlayer(Socket connection)
        {
            KickPlayer(GetPlayer(connection));
        }

        public static void KickServer(Socket conn)
        {
            Console.WriteLine("Server disconnected");
            GameServers.Remove(conn);
        }

        public static void KickConnection(Socket connection)
        {
            if (GameServers.Contains(connection))
                KickServer(connection);
            else
                KickPlayer(GetPlayer(connection));
        }

        // clears pending invite if the invite expired
        static void ClearPendingInvite(Player invitedPlayer)
        {
            if (invitedPlayer == null) return;
            Console.WriteLine($"Clearing pending invite for invited player: {invitedPlayer.Name}");
            PendingInvites.Remove(invitedPlayer);
            invitedPlayer.PendingInvite = null;
            invitedPlayer.HasPendingClanInvite = false;
        }

        static void ClearPendingKick(Player playerToKick)
        {
            if (playerToKick == null) return;
            if (PendingKicks.ContainsKey(playerToKick.Name)) //cancel automatic kick
            {
                Console.WriteLine($"Clearing pending kick for player: {playerToKick.Name}");
                PendingKicks[playerToKick.Name].Stop();
                PendingKicks.Remove(playerToKick.Name);
            }
        }

        // clears pending invite if it was accepted or declined by the player
        static void ClearInviteBeforeTick(Player targetPlayer)
        {
            if (targetPlayer == null) return;

            Console.WriteLine($"Clearing pending invite due to player accepting/declining: {targetPlayer.Name}");

            if (PendingInvites.ContainsKey(targetPlayer))
            {
                PendingInvites[targetPlayer].Stop();
                PendingInvites.Remove(targetPlayer);
            }

            targetPlayer.PendingInvite = null;
            targetPlayer.HasPendingClanInvite = false;
        }

        public static void CreatePendingInvite(Player invitedPlayer, Player invitingParty, bool bClanInvite)
        {
            invitedPlayer.PendingInvite = invitingParty;
            invitedPlayer.HasPendingClanInvite = bClanInvite;
            double delay = 20000.0; // 20 seconds, 1000 ms is one second
            System.Timers.Timer myTimer = new System.Timers.Timer(delay);
            myTimer.Elapsed += (sender, args) => ConQue.Enqueue(() => ClearPendingInvite(invitedPlayer));
            myTimer.AutoReset = false; // fire only once
            myTimer.Enabled = true;
            PendingInvites.Add(invitedPlayer, myTimer);
        }

        public static void CreatePendingKick(Player playerToKick)
        {
            if (!PendingKicks.ContainsKey(playerToKick.Name))
            {
                double delay = 30000.0;
                System.Timers.Timer myTimer = new System.Timers.Timer(delay);
                myTimer.Elapsed += (sender, args) => ConQue.Enqueue(() => ClearPendingKick(playerToKick));
                myTimer.AutoReset = false; // fire only once
                myTimer.Enabled = true;
                PendingKicks.Add(playerToKick.Name, myTimer);
            }
        }

        public static async Task SendPlayerJoinedClanAsync(Player thisPlayer, Player invitingPlayer)
        {
            string response = await SendRequestAsync("AddCharacterToClan",
                "{\"CharacterName\":\"" + thisPlayer.Name + "\"," +
                "\"ClanId\":\"" + invitingPlayer.ClanId + "\"}"
            );

            if (response.Contains("OK"))
            {
                Console.WriteLine($"Database processed clan invite for player: {thisPlayer.Name}");
                ConQue.Enqueue(StartClansUpdate);
            }
            else
            {
                Console.WriteLine($"Clan invite for {thisPlayer.Name} failed. Response: {response}");
            }
        }

        public static async Task SendCreateClanAsync(Player thisPlayer, string clanName)
        {
            string response = await SendRequestAsync("CreateClan",
                "{\"CharacterName\":\"" + thisPlayer.Name + "\"," +
                "\"ClanName\":\"" + clanName + "\"}"
            );

            if (response.Contains("OK"))
                ConQue.Enqueue(StartClansUpdate);
        }

        public static async Task SendDisbandClanAsync(Player thisPlayer)
        {
            string response = await SendRequestAsync("DisbandClan",
                "{\"CharacterId\":\"" + thisPlayer.Id + "\"}"
            );

            if (response.Contains("OK"))
            {
                Console.WriteLine($"Clan disbanded by: {thisPlayer.Name}");
                ConQue.Enqueue(StartClansUpdate);
            }
            else
            {
                Console.WriteLine($"Clan disband by {thisPlayer.Name} failed. Response: {response}");
            }
        }

        public static async Task SendClanLeaveAsync(Player thisPlayer)
        {
            string response = await SendRequestAsync("LeaveFromClan",
                "{\"CharacterId\":\"" + thisPlayer.Id + "\"}"
            );

            if (response.Contains("OK"))
                ConQue.Enqueue(StartClansUpdate);
        }

        public static async Task SendClanKickAsync(Player targetPlayer)
        {
            string response = await SendRequestAsync("DeleteCharacterFromClan",
                "{\"CharacterId\":\"" + targetPlayer.Id + "\"}"
            );

            if (response.Contains("OK"))
                ConQue.Enqueue(StartClansUpdate);
        }

        static void StartClansUpdate()
        {
            _ = UpdateClansAsync();
        }

        public static async Task UpdateClansAsync()
        {
            Console.WriteLine("Requesting clans from database.");
            string response = await SendRequestAsync("ClanCharacters", "");
            Console.WriteLine("Received clans.");
            ClansOutput result = JsonSerializer.Deserialize<ClansOutput>(response);
            ConQue.Enqueue(() => UpdateClansWithInfo(result));
        }

        public static void UpdateClansWithInfo(ClansOutput result)
        {
            CharacterIds.Clear();
            CharacterClans.Clear();
            ClanLeaders.Clear();
            ClanNames.Clear();

            Players.ForEach(player => player.ClanId = 0);

            foreach (var row in result.Clans)
            {
                CharacterIds.Add(row.CharacterName, row.CharacterId);
                CharacterClans.Add(row.CharacterId, row.ClanId);
                if (row.IsLeader)
                    ClanLeaders.Add(row.CharacterId);

                if (!ClanNames.ContainsKey(row.ClanId))
                    ClanNames.Add(row.ClanId, row.ClanName);

                Player foundPlayer = Players.FirstOrDefault(x => x.Name == row.CharacterName);
                if (foundPlayer != null)
                {
                    foundPlayer.Id = row.CharacterId;
                    foundPlayer.ClanId = row.ClanId;
                    foundPlayer.IsClanLeader = row.IsLeader;
                }
            }

            byte[] message = MergeByteArrays(ToBytes(Command.ClanUpdate)); // inform game servers that they need to update clans from db by themselves

            foreach (Socket gameServer in GameServers)
            {
                gameServer.SendOrFail(message);
            }
        }

        static void RemoveFromGroup(Player player)
        {
            if (player == null || player.Group == null) return;
            Group group = player.Group;

            Console.WriteLine($"Kicking player from group: {player.Name}");

            group.GroupMembers.Remove(player);
            player.Group = null;

            if (group.GroupMembers.Count > 1)
            {
                if (group.Leader == player) // if party leader left, assign party leadership to someone else
                {                
                    group.Leader = group.GroupMembers[0];
                }
            }
            else //if there is less than 2 players now, disband the group
            {
                group.GroupMembers[0].Group = null;
                Groups.Remove(group);
            }
            SendGroupUpdateToServers();
        }

        static void SendGroupUpdateToServers()
        {
            string playerNames = "";
            foreach (Group group in Groups) //each group is divided by ":", each group member is divided by ','
            {
                playerNames += group.GetPlayerNames();
                playerNames += ":";
            }

            byte[] message = MergeByteArrays(ToBytes(Command.GroupUpdate),
                WriteMmoString(playerNames));

            foreach (Socket gameServer in GameServers)
            {
                gameServer.SendOrFail(message);
            }
        }

        private static string SanitizeStringForXss(string inputString, int maxLength)
        {
            var result = inputString.Length <= maxLength ? inputString : inputString.Substring(0, maxLength);
            result = result.Replace("'", string.Empty);
            result = result.Replace("\"", string.Empty);
            result = result.Replace("`", string.Empty);
            return result;
        }

        // when a new game server connects, send it all the groups we currently have
        static void InitializeGroupsOnServer()
        {

        }
    }
}