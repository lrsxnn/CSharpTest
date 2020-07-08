using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace CSharpTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // IPHostEntry hostEntry = Dns.GetHostEntry("dztest.runfagame.cn");
            // IPEndPoint ipEndPoint = new IPEndPoint(hostEntry.AddressList[0], 0);

            // Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // try
            // {
            //     socket.Connect(ipEndPoint.Address.ToString(), 26379);
            // }
            // catch (SocketException e)
            // {
            //     Console.WriteLine(e.ToString());
            //     return;
            // }

            // Console.WriteLine("connect success");
            // socket.Close();

            var socket = new UdpSocket();
            // socket.TestKCP();
            socket.Connect();
            Console.ReadKey();
        }
    }
}
