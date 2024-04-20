using AOIServer.Modules.Handler;
using Dignus.Collections;
using Dignus.Log;
using Dignus.Sockets;
using Dignus.Sockets.Interfaces;
using System.Text;

namespace AOIServer.Modules.Serializer
{
    internal class PacketDeserializer : IPacketDeserializer
    {
        private const int ProtocolSize = sizeof(ushort);

        private readonly CSProtocolHandler _protocolHandler;
        public PacketDeserializer(CSProtocolHandler csProtocolHandler)
        {
            _protocolHandler = csProtocolHandler;
        }
        public const int LegnthSize = sizeof(int);

        public bool IsCompletePacketInBuffer(ArrayQueue<byte> buffer)
        {
            if (buffer.LongCount < LegnthSize)
            {
                return false;
            }
            var packetSizeBytes = BitConverter.ToInt32(buffer.Peek(LegnthSize));
            return (buffer.LongCount - LegnthSize) >= packetSizeBytes;
        }

        public void Deserialize(ArrayQueue<byte> buffer)
        {
            var packetSizeBytes = BitConverter.ToInt32(buffer.Read(LegnthSize));
            var bytes = buffer.Read(packetSizeBytes);
            var protocol = BitConverter.ToInt16(bytes);
            var body = Encoding.UTF8.GetString(bytes, ProtocolSize, bytes.Length - ProtocolSize);
            if (ProtocolHandlerMapper.ValidateProtocol<CSProtocolHandler>(protocol) == false)
            {
                LogHelper.Error($"[Server]protocol invalid - {protocol}");
                return;
            }
            ProtocolHandlerMapper<CSProtocolHandler, string>.DispatchProtocolAction(_protocolHandler, protocol, body);
        }
    }
}
