using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

namespace CSharpClient
{
    public class TcpSocket
    {
        private Socket socket;

        public void Connection()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8881));
            Console.WriteLine("TCP客户端启动");

            Thread threadReceive = new Thread(Receive);
            threadReceive.IsBackground = true;
            threadReceive.Start();

            byte[] buffer = Encoding.ASCII.GetBytes("hello tan da shen");
            socket.Send(buffer);
        }

        public void Receive()
        {
            try
            {
                byte[] buffer = new byte[1024];
                int buflen = socket.Receive(buffer);
                string message = Encoding.ASCII.GetString(buffer, 0, buflen);
                Console.WriteLine("TCP客户端接收{0}", message);

                Receive();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}