using System;
using CSharpServer.Net;

namespace CSharpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Console.WriteLine("Hello World!");
            UdpSocket.Connect();
            Console.ReadKey();
        }
    }
}
