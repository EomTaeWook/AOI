using AOIClient.Net;
using Dignus.Collections;
using Dignus.Log;
using Dignus.Sockets.Interfaces;

namespace AOIClient.Modules.Serializer
{
    internal class PacketSerializer : IPacketSerializer
    {
        public ArraySegment<byte> MakeSendBuffer(IPacket packet)
        {
            var sendPacket = packet as Packet;

            var packetSize = sendPacket.GetLength();

            var buffer = new ArrayQueue<byte>();

            buffer.AddRange(BitConverter.GetBytes(packetSize));

            buffer.AddRange(BitConverter.GetBytes((ushort)sendPacket.Protocol));

            buffer.AddRange(sendPacket.Body);
            LogHelper.Debug($"send : {sendPacket.Protocol}");
            return buffer.ToArray();
        }
    }
}
