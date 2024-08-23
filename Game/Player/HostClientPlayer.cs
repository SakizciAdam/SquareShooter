using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using Raylib_cs;
using SquareShooter.Network.Packet.Impl.Server;
using SquareShooter.Utils;

namespace SquareShooter.Game.Player
{
    //Host
    public class HostClientPlayer : ClientPlayer
    {
      

        public HostClientPlayer(String username) : base(username)
        {
      
        }

        public override void send()
        {
            foreach (var client in SquareShooter.instance.gameManager.gameServer.clients)
            {
                if(client == null ||!client.loggedIn ||!client.IsConnected()) {
                    continue;
                }

                client.SendPacket(new ServerPacketPlayerPosition(this.username,position,this.rotation));
                

            }
        }

        public override void SpawnBullet()
        {
            Vector2 velocity = MathUtils.Rotate(new System.Numerics.Vector2(0.3F, 0), this.rotation);
            SquareShooter.instance.gameManager.gameServer.SpawnBullet(this.username,position, velocity);
        }

        public override void Update()
        {
            
           base.Update();
        }



        



        
    }
}
