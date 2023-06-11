using AOIServer.Net;
using Dignus.Framework;
using Dignus.Log;
using Protocol.SAndC;
using Share;
using System.Collections.Concurrent;

namespace AOIServer.Internal
{
    public class GameManager : Singleton<GameManager>
    {
        private readonly Map _map;
        private ConcurrentDictionary<string, User> _userDatas = new ConcurrentDictionary<string, User>();
        public Map GetMap()
        {
            return _map;
        }
        public GameManager()
        {
            _map = new Map(0, 10, 0, 10);
        }
        public void EnterGame(User user)
        {
            if (user == null)
            {
                LogHelper.Error($"player is null");
                return;
            }
            _userDatas.TryAdd(user.Player.Nickname, user);

            _map.AddPlayerInCell(user.Player.CellPos, user.Player);
        }
        public void LeaveGame(User user)
        {
            if(user.Player == null)
            {
                LogHelper.Error($"player is null");
                return;
            }
            _map.RemovePlayerInCell(user.Player);

            _userDatas.Remove(user.Player.Nickname, out User _);

            foreach (var item in user.AroundPlayers)
            {
                if (_userDatas.TryGetValue(item.Nickname, out User other))
                {
                    other.Session.Send(Packet.MakePacket(
                        SCProtocol.Despawn,
                        new Despawn()
                        {
                            Player = user.Player
                        }));
                }
            }
        }
        public void UpdateAroundPlayer(User user)
        {
            var cellIndex = _map.GetCellIndex(user.Player.CellPos);

            var indexList = _map.GetAroundCellByIndex(cellIndex);

            var prevAroundPlayers = new HashSet<Player>(user.AroundPlayers);

            var currentAroundPlayers = new List<Player>();

            var addAroundPlayer = new List<Player>();
            foreach (var index in indexList)
            {
                var players = _map.GetAllPlayersInCell(index);
                foreach(var player in players)
                {
                    if(user.Player == player)
                    {
                        continue;
                    }
                    if(prevAroundPlayers.Contains(player) == false)
                    {
                        addAroundPlayer.Add(player);
                    }
                    currentAroundPlayers.Add(player);
                }
            }

            var removed = new List<Player>();
            foreach (var player in prevAroundPlayers)
            {
                if(currentAroundPlayers.Contains(player) == false)
                {
                    removed.Add(player);
                }
            }

            foreach(var item in addAroundPlayer)
            {
                user.AroundPlayers.Add(item);
            }
            foreach (var item in removed)
            {
                user.AroundPlayers.Remove(item);
            }

            foreach (var item in addAroundPlayer)
            {
                if(item.Nickname.Equals(user.Player.Nickname) == true)
                {
                    continue;
                }

                if (_userDatas.TryGetValue(item.Nickname, out User addedUser))
                {
                    addedUser.Session.Send(Packet.MakePacket(
                        SCProtocol.Spawn,
                        new Spawn()
                        {
                            Player = user.Player
                        }));

                    user.Session.Send(Packet.MakePacket(
                        SCProtocol.Spawn,
                        new Spawn()
                        {
                            Player = item
                        }));
                }

            }

            foreach (var item in removed)
            {
                if (item.Nickname.Equals(user.Player.Nickname) == true)
                {
                    continue;
                }

                if (_userDatas.TryGetValue(item.Nickname, out User removedUser))
                {
                    removedUser.Session.Send(Packet.MakePacket(
                        SCProtocol.Despawn,
                        new Despawn()
                        {
                            Player = user.Player
                        }));

                    user.Session.Send(Packet.MakePacket(
                        SCProtocol.Despawn,
                        new Despawn()
                        {
                            Player = item
                        }));
                }
            }            
        }
        public bool Move(User user, Vector2Int targetPosition)
        {
            if(_map.CanGo(targetPosition) == false)
            {
                return false;
            }

            _map.RemovePlayerInCell(user.Player);

            user.Player.CellPos = targetPosition;
            _map.AddPlayerInCell(user.Player.CellPos, user.Player);

            return true;
        }
    }
}
