using AOIClient.Modules.Handler;
using Dignus.Collections;
using Dignus.Log;
using Dignus.Sockets;
using Dignus.Sockets.Processing;
using System.Text;

namespace AOIClient.Modules.Serializer
{
    internal class PacketDeserializer : SessionPacketProcessorBase
    {
        private const int ProtocolSize = sizeof(ushort);
        private const int HeaderSize = sizeof(int);
        readonly SCProtocolHandler _handler;
        public PacketDeserializer(SCProtocolHandler handler)
        {
            _handler = handler;
        }

        public override bool TakeReceivedPacket(ArrayQueue<byte> buffer, out ArraySegment<byte> packet, out int consumedBytes)
        {
            packet = null;
            consumedBytes = 0;
            if (buffer.Count < HeaderSize)
            {
                return false;
            }

            var bodySize = BitConverter.ToInt32(buffer.Peek(HeaderSize));
            if (buffer.Count < HeaderSize + bodySize)
            {
                return false;
            }

            buffer.TryReadBytes(out _, HeaderSize);

            consumedBytes = bodySize;

            return buffer.TrySlice(out packet, bodySize);
        }

        public override void ProcessPacket(in ArraySegment<byte> packet)
        {
            var protocol = BitConverter.ToInt16(packet);

            if (ProtocolHandlerMapper.ValidateProtocol<SCProtocolHandler>(protocol) == false)
            {
                LogHelper.Error($"[Server]protocol invalid - {protocol}");
                return;
            }
            var body = Encoding.UTF8.GetString(packet.Array, packet.Offset + ProtocolSize, packet.Count - ProtocolSize);
            ProtocolHandlerMapper<SCProtocolHandler, string>.DispatchProtocolAction(_handler, protocol, body);
        }
    }
}
