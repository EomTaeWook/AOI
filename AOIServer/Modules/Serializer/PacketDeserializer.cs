using AOIServer.Modules.Handler;
using Kosher.Log;
using Kosher.Sockets.Extensions;
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
            buffer.BaseStream.Seek(-LegnthSize, SeekOrigin.Current);
            return buffer.BaseStream.Length >= packetSizeBytes;
        }

        public void Deserialize(BinaryReader buffer)
        {
            var packetSizeBytes = buffer.ReadInt32();
            var bytes = buffer.ReadBytes(packetSizeBytes);
            var protocol = BitConverter.ToInt16(bytes);
            var body = Encoding.UTF8.GetString(bytes, ProtocolSize, bytes.Length - ProtocolSize);
            if (_aoiHandler.CheckProtocol(protocol) == false)
            {
                LogHelper.Error($"[Server]protocol invalid - {protocol}");
                buffer.BaseStream.Flush();
                return;
            }
            _aoiHandler.Process(protocol, body);
        }
    }
}
