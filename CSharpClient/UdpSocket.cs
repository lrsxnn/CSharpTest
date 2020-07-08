using System;
using System.Net;
using System.Net.Sockets;
using System.Net.Sockets.Kcp;
using System.Collections.Generic;
using System.Collections;



using System.Buffers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace CSharpTest
{
    public class UdpSocket
    {
        private Socket socket;

        public void Connect()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            Thread threadReceive = new Thread(Receive);
            threadReceive.IsBackground = true;
            threadReceive.Start();

            EndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8881);
            byte[] buffer = Encoding.ASCII.GetBytes("hello tan da shen");
            socket.SendTo(buffer, ep);
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