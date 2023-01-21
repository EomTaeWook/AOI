using Kosher.Sockets.Interface;
using System.Text.Json;
using System.Text;

namespace AOIServer.Net
{
    public class Packet : IPacket
    {
        public ushort Protocol { get; private set; }
        public byte[] Body { get; private set; }
        public Packet(ushort protocol, string body)
        {
            Protocol = protocol;
            Body = Encoding.UTF8.GetBytes(body);
        }
        public Packet(ushort protocol, byte[] body)
        {
            Protocol = protocol;
            Body = body;
        }
        public int GetLength()
        {
            return sizeof(ushort) + Body.Length;
        }
        public static Packet MakePacket<T>(ushort protocol, T packetData)
        {
            var packet = new Packet(protocol, JsonSerializer.Serialize(packetData));

            return packet;
        }
    }
}
