using System;
using System.Net;
using System.Net.Sockets;
using System.Net.Sockets.Kcp;

using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using CSharpClient.Net.func;

namespace CSharpClient.Net
{
    public class UdpSocket
    {
        private static Socket socket;
        private static Kcp mKcp;
        private static KcpHandler mHandler;
        private static ReceiveProto proto;
        private static EndPoint ep;


        public static void ConnectServer(string address, int port)
        {
            Regex regex = new Regex(@"^((2(5[0-5]|[0-4]\d))|[0-1]?\d{1,2})(\.((2(5[0-5]|[0-4]\d))|[0-1]?\d{1,2})){3}$");//判断是否为IP地址
            if (regex.IsMatch(address))
            {
                ConnectByIP(address, port);
            }
            else
            {
                ConnectByHost(address, port);
            }
        }

        private static void ConnectByIP(string ip, int port)
        {
            try
            {
                IPAddress iPAddress = IPAddress.Parse(ip);
                IPEndPoint ipep = new IPEndPoint(iPAddress, port);
                Connect(ipep);
            }
            catch (Exception e)
            {
                Console.WriteLine("[UdpSocket] error {0}", e.ToString());
            }
        }

        private static void ConnectByHost(string host, int port)
        {
            IPHostEntry iphe = null;
            try
            {
                iphe = Dns.GetHostEntry(host);
                IPEndPoint ipep = new IPEndPoint(iphe.AddressList[0], port);
                Connect(ipep);
            }
            catch (Exception e)
            {
                Console.WriteLine("[UdpSocket] error {0}", e.ToString());
            }
        }

        private static void Connect(IPEndPoint ipep)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.Bind(new IPEndPoint(IPAddress.Any, 11000));

            EndPoint serverEP = ipep;

            mHandler = new KcpHandler();
            mKcp = new Kcp(10, mHandler);
            mKcp.NoDelay(1, 10, 2, 1);
            mKcp.WndSize(64, 64);
            mKcp.SetMtu(512);

            proto = new ReceiveProto();

            mHandler.Out += buffer =>
            {
                Console.WriteLine("mHandler Out");
                try
                {
                    socket.SendTo(buffer.ToArray(), serverEP);
                }
                catch (SocketException e)
                {
                    Console.WriteLine("[UdpSocket] socket send error {0}", e.ToString());
                }
            };

            mHandler.Recv += buffer =>
            {
                proto.Receive(buffer);
            };

            Task.Run(async () =>
            {
                try
                {
                    while (true)
                    {
                        mKcp.Update(DateTime.UtcNow);
                        int len;
                        //方法1
                        while ((len = mKcp.PeekSize()) > 0)
                        {
                            byte[] buffer = new byte[len];
                            if (mKcp.Recv(buffer) >= 0)
                            {
                                Console.WriteLine("mHandler Receive");
                                mHandler.Receive(buffer);
                            }
                        }
                        //方法2
                        // do
                        // {
                        //     var (buffer, avalidSize) = mKcp.TryRecv();
                        //     len = avalidSize;
                        //     if (buffer != null)
                        //     {
                        //         var temp = new byte[len];
                        //         buffer.Memory.Span.Slice(0, len).CopyTo(temp);
                        //         Console.WriteLine("mKcp Receive");
                        //         mHandler.Receive(temp);
                        //     }
                        // } while (len > 0);

                        await Task.Delay(5);
                    }
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            });

            ep = new IPEndPoint(IPAddress.Any, 0);

            Thread threadReceive = new Thread(Receive);
            threadReceive.IsBackground = true;
            threadReceive.Start();
        }

        private static void Receive()
        {
            try
            {
                byte[] data = new byte[1024];
                int buflen = socket.ReceiveFrom(data, ref ep);
                Console.WriteLine("收到UDP消息长度{0} IP{1} port{2}", buflen, (ep as IPEndPoint).Address, (ep as IPEndPoint).Port);
                Console.WriteLine("mKcp Input");
                Span<byte> buffer = data;
                mKcp.Input(buffer);

                Receive();
            }
            catch (Exception e)
            {
                Console.WriteLine("[UdpSocket] socket receive error {0}", e.ToString());
            }
        }

        public static void Send(byte[] buffer)
        {
            mKcp.Send(buffer);
        }
    }
}