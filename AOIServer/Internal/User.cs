using Dignus.Sockets.Interfaces;
using ShareModel;

namespace AOIServer.Internal
{
    public class User
    {
        public Player Player { get; private set; }

        public ISession Session { get; private set; }

        public HashSet<Player> AroundPlayers { get; private set; }

        public User(Player player, ISession session)
        {
            Player = player;
            Session = session;
            AroundPlayers = new HashSet<Player>();
        }
    }
}
