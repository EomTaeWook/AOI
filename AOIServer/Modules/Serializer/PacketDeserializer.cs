using AOIServer.Modules.Handler;
using Kosher.Log;
using Kosher.Sockets;
using Kosher.Sockets.Interface;
using System.Text;

namespace AOIServer.Modules.Serializer
{
    internal class PacketDeserializer : IPacketDeserializer
    {
        private const int ProtocolSize = sizeof(ushort);

        private CSProtocolHandler _aoiHandler;
        public PacketDeserializer(CSProtocolHandler aoiHandler)
        {
            _aoiHandler = aoiHandler;
        }
        public const int LegnthSize = sizeof(int);
        public bool IsTakedCompletePacket(NetworkBuffer buffer)
        {
            if (buffer.Count < LegnthSize)
            {
                return false;
            }
            var packetSizeBytes = buffer.Peek(LegnthSize);
            var length = BitConverter.ToInt32(packetSizeBytes);
            return buffer.Count >= length;
        }

        public void Deserialize(NetworkBuffer buffer)
        {
            var packetSizeBytes = buffer.Read(LegnthSize);
            var length = BitConverter.ToInt32(packetSizeBytes);
            var bytes = buffer.Read(length);
            var protocol = BitConverter.ToInt16(bytes);
            var body = Encoding.UTF8.GetString(bytes, ProtocolSize, bytes.Length - ProtocolSize);
            if (CSProtocolHandler.CheckProtocol(protocol) == false)
            {
                LogHelper.Error($"[Server]protocol invalid - {protocol}");
                buffer.Clear();
                return;
            }
            _aoiHandler.Process(protocol, body);
        }
    }
}
