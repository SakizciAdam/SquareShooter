using SquareShooter.Network.Packet.Impl.Client;
using SquareShooter.Network.Packet.Impl.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareShooter.Network.Packet
{
    public static class PacketManager
    {
        public static Dictionary<Byte,Type> clientPacketMap= new Dictionary<Byte,Type>();
        public static Dictionary<Byte, Type> serverPacketMap = new Dictionary<Byte, Type>();
        public static void init()
        {
            clientPacketMap.Add(0, typeof(ClientPacketHandshake));
            clientPacketMap.Add(1, typeof(ClientPacketMove));
            clientPacketMap.Add(2, typeof(ClientPacketPing));
            clientPacketMap.Add(3, typeof(ClientPacketAction));


            serverPacketMap.Add(0, typeof(ServerPacketJoin));
            serverPacketMap.Add(1, typeof(ServerPacketDisconnect));
            serverPacketMap.Add(2, typeof(ServerPacketPing));
            serverPacketMap.Add(3, typeof(ServerPacketPlayerChange));
            serverPacketMap.Add(4, typeof(ServerPacketStart));
            serverPacketMap.Add(5, typeof(ServerPacketPlayerPosition));
            serverPacketMap.Add(6, typeof(ServerPacketSpawnObject));
            serverPacketMap.Add(7, typeof(ServerPacketHealth));
        }

        public static ServerPacket? GetServerPacket(byte id)
        {
            Type type = serverPacketMap[id];

            return (ServerPacket?)Activator.CreateInstance(type);
        }

        public static ClientPacket? GetClientPacket(byte id)
        {
            Type type= clientPacketMap[id];

            return (ClientPacket?)Activator.CreateInstance(type);
        }


    }
}
