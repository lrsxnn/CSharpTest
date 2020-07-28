using System;
using CSharpClient.Net;
using CSharpClient.Net.Cmd;
namespace CSharpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // UdpSocket.ConnectServer("dztest.runfagame.cn", 55561);
            UdpSocket.ConnectServer("127.0.0.1", 8880);

            SendProto.CliEnterRoom(1, "坦克大战");
            Console.ReadKey();
        }
    }
}
