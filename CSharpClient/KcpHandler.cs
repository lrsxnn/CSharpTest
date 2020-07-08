using System;
using System.Buffers;
using System.Net.Sockets.Kcp;

namespace CSharpTest
{
    public class KcpHandler : IKcpCallback
    {
        public Action<Memory<byte>> Out;
        public Action<byte[]> Recv;

        public void Receive(byte[] buffer)
        {
            Recv(buffer);
        }

        public IMemoryOwner<byte> RentBuffer(int length)
        {
            return null;
        }

        public void Output(IMemoryOwner<byte> buffer, int avalidLength)
        {
            using (buffer)
            {
                Out(buffer.Memory.Slice(0, avalidLength));
            }
        }
    }
}