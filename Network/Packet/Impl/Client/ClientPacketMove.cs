using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SquareShooter.Network.Packet;
using SquareShooter.Screen;

namespace SquareShooter.Network.Packet.Impl.Server
{
    public class ClientPacketMove : ClientPacket
    {

        public float posX,posY,rot;

        public ClientPacketMove() { 
            this.posX = this.posY = this.rot=0;   
        }

        public ClientPacketMove(float posX, float posY,float rot) { 
            this.posX= posX;
            this.posY= posY;
            this.rot= rot;
        }

        public override byte getPacketID()
        {
            return 1;
        }
        
        public override void Process(NetworkClient client)
        {
            //Anticheat
            /*
            bool funkyMove = false;
            if (Math.Abs(this.deltaX) > 2)
            {
                funkyMove= true;
                this.deltaX = Math.Clamp(this.deltaX, -2, 2);
            }
            if (Math.Abs(this.deltaY) > 2)
            {
                funkyMove = true;
                this.deltaY = Math.Clamp(this.deltaY, -2, 2);
            }*/

          
  
            
            client.player.position = new System.Numerics.Vector2(this.posX, this.posY);
            Console.WriteLine("Player position " + client.player.position.X + " " + client.player.position.Y);
            client.player.rotation= this.rot;
          
            

            foreach(NetworkClient cl in SquareShooter.instance.gameManager.gameServer.clients)
            {
                if (cl == null || !cl.loggedIn)
                {
                    continue;
                }
                if(cl == client)
                {
                    continue;
                }
                cl.SendPacket(new ServerPacketPlayerPosition(cl.player.username, client.player.position, this.rot));
            }
        }

        public override void Read(ByteWrapper wrapper)
        {
            this.posX=wrapper.ReadFloat();
            this.posY=wrapper.ReadFloat();
            this.rot=wrapper.ReadFloat();

       
        }

        public override void Write(ByteWrapper wrapper)
        {
            wrapper.WriteFloat(this.posX);
            wrapper.WriteFloat(this.posY);
            wrapper.WriteFloat(this.rot);
        }
    }
}
