
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

        public static SrvRes BGameInit(int seed = 0, List<PlayerInfo> pList = null, Result result = Result.Success, string errStr = "")
        {
            BGameInit bGameInit = new BGameInit();
            bGameInit.Seed = seed;
            bGameInit.PList.AddRange(pList);

            SrvRes res = CreateSrvRes(SrvMsgType.BGameInit, result, errStr);
            res.BGameInit = bGameInit;
            return res;
        }

        public static SrvRes SrvEnterRoom(int playerId = 0, Result result = Result.Success, string errStr = "")
        {
            SrvEnterRoom srvEnterRoom = new SrvEnterRoom();
            srvEnterRoom.PlayerId = playerId;

            SrvRes res = CreateSrvRes(SrvMsgType.SrvEnterRoom, result, errStr);
            res.SrvEnterRoom = srvEnterRoom;
            return res;
        }

        public static SrvRes SrvInitOver(Result result = Result.Success, string errStr = "")
        {
            SrvInitOver srvInitOver = new SrvInitOver();

            SrvRes res = CreateSrvRes(SrvMsgType.SrvInitOver, result, errStr);
            res.SrvInitOver = srvInitOver;
            return res;
        }

        public static SrvRes BGameFrame(int fId = 0, List<CliOperate> operList = null, Result result = Result.Success, string errStr = "")
        {
            BGameFrame bGameFrame = new BGameFrame();
            bGameFrame.FId = fId;
            bGameFrame.OperList.AddRange(operList);

            SrvRes res = CreateSrvRes(SrvMsgType.BGameFrame, result, errStr);
            res.BGameFrame = bGameFrame;
            return res;
        }

        public static SrvRes BGameStart(Result result = Result.Success, string errStr = "")
        {
            BGameStart bGameStart = new BGameStart();

            SrvRes res = CreateSrvRes(SrvMsgType.BGameStart, result, errStr);
            res.BGameStart = bGameStart;
            return res;
        }

    }
}
