using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SquareShooter.Network.Packet;
using SquareShooter.Screen;

namespace SquareShooter.Network.Packet.Impl.Server
{
    public class ServerPacketPing : ServerPacket
    {

        public int id;

        public ServerPacketPing() { 
            this.id=0;   
        }

        public ServerPacketPing(int id) { 
            this.id=id;
        }

        public override byte getPacketID()
        {
            return 2;
        }

        public override void Process(GameClient client)
        {
            ClientPacketPing packet=new ClientPacketPing(this.id);
            
            SquareShooter.instance.gameManager.gameClient.SendPacket(packet);
        }

        public override void Read(ByteWrapper wrapper)
        {
            this.id = wrapper.ReadInt();

       
        }

        public override void Write(ByteWrapper wrapper)
        {
            wrapper.WriteInt(this.id);

        }
    }
}
