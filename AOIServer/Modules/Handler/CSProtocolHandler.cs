﻿using AOIServer.Internal;
using AOIServer.Net;
using Kosher.Sockets;
using Kosher.Sockets.Interface;
using Protocol.CAndS;
using Protocol.SAndC;
using Share;
using System.Text.Json;

namespace AOIServer.Modules.Handler
{
    public partial class CSProtocolHandler : ISessionComponent
    {
        private User User { get; set; }
        public Session Session { get; private set; }
        public T DeserializeBody<T>(string body)
        {
            return JsonSerializer.Deserialize<T>(body);
        }
        
        public void Process(Move packet)
        {
            var targetPosition = User.Player.CellPos + new Vector2Int(packet.X, packet.Y);

            var result = GameManager.Instance.Move(User,
                targetPosition);

            Session.Send(Packet.MakePacket(SCProtocol.MoveResponse,
                new MoveResponse()
                {
                    Ok = result,
                    X = packet.X,
                    Y = packet.Y
                }));

            GameManager.Instance.UpdateAroundPlayer(User);

        }
        public void Process(Login packet)
        {
            this.User = new User(packet.Player, Session);
            GameManager.Instance.EnterGame(User);
        }
        public void SetSession(Session session)
        {
            Session = session;
        }

        public void Dispose()
        {
            GameManager.Instance.LeaveGame(User);
        }
    }
}
