using AOIClient.Modules.Handler;
using Kosher.Log;
using Kosher.Sockets;
using Kosher.Sockets.Interface;
using System.Text;

namespace AOIClient.Modules.Serializer
{
    internal class PacketDeserializer : IPacketDeserializer
    {
        private const int ProtocolSize = sizeof(ushort);
        public const int LegnthSize = sizeof(int);
        readonly SCProtocolHandler _handler;
        public PacketDeserializer(SCProtocolHandler handler)
        {
            _handler = handler;
        }
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

            if (SCProtocolHandler.CheckProtocol(protocol) == false)
            {
                LogHelper.Error($"[Server]protocol invalid - {protocol}");
                buffer.Clear();
                return;
            }
            _handler.Process(protocol, body);
        }
    }
}
