using Dignus.Sockets;
using Share;

namespace AOIServer.Internal
{
    public class User
    {
        public Player Player { get; private set; }

        public Session Session { get; private set; }

        public HashSet<Player> AroundPlayers { get; private set; }

        public User(Player player, Session session)
        {
            Player = player;
            Session = session;
            AroundPlayers = new HashSet<Player>();
        }
    }
}
