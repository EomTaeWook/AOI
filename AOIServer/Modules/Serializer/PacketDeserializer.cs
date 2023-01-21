using Kosher.Collections;
using Kosher.Sockets.Interface;
using Protocol.SAndC;
using System.Text;

namespace AOIServer.Modules.Serializer
{
    internal class PacketDeserializer : IPacketDeserializer
    {
        private const int ProtocolSize = sizeof(ushort);

        private AOIHandler _aoiHandler;
        public PacketDeserializer(AOIHandler aoiHandler)
        {
            _aoiHandler = aoiHandler;
        }
        public const int LegnthSize = sizeof(int);
        public void Deserialize(Vector<byte> buffer)
        {
            var packetSizeBytes = buffer.Read(LegnthSize);
            var length = BitConverter.ToInt32(packetSizeBytes);

            var bytes = buffer.Read(length);

            SAndCProtocol protocol = (SAndCProtocol)BitConverter.ToInt16(bytes);

            var body = Encoding.UTF8.GetString(bytes, ProtocolSize, bytes.Length - ProtocolSize);

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
