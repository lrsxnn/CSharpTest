using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

namespace CSharpServer
{
    public class TcpSocket
    {
        private Socket socket;

        public void Connect()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Any, 8881));
            socket.Listen(0);

            Console.WriteLine("TCP服务端启动");

            Thread threadAccept = new Thread(Accept);
            threadAccept.IsBackground = true;
            threadAccept.Start();
        }

        private void Accept()
        {
            Socket client = socket.Accept();
            IPEndPoint ipep = client.RemoteEndPoint as IPEndPoint;
            Console.WriteLine("[{0}:{1}]: connect", ipep.Address, ipep.Port);

            Thread threadReceive = new Thread(Receive);
            threadReceive.IsBackground = true;
            threadReceive.Start(client);

            Accept();
        }

        private void Receive(object obj)
        {
            Socket client = obj as Socket;
            IPEndPoint ipep = client.RemoteEndPoint as IPEndPoint;
            IPAddress ip = ipep.Address;
            int port = ipep.Port;
            try
            {
                byte[] buffer = new byte[1024];
                int buflen = client.Receive(buffer);
                string recvMsg = Encoding.ASCII.GetString(buffer, 0, buflen);

                byte[] sendMsg = Encoding.ASCII.GetBytes(recvMsg + " " + DateTime.Now);
                client.Send(sendMsg);

                Receive(client);
            }
            catch
            {
                Console.WriteLine("[{0}:{1}]: close", ip, port);
            }
        }
    }
}