using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SquareShooter.Game.Player;
using SquareShooter.Network.Packet;
using SquareShooter.Screen;

namespace SquareShooter.Network.Packet.Impl.Server
{
    public class ServerPacketHealth : ServerPacket
    {

        public string username;
        public int health;

        public ServerPacketHealth() {
            this.username = "";
            this.health = 0;
        }

        public ServerPacketHealth(string username,int health) { 
            this.username=username;
            this.health = health;
        }

        public override byte getPacketID()
        {
            return 7;
        }

        public override void Process(GameClient client)
        {
            GamePlayer player = SquareShooter.instance.gameManager.GetPlayer(this.username);
            if(player.health>health)
            {
                //For animation
                player.hitFrame.Restart();
            }
            player.health=health;
        }

        public override void Read(ByteWrapper wrapper)
        {
            this.username = wrapper.ReadString();
            this.health=wrapper.ReadByte();

       
        }

        public override void Write(ByteWrapper wrapper)
        {
            wrapper.WriteString(this.username);
            wrapper.WriteByte((byte)this.health);

        }
    }
}
