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
        public bool IsTakedCompletePacket(BinaryReader buffer)
        {
            if (buffer.BaseStream.Length < LegnthSize)
            {
                return false;
            }
            var packetSizeBytes = buffer.ReadInt32();
            buffer.BaseStream.Seek(0, SeekOrigin.Begin);
            return buffer.BaseStream.Length >= packetSizeBytes;
        }

        public void Deserialize(BinaryReader buffer)
        {
            var packetSizeBytes = buffer.ReadInt32();
            var bytes = buffer.ReadBytes(packetSizeBytes);
            var protocol = BitConverter.ToInt16(bytes);
            var body = Encoding.UTF8.GetString(bytes, ProtocolSize, bytes.Length - ProtocolSize);
            if (CSProtocolHandler.CheckProtocol(protocol) == false)
            {
                LogHelper.Error($"[Server]protocol invalid - {protocol}");
                buffer.BaseStream.Flush();
                return;
            }
            _aoiHandler.Process(protocol, body);
        }
    }
}
