using Dignus.Framework;
using ShareModel;


namespace AOIClient.Internal
{
    internal class GameManager : Singleton<GameManager>
    {
        public Player UserPlayer;
        public Map Map { get; private set; }
        public const int CellSize = 20;
        public Dictionary<string, Player> Players { get; private set; } = new Dictionary<string, Player>();

        private HashSet<Player> _npc = new HashSet<Player>();
        public GameManager()
        {
            Map = new Map(0, 10, 0, 10);
        }
        public bool RemovePlayer(Player palyer)
        {
            if (UserPlayer == null)
            {
                return true;
            }
            return Players.Remove(palyer.Nickname);
        }
        public bool AddPlayer(Player player)
        {
            if (UserPlayer == null)
            {
                return true;
            }
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
        public void AddNpc(Player npc)
        {
            _npc.Add(npc);
        }

        public void SetUserPlayer(Player player)
        {
            EnterMyPlayer(player);
        }
        public void EnterMyPlayer(Player player)
        {
            UserPlayer = player;
        }

    }
}
