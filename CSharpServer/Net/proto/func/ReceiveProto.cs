using Lspb;
using System;
using CSharpServer.Net.Cmd;

namespace CSharpServer.Net.func
{
    public class ReceiveProto : CSharpServer.Net.Cmd.ReceiveProto
    {
        public override void CliEnterRoom(CliEnterRoom msg)
        {
            // Program.server.Send(SendProto.Serialize(SendProto.SrvEnterRoom(1)));
            var req = SendProto.SrvEnterRoom(1);
            byte[] buffer = SendProto.Serialize(req);
            Program.server.Send(buffer);
        }
        public override void CliInitOver(CliInitOver msg)
        {
            Console.WriteLine("CliInitOver");
        }
        public override void CliOperate(CliOperate msg)
        {
            Console.WriteLine("CliOperate");
        }
    }
}