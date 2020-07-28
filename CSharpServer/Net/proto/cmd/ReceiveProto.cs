
using Google.Protobuf;
using Lspb;
using System;
using System.Collections.Generic;

namespace CSharpServer.Net.Cmd
{
    public class ReceiveProto
    {
        private static T Deserialize<T>(byte[] data) where T : class, IMessage, new()
        {
            T obj = new T();
            IMessage message = obj.Descriptor.Parser.ParseFrom(data);
            return message as T;
        }

        public void Receive(byte[] buffer)
        {
            CliReq req = Deserialize<CliReq>(buffer);
            switch (req.MethodId)
            {
                case ClientMsgType.CliEnterRoom:
                    CliEnterRoom(req.CliEnterRoom);
                    break;
                case ClientMsgType.CliInitOver:
                    CliInitOver(req.CliInitOver);
                    break;
                case ClientMsgType.CliOperate:
                    CliOperate(req.CliOperate);
                    break;
                default:
                    Console.WriteLine("proto error no {0}", req.MethodId);
                    break;
            }
        }

        protected virtual void CliEnterRoom(CliEnterRoom msg)
        {
            Console.WriteLine("-----------------------no implements CliEnterRoom-----------------------");
        }

        protected virtual void CliInitOver(CliInitOver msg)
        {
            Console.WriteLine("-----------------------no implements CliInitOver-----------------------");
        }

        protected virtual void CliOperate(CliOperate msg)
        {
            Console.WriteLine("-----------------------no implements CliOperate-----------------------");
        }

    }
}
