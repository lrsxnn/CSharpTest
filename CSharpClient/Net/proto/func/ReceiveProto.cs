using Lspb;
using System;

namespace CSharpClient.Net.func
{
    public class ReceiveProto : CSharpClient.Net.Cmd.ReceiveProto
    {
        protected override void SrvEnterRoom(SrvEnterRoom msg, Result result, string errStr)
        {
            Console.WriteLine("SrvEnterRoom");
        }
        protected override void SrvInitOver(SrvInitOver msg, Result result, string errStr)
        {
            Console.WriteLine("SrvInitOver");
        }
        protected override void BGameInit(BGameInit msg, Result result, string errStr)
        {
            Console.WriteLine("BGameInit");
        }
        protected override void BGameStart(BGameStart msg, Result result, string errStr)
        {
            Console.WriteLine("BGameStart");
        }
        protected override void BGameFrame(BGameFrame msg, Result result, string errStr)
        {
            Console.WriteLine("BGameFrame");
        }
    }
}