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

        public void Decode(byte[] buffer)
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

        public virtual void SrvEnterRoom(SrvEnterRoom msg, Result result, string errStr)
        {
            Console.WriteLine("SrvEnterRoom no instantiation");
        }
        public virtual void SrvInitOver(SrvInitOver msg, Result result, string errStr)
        {
            Console.WriteLine("SrvInitOver no instantiation");
        }
        public virtual void BGameInit(BGameInit msg, Result result, string errStr)
        {
            Console.WriteLine("BGameInit no instantiation");
        }
        public virtual void BGameStart(BGameStart msg, Result result, string errStr)
        {
            Console.WriteLine("BGameStart no instantiation");
        }
        public virtual void BGameFrame(BGameFrame msg, Result result, string errStr)
        {
            Console.WriteLine("BGameFrame no instantiation");
        }

        public virtual void CliEnterRoom(CliEnterRoom msg)
        {
            Console.WriteLine("CliEnterRoom no instantiation");
        }

        public virtual void CliInitOver(CliInitOver msg)
        {
            Console.WriteLine("CliInitOver no instantiation");
        }

        public virtual void CliOperate(CliOperate msg)
        {
            Console.WriteLine("CliOperate no instantiation");
        }
    }
}