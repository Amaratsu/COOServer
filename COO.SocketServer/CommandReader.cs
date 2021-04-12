using System.IO;
using System.Net.Sockets;

/* 
 * This file only contains Read methods. They read as many bytes as necessary, and then pass the command they've read on the concurrent queue that's being executed on the main thread.
 * This way if multiple commands are packed in the same buffer, we can read them one by one.
 */
namespace COO.SocketServer
{
    public partial class ChatServer
    {
        public static void ReadLogin(BinaryReader reader, Socket connection)
        {
            string charName = reader.ReadMmoString();
            ConQue.Enqueue(() => ProcessLogin(charName, connection));
        }

        private static void ReadGeneralMessage(BinaryReader reader, Socket connection)
        {
            string playerMessage = reader.ReadMmoString();
            ConQue.Enqueue(() => ProcessGeneralMessage(playerMessage, connection));
        }

        private static void ReadPrivateMessage(BinaryReader reader, Socket connection)
        {
            string targetPlayerName = reader.ReadMmoString();
            string playerMessage = reader.ReadMmoString();
            ConQue.Enqueue(() => ProcessPrivateMessage(playerMessage, targetPlayerName, connection));
        }

        private static void ReadGroupMessage(BinaryReader reader, Socket connection)
        {
            string playerMessage = reader.ReadMmoString();
            ConQue.Enqueue(() => ProcessGroupMessage(playerMessage, connection));
        }

        private static void ReadClanMessage(BinaryReader reader, Socket connection)
        {
            string playerMessage = reader.ReadMmoString();
            ConQue.Enqueue(() => ProcessClanMessage(playerMessage, connection));
        }

        private static void ReadGroupInvite(BinaryReader reader, Socket connection)
        {
            string targetPlayerName = reader.ReadMmoString();
            ConQue.Enqueue(() => ProcessGroupInvite(targetPlayerName, connection));
        }

        private static void ReadAcceptInvite(Socket connection)
        {
            ConQue.Enqueue(() => ProcessAcceptInvite(connection));
        }

        private static void ReadDeclineInvite(Socket connection)
        {
            ConQue.Enqueue(() => ProcessDeclineInvite(connection));
        }

        private static void ReadClanCreate(BinaryReader reader, Socket connection)
        {
            string clanName = reader.ReadMmoString();
            ConQue.Enqueue(() => ProcessClanCreate(clanName, connection));
        }

        private static void ReadClanInvite(BinaryReader reader, Socket connection)
        {
            string invitedPlayer = reader.ReadMmoString();
            ConQue.Enqueue(() => ProcessClanInvite(invitedPlayer, connection));
        }

        private static void ReadClanDisband(Socket connection)
        {
            ConQue.Enqueue(() => ProcessClanDisband(connection));
        }

        private static void ReadClanLeave(Socket connection)
        {
            ConQue.Enqueue(() => ProcessClanLeave(connection));
        }

        private static void ReadGroupLeave(Socket connection)
        {
            ConQue.Enqueue(() => ProcessGroupLeave(connection));
        }

        private static void ReadClanKick(BinaryReader reader, Socket connection)
        {
            string targetPlayerName = reader.ReadMmoString();
            ConQue.Enqueue(() => ProcessClanKick(targetPlayerName, connection));
        }

        private static void ReadGroupKick(BinaryReader reader, Socket connection)
        {
            string targetPlayerName = reader.ReadMmoString();
            ConQue.Enqueue(() => ProcessGroupKick(targetPlayerName, connection));
        }
    }
}

