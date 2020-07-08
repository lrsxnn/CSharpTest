using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

namespace CSharpServer
{
    public class UdpSocket
    {
        private Socket socket;

        public void Connect()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.Bind(new IPEndPoint(IPAddress.Any, 8881));

            Console.WriteLine("UDP服务端启动");

            Thread threadReceive = new Thread(Receive);
            threadReceive.IsBackground = true;
            threadReceive.Start();
        }

        private void Receive()
        {
            EndPoint ep = new IPEndPoint(IPAddress.Any, 0);
            try
            {
                byte[] buffer = new byte[1024];
                int buflen = socket.ReceiveFrom(buffer, ref ep);
                string recvMsg = Encoding.ASCII.GetString(buffer, 0, buflen);
                Console.WriteLine("收到UDP消息{0}", recvMsg);
                byte[] sendMsg = Encoding.ASCII.GetBytes(recvMsg + " " + DateTime.Now);
                socket.SendTo(sendMsg, ep);

                Receive();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}