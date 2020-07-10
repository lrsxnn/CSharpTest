using Google.Protobuf;
using Lspb;
using System.Collections.Generic;

namespace CSharpServer.Net.Cmd
{
    public class SendProto
    {
        public static byte[] Serialize<T>(T obj) where T : IMessage
        {
            return obj.ToByteArray();
        }

        private static SrvRes CreateSrvRes(SrvMsgType methodId, Result result = Result.Success, string errStr = "")
        {
            return new SrvRes
            {
                MethodId = methodId,
                Result = result,
                ErrStr = errStr,
            };
        }
        public static SrvRes SrvEnterRoom(int playerId)
        {
            SrvEnterRoom srvEnterRoom = new SrvEnterRoom();
            srvEnterRoom.PlayerId = playerId;

            SrvRes res = CreateSrvRes(SrvMsgType.SrvEnterRoom);
            res.SrvEnterRoom = srvEnterRoom;
            return res;
        }

        public static SrvRes SrvInitOver()
        {
            SrvInitOver srvInitOver = new SrvInitOver();

            SrvRes res = CreateSrvRes(SrvMsgType.SrvInitOver);
            res.SrvInitOver = srvInitOver;
            return res;
        }

        public static SrvRes BGameInit(int seed, IEnumerable<PlayerInfo> pList)
        {
            BGameInit bGameInit = new BGameInit();
            bGameInit.Seed = seed;
            bGameInit.PList.AddRange(pList);

            SrvRes res = CreateSrvRes(SrvMsgType.BGameInit);
            res.BGameInit = bGameInit;
            return res;
        }

        public static SrvRes BGameStart()
        {
            BGameStart bGameStart = new BGameStart();

            SrvRes res = CreateSrvRes(SrvMsgType.BGameStart);
            res.BGameStart = bGameStart;
            return res;
        }

        public static SrvRes BGameFrame(int fId, IEnumerable<CliOperate> operList)
        {
            BGameFrame bGameFrame = new BGameFrame();
            bGameFrame.FId = fId;
            bGameFrame.OperList.AddRange(operList);

            SrvRes res = CreateSrvRes(SrvMsgType.BGameFrame);
            res.BGameFrame = bGameFrame;
            return res;
        }
    }
}