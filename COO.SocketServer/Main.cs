using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace COO.SocketServer
{
    public partial class ChatServer
    {
        static readonly ManualResetEvent WaitForConnectionEvent = new ManualResetEvent(false);

        const int MaxGroupSize = 5;
        static readonly List<Player> Players = new List<Player>();
        static readonly List<Socket> GameServers = new List<Socket>();
        static readonly List<Group> Groups = new List<Group>();

        static readonly Dictionary<string, System.Timers.Timer> PendingKicks = new Dictionary<string, System.Timers.Timer>();
        static readonly Dictionary<Player, System.Timers.Timer> PendingInvites = new Dictionary<Player, System.Timers.Timer>();

        static readonly Dictionary<string, int> CharacterIds = new Dictionary<string, int>(); // [character name : character id] - Only characters that are in clans are in this dictionary!
        static readonly Dictionary<int, int> CharacterClans = new Dictionary<int,int>(); // [character id - clan id]
        static readonly HashSet<int> ClanLeaders = new HashSet<int>(); //ids of the characters which are clan leaders
        static readonly Dictionary<int, string> ClanNames = new Dictionary<int,string>(); //[clan id - clan name]

        static string Hostname = "http://localhost:55326/"; //url is loaded from hostname.ini

        public static void Start()
        {
            //using (var myFile = new StreamReader("hostname.ini"))
            //{
            //    Hostname = myFile.ReadToEnd(); // Read the file as one string            
            //}
            Console.WriteLine("url: " + Hostname);
            ConQue.Enqueue(StartClansUpdate); // download clans from DB on first launch

            _ = ProcessorLoop(); // launch the loop that will 'tick' once every 8ms. It will process Actions on the concurrent queue.

            StartListening(); // Each connection will run asynchronously.
        }

        public static void StartListening()
        {
            // People can connect from any IP, to the 3457 port.
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 3457);

            // Create an ipv4 TCP/IP socket
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    // Set the event to non signaled state.
                    WaitForConnectionEvent.Reset();

                    // Start an asynchronous socket to listen for connections.
                    //Console.WriteLine("Waiting for a new connection...");
                    listener.BeginAccept(
                        AcceptCallback,
                        listener);

                    // Wait until a connection is made before continuing.
                    WaitForConnectionEvent.WaitOne();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            WaitForConnectionEvent.Set();

            // Get the socket that handles the client request.
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object.
            StateObject state = new StateObject();
            state.WorkSocket = handler;
            handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, ReadMessage, state);
        }

        public static void ReadMessage(IAsyncResult ar)
        {
            // Retrieve the state object and the handler socket from the asynchronous state object.
            StateObject state = (StateObject)ar.AsyncState;
            Socket connection = state.WorkSocket;
            if (!connection.Connected)
                return;
            try
            {
                int bytesRead = connection.EndReceive(ar);

                // disconnect happened
                if (bytesRead == 0)
                {
                    ConQue.Enqueue(() => KickConnection(connection));
                    connection.Close();
                    return;
                }

                //Read the first byte to determine the type of "command" we received and switch on it
                Stream stream = new MemoryStream(state.Buffer);
                BinaryReader reader = new BinaryReader(stream);
                while (reader.PeekChar() > 0) // -1 is "end of stream", "0" is "undefined"
                {
                    Command cmd = (Command)reader.ReadByte();
                    switch (cmd)
                    {                
                        case Command.Login: ReadLogin(reader, connection); break;
                        //case Command.Logout: break; // no need, a dropped connection is caught elsewhere
                        case Command.GeneralMessage: ReadGeneralMessage(reader, connection); break;
                        case Command.PrivateMessage: ReadPrivateMessage(reader, connection); break;
                        case Command.GroupMessage: ReadGroupMessage(reader, connection); break;
                        case Command.ClanMessage: ReadClanMessage(reader, connection); break;
                        case Command.GroupInvite: ReadGroupInvite(reader, connection); break;
                        case Command.AcceptInvite: ReadAcceptInvite(connection); break; // accept group invite or clan invite
                        case Command.DeclineInvite: ReadDeclineInvite(connection); break;
                        case Command.ClanCreate: ReadClanCreate(reader, connection); break;
                        case Command.ClanInvite: ReadClanInvite(reader, connection); break;
                        case Command.ClanDisband: ReadClanDisband(connection); break;
                        case Command.ClanLeave: ReadClanLeave(connection); break;
                        case Command.GroupLeave: ReadGroupLeave(connection); break;
                        case Command.ClanKick: ReadClanKick(reader, connection); break;
                        case Command.GroupKick: ReadGroupKick(reader, connection); break;
                    }
                }

                // listen again:          
                state.ResetBuffer();
                connection.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, ReadMessage, state);
            }
            catch (SocketException socketException)
            {
                //WSAECONNRESET, the other side closed impolitely 
                if (socketException.ErrorCode == 10054 ||
                    ((socketException.ErrorCode != 10004) &&
                     (socketException.ErrorCode != 10053)) )
                {
                    Console.WriteLine("remote client disconnected");
                }
                else Console.WriteLine(socketException.Message);
                ConQue.Enqueue(() => KickPlayer(connection));
                connection.Close(); //@TODO: is this necessary?
            }
            catch (EndOfStreamException)
            {
                Console.WriteLine("EndOfStreamException exception");
                // listen again:   
                state.ResetBuffer();
                connection.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, ReadMessage, state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
                ConQue.Enqueue(() => KickPlayer(connection));
                connection.Close();//@TODO: check if this is necessary
            }        
        }
    }
}