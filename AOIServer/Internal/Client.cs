using Kosher.Sockets;
using Share;

namespace AOIServer.Internal
{
    internal class User
    {
        public Player Player { get; private set; }

        public Session Session { get; private set; }

        public HashSet<User> AroundPlayers { get; private set; }

        public User(Player player, Session session)
        {
            Player = player;
            Session = session;
            AroundPlayers = new HashSet<User>();
        }
    }
}
