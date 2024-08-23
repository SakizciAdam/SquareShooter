using SquareShooter.Network;
using SquareShooter.Network.Packet.Impl.Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SquareShooter.Game.Player
{
    public abstract class GamePlayer
    {
        public static readonly int MaxHealth = 100;
     

        public readonly String username;
        public int health;
        public Vector2 position=new Vector2(0,0);
        public float rotation = 0f; //radians

        public Stopwatch hitFrame = Stopwatch.StartNew();

        public Rectangle hitbox=> new System.Drawing.Rectangle((int)position.X, (int)position.Y,16,16);

        public GamePlayer(String username)
        {
            this.username = username;
            this.health = MaxHealth;
        }

        public abstract void Update();

        public virtual void OnHitByBullet()
        {
            if (hitFrame.ElapsedMilliseconds > 15)
            {
                this.health -= 10;
                if (SquareShooter.instance.gameManager.isHost)
                {
                    foreach(NetworkClient client in SquareShooter.instance.gameManager.gameServer.clients)
                    {
                        if(client == null || !client.loggedIn)
                        {
                            continue;
                        }
                        if(client.player is HostClientPlayer)
                        {
                            continue;
                        }

                        client.SendPacket(new ServerPacketHealth(username, health));
                    }
                }
                hitFrame.Restart(); 
            }
           
        }


        public Rectangle GetRectangle()
        {
            return new Rectangle((int)position.X, (int)position.Y, 16, 16);
        }



        
    }
}
