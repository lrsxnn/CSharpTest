using System;
using CSharpServer.Net;

namespace CSharpServer
{
    class Program
    {
        public static UdpSocket server;
        static void Main(string[] args)
        {
            // Console.WriteLine("Hello World!");
            server = new UdpSocket();
            server.Connect();
            Console.ReadKey();
        }
    }
}
