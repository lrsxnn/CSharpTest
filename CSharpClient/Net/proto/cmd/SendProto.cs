using Google.Protobuf;
using Lspb;
using System.Collections.Generic;

namespace CSharpClient.Net.Cmd
{
    public class SendProto
    {
        public static byte[] Serialize<T>(T obj) where T : IMessage
        {
            return obj.ToByteArray();
        }

        private static CliReq CreateCliReq(ClientMsgType methodId, int userId = 1, ModuleId moduleId = ModuleId.Game)
        {
            return new CliReq
            {
                UserId = userId,
                ModuleId = moduleId,
                MethodId = methodId,
            };
        }

        public static CliReq CliEnterRoom(int roomId, string name)
        {
            CliEnterRoom cliEnterRoom = new CliEnterRoom();
            cliEnterRoom.RoomId = roomId;
            cliEnterRoom.Name = name;

            CliReq req = CreateCliReq(ClientMsgType.CliEnterRoom);
            req.CliEnterRoom = cliEnterRoom;
            return req;
        }

        public static CliReq CliInitOver()
        {
            CliInitOver cliInitOver = new CliInitOver();

            CliReq req = CreateCliReq(ClientMsgType.CliInitOver);
            req.CliInitOver = cliInitOver;
            return req;
        }

        public static CliReq CliOperate(float direction, bool isFire)
        {
            CliOperate cliOperate = new CliOperate();
            cliOperate.Direction = direction;
            cliOperate.IsFire = isFire;

            CliReq req = CreateCliReq(ClientMsgType.CliOperate);
            req.CliOperate = cliOperate;
            return req;
        }
    }
}