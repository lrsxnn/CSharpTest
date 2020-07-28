using System;
using System.Net;
using System.Net.Sockets;
using System.Net.Sockets.Kcp;

using System.Text;
using System.Threading;
using System.Threading.Tasks;

using CSharpServer.Net.func;

namespace CSharpServer.Net
{
    public class UdpSocket
    {
        private static Socket socket;
        private static Kcp mKcp;
        private static KcpHandler mHandler;
        private static EndPoint ep;
        private static ReceiveProto proto;
        private static byte[] data;
        private static int buflen;
        public static void Connect()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.Bind(new IPEndPoint(IPAddress.Any, 8880));

            Console.WriteLine("UDP服务端启动");

            mHandler = new KcpHandler();
            mKcp = new Kcp(10, mHandler);
            mKcp.NoDelay(1, 10, 2, 1);
            mKcp.WndSize(64, 64);
            mKcp.SetMtu(512);

            proto = new ReceiveProto();

            mHandler.Out += buffer =>
            {
                Console.WriteLine("mHandler Out IP{0} port{1}", (ep as IPEndPoint).Address, (ep as IPEndPoint).Port);
                socket.SendTo(buffer.ToArray(), ep);
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
            data = new byte[1024];

            Thread threadReceive = new Thread(Receive);
            threadReceive.IsBackground = true;
            threadReceive.Start();
        }

        private static void Receive()
        {
            try
            {
                buflen = socket.ReceiveFrom(data, ref ep);

                Console.WriteLine("收到UDP消息长度{0} IP{1} port{2}", buflen, (ep as IPEndPoint).Address, (ep as IPEndPoint).Port);
                Console.WriteLine("mKcp Input");

                Span<byte> buffer = data;
                mKcp.Input(buffer);

                Receive();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void Send(byte[] buffer)
        {
            mKcp.Send(buffer);
        }
    }
}