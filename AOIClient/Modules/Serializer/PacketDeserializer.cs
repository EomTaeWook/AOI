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

            if (SCProtocolHandler.CheckProtocol(protocol) == false)
            {
                LogHelper.Error($"[Server]protocol invalid - {protocol}");
                buffer.BaseStream.Flush();
                return;
            }
            _handler.Process(protocol, body);
        }
    }
}
