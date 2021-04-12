using System;

namespace COO.SocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start Socket!");
            ChatServer.Start();
        }
    }
}
