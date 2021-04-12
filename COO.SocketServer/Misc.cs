using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace COO.SocketServer
{
    public enum Command
    {
        Undefined,          //0 - Undefined
        Login,              //1 - Log into the server
        Logout,             //2 - Logout of the server
        GeneralMessage,     //3 - Send a text message to all the chat clients
        PrivateMessage,     //4
        GroupInvite,        //5
        GroupLeave,         //6
        GroupUpdate,        //7 - is sent to the game servers
        AcceptInvite,       //8 - used both for groups and clans
        DeclineInvite,      //9 - used both for groups and clans
        GroupMessage,       //10
        GroupKick,          //11       
        ClanLeave,         //12
        ClanInvite,        //13    
        NotUsed,            //14   -- not used
        NotUsed2,           //15   -- not used
        ClanMessage,       //16
        ClanKick,          //17
        ClanUpdate,        //18 - is sent to the game servers
        ClanCreate,        //19
        ClanDisband,       //20
    }

    public static class ExtensionMethods
    {
        // Used on servers. Send or fail silently. We don't kick servers even if a Send() fails for some reason.
        public static bool SendOrFail(this Socket socket, byte[] buffer)
        {
            try
            {
                socket.Send(buffer);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public static string ReadMmoString(this BinaryReader reader)
        {
            int strLen = reader.ReadInt32();
            var stringBytes = reader.ReadBytes(strLen);
            string result = Encoding.UTF8.GetString(stringBytes);
            return result;
        }
    }

// State object for reading client data asynchronously
    public class StateObject
    {
        // Client  socket.
        public Socket WorkSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 1024;
        // Receive buffer.
        public byte[] Buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder Sb = new StringBuilder();

        public void ResetBuffer()
        {
            Sb.Clear();
            Buffer = new byte[BufferSize];
        }
    }

    public class Player
    {
        public Socket Socket;

        public string Name;
        public int Id = -1;

        public Group Group;
        public int ClanId;
        public bool IsClanLeader;
        public Player(Socket inSocket, string inName)
        {
            Socket = inSocket;
            Name = inName;
        }

        public Player PendingInvite; //the player who invited this player and is waiting for a reply
        public bool HasPendingClanInvite;

        // If Send fails, we kick the player, because he left.
        public bool SendOrKick(byte[] buffer)
        {        
            if (Socket == null || !Socket.SendOrFail(buffer))
            {
                ChatServer.KickPlayer(this);
                return false;
            }
            return true;
        }
    }

    public class Group
    {
        public Group(Player player1, Player player2)
        {
            GroupMembers = new List<Player>() { player1, player2 };
            player1.Group = player2.Group = this;
            Leader = player1;
        }

        public List<Player> GroupMembers;

        public Player Leader;
        public string GetPlayerNames()
        {
            string result = string.Join(",", GroupMembers.Select(x => x.Name));
            return result;
        }

        public void UpdateReconnectedPlayer(Player reconnectedPlayer)
        {
            var foundPlayer = GroupMembers.SingleOrDefault(player => player.Name == reconnectedPlayer.Name);
            if (foundPlayer != null)
            {
                foundPlayer = reconnectedPlayer;
            }
        }
    }

    public class Data
    {
        //Default constructor
        public Data()
        {
            this.CmdCommand = Command.Undefined;
            this.StrMessage = null;
            this.StrName = null;
        }

        //Converts the bytes into an object of type Data
        public Data(byte[] data)
        {
            //The first four bytes are for the Command
            this.CmdCommand = (Command)BitConverter.ToInt32(data, 0);

            //The next four store the length of the name
            int nameLen = BitConverter.ToInt32(data, 4);

            //The next four store the length of the message
            int msgLen = BitConverter.ToInt32(data, 8);

            //This check makes sure that strName has been passed in the array of bytes
            if (nameLen > 0)
                this.StrName = Encoding.UTF8.GetString(data, 12, nameLen);
            else
                this.StrName = null;

            //This checks for a null message field
            if (msgLen > 0)
                this.StrMessage = Encoding.UTF8.GetString(data, 12 + nameLen, msgLen);
            else
                this.StrMessage = null;
        }

        //Converts the Data structure into an array of bytes
        public byte[] ToBytes()
        {
            List<byte> result = new List<byte>();

            //First four are for the Command
            result.AddRange(BitConverter.GetBytes((int)CmdCommand));

            //Add the length of the name
            if (StrName != null)
                result.AddRange(BitConverter.GetBytes(Encoding.UTF8.GetByteCount(StrName)));
            //result.AddRange(BitConverter.GetBytes(strName.Length));
            else
                result.AddRange(BitConverter.GetBytes(0));

            //Length of the message
            if (StrMessage != null)
                //result.AddRange(BitConverter.GetBytes(strMessage.Length));
                result.AddRange(BitConverter.GetBytes(Encoding.UTF8.GetByteCount(StrMessage)));
            else
                result.AddRange(BitConverter.GetBytes(0));

            //Add the name
            if (StrName != null)
                result.AddRange(Encoding.UTF8.GetBytes(StrName));

            //And, lastly we add the message text to our array of bytes
            if (StrMessage != null)
                result.AddRange(Encoding.UTF8.GetBytes(StrMessage));

            return result.ToArray();
        }

        public string StrName;      //Name by which the client logs into the room
        public string StrMessage;   //Message text
        public Command CmdCommand;  //Command type (login, logout, send message, etcetera)
    }

    public class ClansOutput
    {
        public string Status { get; set; }
        public List<CharacterRow> Clans { get; set; }
    }

    public class CharacterRow
    {
        public int CharacterId { get; set; }
        public string CharacterName { get; set; }
        public int ClanId { get; set; }
        public string ClanName { get; set; }
        public bool IsLeader { get; set; }
    }

    public partial class ChatServer
    {
        private static async Task<string> SendRequestAsync(string url, string json)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(Hostname + url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            await using (var streamWriter = new StreamWriter(await httpWebRequest.GetRequestStreamAsync()))
            {
                await streamWriter.WriteAsync(json);
            }

            HttpWebResponse httpResponse = (HttpWebResponse)await httpWebRequest.GetResponseAsync();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                return await streamReader.ReadToEndAsync();
            }
        }

        public static byte[] MergeByteArrays(params object[] list)
        {
            int totalBytesLength = 0;
            for (int i = 0; i < list.Length; i++)
                totalBytesLength += ((byte[])list[i]).Length;

            byte[] result = new byte[totalBytesLength];
            int pos = 0;
            for (int i = 0; i < list.Length; i++)
            {
                byte[] thisArray = (byte[])list[i];
                thisArray.CopyTo(result, pos);
                pos += thisArray.Length;
            }
            return result;
        }

        public static byte[] ToBytes(Command rpc)
        {
            return new byte[1] { (byte)rpc };
        }

        public static byte[] WriteMmoString(string str)
        {
            return MergeByteArrays(ToBytes(Encoding.UTF8.GetBytes(str).Length), Encoding.UTF8.GetBytes(str));
        }

        public static byte[] ToBytes(int num)
        {
            byte[] bytes = BitConverter.GetBytes(num);
            return bytes;
        }

        public static byte[] ToBytes(float num)
        {
            byte[] bytes = BitConverter.GetBytes(num);
            if (!BitConverter.IsLittleEndian)
            {
                bytes = bytes.Reverse().ToArray();
            }
            return bytes;
        }
    }
}