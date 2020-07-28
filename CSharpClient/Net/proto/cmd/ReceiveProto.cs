
using Google.Protobuf;
using Lspb;
using System;
using System.Collections.Generic;

namespace CSharpClient.Net.Cmd
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
            SrvRes res = Deserialize<SrvRes>(buffer);
            switch (res.MethodId)
            {
                case SrvMsgType.SrvEnterRoom:
                    SrvEnterRoom(res.SrvEnterRoom, res.Result, res.ErrStr);
                    break;
                case SrvMsgType.SrvInitOver:
                    SrvInitOver(res.SrvInitOver, res.Result, res.ErrStr);
                    break;
                case SrvMsgType.BGameInit:
                    BGameInit(res.BGameInit, res.Result, res.ErrStr);
                    break;
                case SrvMsgType.BGameStart:
                    BGameStart(res.BGameStart, res.Result, res.ErrStr);
                    break;
                case SrvMsgType.BGameFrame:
                    BGameFrame(res.BGameFrame, res.Result, res.ErrStr);
                    break;
                default:
                    Console.WriteLine("proto error no {0}", res.MethodId);
                    break;
            }
        }

        protected virtual void SrvEnterRoom(SrvEnterRoom msg, Result result, string errStr)
        {
            Console.WriteLine("-----------------------no implements SrvEnterRoom-----------------------");
        }

        protected virtual void SrvInitOver(SrvInitOver msg, Result result, string errStr)
        {
            Console.WriteLine("-----------------------no implements SrvInitOver-----------------------");
        }

        protected virtual void BGameInit(BGameInit msg, Result result, string errStr)
        {
            Console.WriteLine("-----------------------no implements BGameInit-----------------------");
        }

        protected virtual void BGameStart(BGameStart msg, Result result, string errStr)
        {
            Console.WriteLine("-----------------------no implements BGameStart-----------------------");
        }

        protected virtual void BGameFrame(BGameFrame msg, Result result, string errStr)
        {
            Console.WriteLine("-----------------------no implements BGameFrame-----------------------");
        }

    }
}
