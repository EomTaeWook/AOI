using AOIClient.Modules;
using AOIClient.Net;
using Kosher.Framework;
using Protocol.CAndS;
using Share;

namespace AOIClient.Internal
{
    internal class GameManager : Singleton<GameManager>
    {
        public Map Map { get; private set; }
        public Player UserPlayer;
        public List<Player> Players { get; private set; } = new List<Player>();
        public GameManager()
        {
            Map = new Map()
            {
                MaxX = 5,
                MaxY = 5
            };
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
