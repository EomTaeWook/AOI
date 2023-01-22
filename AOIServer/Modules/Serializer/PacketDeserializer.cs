using AOIServer.Modules.Handler;
using Kosher.Collections;
using Kosher.Log;
using Kosher.Sockets.Interface;
using Protocol.SAndC;
using System.Text;

namespace AOIServer.Modules.Serializer
{
    internal class PacketDeserializer : IPacketDeserializer
    {
        private const int ProtocolSize = sizeof(ushort);

        private SAndCProtocolHandler _aoiHandler;
        public PacketDeserializer(SAndCProtocolHandler aoiHandler)
        {
            _aoiHandler = aoiHandler;
        }
        public const int LegnthSize = sizeof(int);
        public void Deserialize(Vector<byte> buffer)
        {
            var packetSizeBytes = buffer.Read(LegnthSize);
            var length = BitConverter.ToInt32(packetSizeBytes);

            var bytes = buffer.Read(length);

            var protocol = BitConverter.ToInt16(bytes);

            var body = Encoding.UTF8.GetString(bytes, ProtocolSize, bytes.Length - ProtocolSize);

            if(SAndCProtocolHandler.CheckProtocol(protocol) == false)
            {
                LogHelper.Error($"[Server]protocol invalid - {protocol}");
                buffer.Clear();
                return;
            }
            _aoiHandler.Process(protocol, body);
        }

        public bool IsTakedCompletePacket(Vector<byte> buffer)
        {
            if (buffer.Count < LegnthSize)
            {
                return false;
            }
            var packetSizeBytes = buffer.Peek(LegnthSize);
            var length = BitConverter.ToInt32(packetSizeBytes);
            return buffer.Count >= length;
        }
    }
}
