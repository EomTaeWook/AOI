﻿using AOIServer.Net;
using Kosher.Framework;
using Kosher.Log;
using Protocol.SAndC;
using Share;

namespace AOIServer.Internal
{
    internal class GameManager : Singleton<GameManager>
    {
        private readonly Map _map;
        public GameManager()
        {
            _map = new Map(5, 5);
        }
        public void EnterGame(User user)
        {
            if (user == null)
            {
                LogHelper.Error($"player is null");
                return;
            }
            var index = _map.GetZoneIndex(user.Player.CellPos);
            var zone = _map.GetZone(index);
            if(zone == null)
            {
                LogHelper.Error($"zone is null");
                return;
            }
            zone.AddUser(user);
        }
        public void LeaveGame(User user)
        {
            if(user == null)
            {
                LogHelper.Error($"player is null");
                return;
            }
            var index = _map.GetZoneIndex(user.Player.CellPos);
            var zone = _map.GetZone(index);
            if (zone == null)
            {
                LogHelper.Error($"zone is null");
                return;
            }
            zone.RemoveUser(user);
        }
        public void UpdateAroundPlayer(User user)
        {
            var zone = _map.GetZone(_map.GetZoneIndex(user.Player.CellPos));

            var cellIndex = zone.GetCellIndex(user.Player.CellPos);
            var indexList = zone.GetAroundCellByIndex(cellIndex);

            var prevAroundUsers = new HashSet<User>(user.AroundPlayers);

            var added = new List<User>();

            var currentAroundUsers = new HashSet<User>();
            foreach (var item in indexList)
            {
                var players = zone.GetPlayerByCellIndex(item);
                foreach(var player in players)
                {
                    if(prevAroundUsers.Contains(player) == false)
                    {
                        added.Add(player);
                    }
                    currentAroundUsers.Add(player);
                }
            }

            var removed = new List<User>();
            foreach (var item in prevAroundUsers)
            {
                if(currentAroundUsers.Contains(item) == false)
                {
                    removed.Add(item);
                }
            }

            foreach(var item in added)
            {
                user.Session.Send(Packet.MakePacket(SCProtocol.Spawn,
                    new Spawn()
                    {
                        Player = item.Player
                    }));

                item.Session.Send(Packet.MakePacket(SCProtocol.Spawn,
                    new Spawn()
                    {
                        Player = user.Player
                    }));
            }

            foreach (var item in removed)
            {
                user.Session.Send(Packet.MakePacket(SCProtocol.Despawn,
                    new Despawn()
                    {
                        Player = item.Player
                    }));

                item.Session.Send(Packet.MakePacket(SCProtocol.Despawn,
                    new Despawn()
                    {
                        Player = user.Player
                    }));
            }

        }
        public bool Move(User user, Vector2Int targetPosition)
        {
            var targetZone = _map.GetZone(_map.GetZoneIndex(targetPosition));
            if(targetZone.CanGo(targetPosition) == false)
            {
                return false;
            }

            var prevZone = _map.GetZone(_map.GetZoneIndex(user.Player.CellPos));
            if (prevZone == null)
            {
                LogHelper.Error($"zone is null");
                return false;
            }

            if(prevZone!= targetZone)
            {
                prevZone.RemoveUser(user);
                targetZone.AddUser(user);
            }

            prevZone.LeaveCell(user);
            targetZone.AddCell(targetPosition, user);

            return true;
        }
    }
}