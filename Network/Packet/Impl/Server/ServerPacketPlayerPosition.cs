using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using SquareShooter.Game.Player;
using SquareShooter.Network.Packet;
using SquareShooter.Screen;

namespace SquareShooter.Network.Packet.Impl.Server
{
    public class ServerPacketPlayerPosition : ServerPacket
    {

        public string username;
        public int x, y;
        public float rotation;
        public ServerPacketPlayerPosition() {
            this.username = "";
            this.x=this.y=0;
        }

        public ServerPacketPlayerPosition(string username,int x,int y,float rot) {
            this.username=username;
            this.x = x;
            this.y = y;
            this.rotation = rot;
        }

        public ServerPacketPlayerPosition(string username,Vector2 pos,float rot)
        {
            this.username = username;
            this.x = (int)pos.X;
            this.y = (int)pos.Y;
            this.rotation = rot;
        }

        public override byte getPacketID()
        {
            return 5;
        }

        public override void Process(GameClient client)
        {
            var pl = SquareShooter.instance.gameManager.GetPlayer(this.username);
            pl.position = new Vector2((float)x, (float)y);
            pl.rotation = this.rotation;
        
        }

        public override void Read(ByteWrapper wrapper)
        {
            this.username=wrapper.ReadString();
            this.x=wrapper.ReadInt();
            this.y = wrapper.ReadInt();
            this.rotation=wrapper.ReadFloat();
        }

        public override void Write(ByteWrapper wrapper)
        {
            wrapper.WriteString(this.username);
            wrapper.WriteInt(this.x);
            wrapper.WriteInt(this.y);
            wrapper.WriteFloat(this.rotation);

        }
    }
}
