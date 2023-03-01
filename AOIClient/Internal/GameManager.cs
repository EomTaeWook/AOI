using AOIClient.Modules;
using AOIClient.Net;
using Kosher.Framework;
using Protocol.CAndS;
using Share;
using System.Collections.Generic;

namespace AOIClient.Internal
{
    internal class GameManager : Singleton<GameManager>
    {
        public Map Map { get; private set; }
        public Player UserPlayer;
        public Dictionary<string, Player> Players { get; private set; } = new Dictionary<string, Player>();
        public GameManager()
        {
            Map = new Map()
            {
                MaxX = 5,
                MaxY = 5
            };
        }
        public bool RemovePlayer(Player palyer)
        {
            return Players.Remove(palyer.Nickname);
        }
        public bool AddPlayer(Player player)
        {
            return Players.TryAdd(player.Nickname, player);
        }
        public Player this[string key]
        {
            get 
            {
                Players.TryGetValue(key, out Player player);
                return player;
            }
        }
        
        public void SetUserPlayer(Player player)
        {
            EnterMyPlayer(player);
        }
        public void EnterMyPlayer(Player player)
        {
            UserPlayer = player;
        }
        //public void Enter(Player player)
        //{
        //    Players..Add(player);
        //}
        //public void Leave(Player player)
        //{
        //    Players.Remove(player);
        //}
    }
}
