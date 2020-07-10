using Lspb;
using System;

namespace CSharpClient.Net.func
{
    public class ReceiveProto : CSharpClient.Net.Cmd.ReceiveProto
    {
        public override void SrvEnterRoom(SrvEnterRoom msg, Result result, string errStr)
        {
            Console.WriteLine("SrvEnterRoom");
        }
        public override void SrvInitOver(SrvInitOver msg, Result result, string errStr)
        {
            Console.WriteLine("SrvInitOver");
        }
        public override void BGameInit(BGameInit msg, Result result, string errStr)
        {
            Console.WriteLine("BGameInit");
        }
        public override void BGameStart(BGameStart msg, Result result, string errStr)
        {
            Console.WriteLine("BGameStart");
        }
        public override void BGameFrame(BGameFrame msg, Result result, string errStr)
        {
            Console.WriteLine("BGameFrame");
        }
    }
}