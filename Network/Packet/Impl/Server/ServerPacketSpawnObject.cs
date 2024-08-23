using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using SquareShooter.Network.Packet;
using SquareShooter.Screen;

namespace SquareShooter.Network.Packet.Impl.Server
{
    public class ServerPacketSpawnObject : ServerPacket
    {
        public string owner;
        public float x, y;
        public float velX, velY;

        public ServerPacketSpawnObject() { 
            
        }

        public ServerPacketSpawnObject(string owner,Vector2 pos,Vector2 vel) { 
            this.owner = owner;
            this.x=pos.X; this.y=pos.Y; this.velX=vel.X; this.velY=vel.Y;
        }

        public override byte getPacketID()
        {
            return 6;
        }

        public override void Process(GameClient client)
        {
            if (this.owner == "WLL")
            {
                SquareShooter.instance.gameManager.SpawnLocalWall(new Raylib_cs.Rectangle((int)x, (int)y, (int)velX, (int)velY));
                return;
            }
            SquareShooter.instance.gameManager.SpawnLocalBullet(owner,new Vector2(x,y), new Vector2(velX, velY));


        }

        public override void Read(ByteWrapper wrapper)
        {
            this.owner = wrapper.ReadString();
            this.x = wrapper.ReadFloat();
            this.y = wrapper.ReadFloat();
            this.velX = wrapper.ReadFloat();
            this.velY = wrapper.ReadFloat();
        }

        public override void Write(ByteWrapper wrapper)
        {
            wrapper.WriteString(this.owner);    
            wrapper.WriteFloat(this.x);
            wrapper.WriteFloat(this.y);
            wrapper.WriteFloat(this.velX);
            wrapper.WriteFloat(this.velY);
        }
    }
}
