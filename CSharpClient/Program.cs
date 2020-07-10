using System;
using CSharpClient.Net;
using CSharpClient.Net.Cmd;
namespace CSharpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // EndPoint ep = new IPEndPoint(Dns.GetHostEntry("dztest.runfagame.cn").AddressList[0], 55561);
            // EndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8880);

            var socket = new UdpSocket();
            socket.ConnectServer("127.0.0.1", 8880);

            var enterRoom = SendProto.CliEnterRoom(1, "坦克大战");
            socket.Send(SendProto.Serialize(enterRoom));
            Console.ReadKey();
        }
    }
}
