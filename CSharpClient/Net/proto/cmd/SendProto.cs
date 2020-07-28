
using Google.Protobuf;
using Lspb;
using System.Collections.Generic;

namespace CSharpClient.Net.Cmd
{
    public class SendProto
    {
        private static byte[] Serialize<T>(T obj) where T : IMessage
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

        public static void CliEnterRoom(int roomId = 0, string name = "", int userId = 1, ModuleId moduleId = ModuleId.Game)
        {
            CliEnterRoom cliEnterRoom = new CliEnterRoom();
            cliEnterRoom.RoomId = roomId;
            cliEnterRoom.Name = name;

            CliReq req = CreateCliReq(ClientMsgType.CliEnterRoom, userId, moduleId);
            req.CliEnterRoom = cliEnterRoom;

            UdpSocket.Send(Serialize(req));
        }

        public static void CliOperate(string direction = "", bool isFire = false, int playerId = 0, int userId = 1, ModuleId moduleId = ModuleId.Game)
        {
            CliOperate cliOperate = new CliOperate();
            cliOperate.Direction = direction;
            cliOperate.IsFire = isFire;
            cliOperate.PlayerId = playerId;

            CliReq req = CreateCliReq(ClientMsgType.CliOperate, userId, moduleId);
            req.CliOperate = cliOperate;

            UdpSocket.Send(Serialize(req));
        }

        public static void CliInitOver(int userId = 1, ModuleId moduleId = ModuleId.Game)
        {
            CliInitOver cliInitOver = new CliInitOver();

            CliReq req = CreateCliReq(ClientMsgType.CliInitOver, userId, moduleId);
            req.CliInitOver = cliInitOver;

            UdpSocket.Send(Serialize(req));
        }

    }
}
