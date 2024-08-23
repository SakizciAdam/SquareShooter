using SquareShooter.Game.Player;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SquareShooter.Game.Projectile
{
    public class Bullet
    {
        public Vector2 velocity, position;
        public string owner;
        public Bullet(string owner,Vector2 position,Vector2 velocity) {
            this.owner = owner;
            this.position= position;
            this.velocity= velocity;
        
        }

        public GamePlayer Owner => SquareShooter.instance.gameManager.GetPlayer(this.owner);

        public void Remove()
        {
            SquareShooter.instance.gameManager.bullets.Remove(this);
        }

        public void Update()
        {
            position += velocity;
            if (position.X > 512 || position.X < 0 || position.Y > 512 || position.Y < 0)
            {
                Remove();
            }
            if (SquareShooter.instance.gameManager.isHost)
            {
                for (int i = 0; i < SquareShooter.instance.gameManager.Players.Count; i++)
                {
                    if (Intersects(SquareShooter.instance.gameManager.Players[i].GetRectangle())&& SquareShooter.instance.gameManager.Players[i].username!=owner)
                    {
                 
                        SquareShooter.instance.gameManager.Players[i].OnHitByBullet();
                    }
                }
                

            }

            for (int i = 0; i < SquareShooter.instance.gameManager.walls.Count; i++)
            {
                var rec = SquareShooter.instance.gameManager.walls[i].rectangle;

                if (Intersects(new Rectangle((int)rec.X, (int)rec.Y, (int)rec.Width, (int)rec.Height)))
                {

                    Remove();
                }
            }

        }

        public bool Intersects(Rectangle rec)
        {
            return new Rectangle((int)position.X, (int)position.Y, 8, 8).IntersectsWith(rec);
        }

    }
}
