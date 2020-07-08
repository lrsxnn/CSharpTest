using System;

namespace CSharpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Console.WriteLine("Hello World!");
            UdpSocket server = new UdpSocket();
            server.Connect();
            Console.ReadKey();
        }
    }
}
