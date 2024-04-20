using AOIServer.Net;
using Dignus.Collections;
using Dignus.Log;
using Dignus.Sockets.Interfaces;

namespace AOIServer.Modules.Serializer
{
    internal class PacketSerializer : IPacketSerializer
    {
        public ArrayQueue<byte> MakeSendBuffer(IPacket packet)
        {
            var sendPacket = packet as Packet;

            var packetSize = sendPacket.GetLength();

            var buffer = new ArrayQueue<byte>();

            buffer.AddRange(BitConverter.GetBytes(packetSize));

            buffer.AddRange(BitConverter.GetBytes(sendPacket.Protocol));

            buffer.AddRange(sendPacket.Body);
            LogHelper.Debug($"send : {sendPacket.Protocol}");
            return buffer;
        }

        ArraySegment<byte> IPacketSerializer.MakeSendBuffer(IPacket packet)
        {
            var sendPacket = packet as Packet;
            var packetSize = sendPacket.GetLength();
            var buffer = new ArrayQueue<byte>();

            buffer.AddRange(BitConverter.GetBytes(packetSize));
            buffer.AddRange(BitConverter.GetBytes(sendPacket.Protocol));
            buffer.AddRange(sendPacket.Body);
            LogHelper.Debug($"send : {sendPacket.Protocol}");
            return new ArraySegment<byte>([.. buffer]);
        }
    }
}
