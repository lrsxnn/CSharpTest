using Lspb;
using System;
using CSharpServer.Net;
using CSharpServer.Net.Cmd;

namespace CSharpServer.Net.func
{
    public class ReceiveProto : CSharpServer.Net.Cmd.ReceiveProto
    {
        protected override void CliEnterRoom(CliEnterRoom msg)
        {
            // Program.server.Send(SendProto.Serialize(SendProto.SrvEnterRoom(1)));
            SendProto.SrvEnterRoom(1);
        }
        protected override void CliInitOver(CliInitOver msg)
        {
            Console.WriteLine("CliInitOver");
        }
        protected override void CliOperate(CliOperate msg)
        {
            Console.WriteLine("CliOperate");
        }
    }
}