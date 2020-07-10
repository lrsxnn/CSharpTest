using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

namespace CSharpClient
{
    public class TcpSocket
    {
        private static Socket Connect(IPEndPoint ipep)
        {
            Socket socket = null;
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ipep);
                if (!socket.Connected)
                {
                    socket = null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[TcpSocket] error {0}", e.ToString());
            }
            return socket;
        }

        public static Socket ConnectByIP(string ip, int port)
        {
            try
            {
                IPAddress iPAddress = IPAddress.Parse(ip);
                IPEndPoint ipep = new IPEndPoint(iPAddress, port);
                return Connect(ipep);
            }
            catch (Exception e)
            {
                Console.WriteLine("[TcpSocket] error {0}", e.ToString());
                return null;
            }
        }

        public static Socket ConnectByHost(string host, int port)
        {
            IPHostEntry iphe = null;
            try
            {
                Socket temp = null;
                iphe = Dns.GetHostEntry(host);
                foreach (IPAddress ipa in iphe.AddressList)
                {
                    IPEndPoint ipep = new IPEndPoint(ipa, port);
                    temp = Connect(ipep);
                    if (temp == null)
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                return temp;
            }
            catch (Exception e)
            {
                Console.WriteLine("[TcpSocket] error {0}", e.ToString());
                return null;
            }
        }
    }
}