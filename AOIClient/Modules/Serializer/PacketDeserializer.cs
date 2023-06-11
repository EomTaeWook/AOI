using AOIClient.Modules.Handler;
using Dignus.Collections;
using Dignus.Log;
using Dignus.Sockets.Extensions;
using Dignus.Sockets.Interface;
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
        public bool IsTakedCompletePacket(ArrayList<byte> buffer)
        {
            if (buffer.Count < LegnthSize)
            {
                return false;
            }
            var packetSizeBytes = BitConverter.ToInt32(buffer.Peek(LegnthSize));
            return (buffer.Count - LegnthSize) >= packetSizeBytes;
        }

        public void Deserialize(ArrayList<byte> buffer)
        {
            var packetSizeBytes = BitConverter.ToInt32(buffer.Read(LegnthSize));
            var bytes = buffer.Read(packetSizeBytes);
            var protocol = BitConverter.ToInt16(bytes);
            var body = Encoding.UTF8.GetString(bytes, ProtocolSize, bytes.Length - ProtocolSize);
            if (_handler.CheckProtocol(protocol) == false)
            {
                LogHelper.Error($"[Server]protocol invalid - {protocol}");
                return;
            }
            _handler.Process(protocol, body);
        }
    }
}
