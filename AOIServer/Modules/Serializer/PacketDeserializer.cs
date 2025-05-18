using AOIServer.Modules.Handler;
using Dignus.Collections;
using Dignus.Log;
using Dignus.Sockets;
using Dignus.Sockets.Processing;
using System.Text;

namespace AOIServer.Modules.Serializer
{
    internal class PacketDeserializer : SessionPacketProcessorBase
    {
        private const int HeaderSize = sizeof(int);
        private const int ProtocolSize = sizeof(ushort);

        private readonly CSProtocolHandler _protocolHandler;
        public PacketDeserializer(CSProtocolHandler csProtocolHandler)
        {
            _protocolHandler = csProtocolHandler;
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

            if (ProtocolHandlerMapper.ValidateProtocol<CSProtocolHandler>(protocol) == false)
            {
                LogHelper.Error($"[Server]protocol invalid - {protocol}");
                return;
            }
            var body = Encoding.UTF8.GetString(packet.Array, packet.Offset + ProtocolSize, packet.Count - ProtocolSize);
            ProtocolHandlerMapper<CSProtocolHandler, string>.DispatchProtocolAction(_protocolHandler, protocol, body);
        }
    }
}
