using System;
using System.Net;
using System.Net.Sockets;
using System.Net.Sockets.Kcp;

using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpTest
{
    public class UdpSocket
    {
        private Socket socket;
        private Kcp mKcp;
        private KcpHandler mHandler;
        // EndPoint ep = new IPEndPoint(Dns.GetHostEntry("dztest.runfagame.cn").AddressList[0], 26379);
        EndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8880);

        public void Connect()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.Bind(new IPEndPoint(IPAddress.Any, 11000));

            mHandler = new KcpHandler();
            mKcp = new Kcp(10, mHandler);
            mKcp.NoDelay(1, 10, 2, 1);
            mKcp.WndSize(64, 64);
            mKcp.SetMtu(512);

            mHandler.Out += buffer =>
            {
                Console.WriteLine("mHandler Out");
                socket.SendTo(buffer.ToArray(), ep);
            };

            mHandler.Recv += buffer =>
            {
                string str = Encoding.ASCII.GetString(buffer);
                Console.WriteLine("mHandler Recv {0}", str);
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

            Thread threadReceive = new Thread(Receive);
            threadReceive.IsBackground = true;
            threadReceive.Start();

            var sendbyte = Encoding.ASCII.GetBytes("hello tan da shen");
            mKcp.Send(sendbyte);
        }

        private void Receive()
        {
            EndPoint ep = new IPEndPoint(IPAddress.Any, 0);
            try
            {
                byte[] data = new byte[1024];
                int buflen = socket.ReceiveFrom(data, ref ep);
                string recvMsg = Encoding.ASCII.GetString(data, 0, buflen);
                Console.WriteLine("收到UDP消息{0} IP{1} port{2}", recvMsg, (ep as IPEndPoint).Address, (ep as IPEndPoint).Port);
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

        public void TestKCP()
        {
            const int echoTimes = 3;

            KcpHandler chandler = new KcpHandler();
            const int conv = 1;
            Kcp cKcp = new Kcp(conv, chandler);
            cKcp.NoDelay(1, 10, 2, 1);
            cKcp.WndSize(64, 64);
            cKcp.SetMtu(512);

            KcpHandler shandler = new KcpHandler();
            Kcp sKcp = new Kcp(conv, shandler);
            sKcp.NoDelay(1, 10, 2, 1);
            sKcp.WndSize(64, 64);
            sKcp.SetMtu(512);

            int end = 0;

            chandler.Out += buffer =>
            {
                Console.WriteLine("cKcp Out");
                sKcp.Input(buffer.Span);
            };

            chandler.Recv += buffer =>
            {
                Console.WriteLine("cKcp Recv");
                string str = Encoding.ASCII.GetString(buffer);
                Interlocked.Increment(ref end);
                if (end < echoTimes)
                {
                    cKcp.Send(buffer);
                }
            };

            shandler.Out += buffer =>
            {
                Console.WriteLine("sKcp Out");
                cKcp.Input(buffer.Span);
            };
            shandler.Recv += buffer =>
            {
                Console.WriteLine("sKcp Recv");
                sKcp.Send(buffer);
            };

            Task.Run(async () =>
            {
                try
                {
                    while (true)
                    {
                        cKcp.Update(DateTime.UtcNow);
                        int len;
                        while ((len = cKcp.PeekSize()) > 0)
                        {
                            byte[] buffer = new byte[len];
                            if (cKcp.Recv(buffer) >= 0)
                            {
                                Console.WriteLine("cKcp Receive");
                                chandler.Receive(buffer);
                            }
                        }
                        await Task.Delay(5);
                    }
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            });

            Task.Run(async () =>
            {
                try
                {
                    while (true)
                    {
                        sKcp.Update(DateTime.UtcNow);
                        int len;
                        do
                        {
                            var (buffer, avalidSize) = sKcp.TryRecv();
                            len = avalidSize;
                            if (buffer != null)
                            {
                                var temp = new byte[len];
                                buffer.Memory.Span.Slice(0, len).CopyTo(temp);
                                Console.WriteLine("sKcp Receive");
                                shandler.Receive(temp);
                            }
                        } while (len > 0);
                        await Task.Delay(5);
                    }
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            });

            var sendbyte = Encoding.ASCII.GetBytes("hello" + end.ToString());
            cKcp.Send(sendbyte);

            var task = Task.Run(async () =>
            {
                while (end < echoTimes)
                {
                    await Task.Yield();
                }
                return 1;
            });

            task.Result.ToString();
        }
    }
}