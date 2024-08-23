using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using SquareShooter.Network.Packet;
using SquareShooter.Screen;
using SquareShooter.Utils;

namespace SquareShooter.Network.Packet.Impl.Server
{
    public class ClientPacketAction : ClientPacket
    {


        public readonly static byte SpawnBullet = 0;

        public byte action=0;

        public ClientPacketAction() { 
          
        }

        public ClientPacketAction(byte b) {
            this.action = b;
        }



        public override byte getPacketID()
        {
            return 3;
        }
        
        public override void Process(NetworkClient client)
        {
            if(action == 0)
            {
                if (client.lastBullet.ElapsedMilliseconds < 750)
                {
                    return;
                }
                client.lastBullet.Restart();

               Vector2 velocity = MathUtils.Rotate(new System.Numerics.Vector2(0.3F, 0), client.player.rotation);

                SquareShooter.instance.gameManager.gameServer.SpawnBullet(client.player.username,client.player.position, velocity);
                
            }
         

        
        }

        public override void Read(ByteWrapper wrapper)
        {
            this.action = wrapper.ReadByte();
       
        }

        public override void Write(ByteWrapper wrapper)
        {
            wrapper.WriteByte(this.action);
       
        }

        
    }
}
