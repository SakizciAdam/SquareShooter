using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using Raylib_cs;
using SquareShooter.Network.Packet.Impl.Server;
using SquareShooter.Utils;

namespace SquareShooter.Game.Player
{
    public class ClientPlayer : GamePlayer
    {

        public Stopwatch stopwatch = new Stopwatch();
        public Stopwatch bulletStopwatch = Stopwatch.StartNew();
        public Stopwatch lastMovementPacketSend = new Stopwatch();

        public bool startedGame = false;

  
        

        public ClientPlayer(String username) : base(username)
        {
            stopwatch.Start();
            lastMovementPacketSend.Start();
        }

        public virtual void send()
        {
            ClientPacketMove packet = new ClientPacketMove(this.position.X,this.position.Y,this.rotation);

            SquareShooter.instance.gameManager.gameClient.SendPacket(packet);
      
            //networking 
        }

        public virtual void SpawnBullet()
        {
            ClientPacketAction action = new ClientPacketAction(ClientPacketAction.SpawnBullet);
            SquareShooter.instance.gameManager.gameClient.SendPacket(action);
        }

        public override void Update()
        {
            if(!startedGame)
            {
                SquareShooter.instance.camera.Zoom = 1f;

                SquareShooter.instance.camera.Offset = new Vector2(SquareShooter.WIDTH, SquareShooter.HEIGHT) * 0.5F;
                SquareShooter.instance.camera.Target = new Vector2(SquareShooter.WIDTH, SquareShooter.HEIGHT) * 0.5F;
                return;
            }
   

            SquareShooter.instance.camera.Offset = new Vector2(SquareShooter.WIDTH, SquareShooter.HEIGHT) * 0.5F;
            SquareShooter.instance.camera.Target = this.position;//* 0.5F;
            SquareShooter.instance.camera.Zoom = 1.15f;

            float speed = 2f;
            float oldrot = this.rotation;
            Vector2 mousePos = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), SquareShooter.instance.camera);
            rotation = MathUtils.GetAngle(mousePos, this.position);
            Vector2 oldPosition = new Vector2(position.X, position.Y);
            bool moved = false;
            if (rotation!=oldrot) {
                moved = true;
            }

            if (bulletStopwatch.ElapsedMilliseconds > 750)
            {
                SpawnBullet();
                bulletStopwatch.Restart();
            }



            if (stopwatch.ElapsedMilliseconds > 25)
            {
               
               
                if (Raylib.IsKeyDown(KeyboardKey.W))
                {
                    if (!PositionIntersectsWithWall(position-Vector2.UnitY*speed))
                    {
                        position.Y -= speed;



                        moved = true;
                    }
                   
                }
                else if (Raylib.IsKeyDown(KeyboardKey.S))
                {
                    if (!PositionIntersectsWithWall(position + Vector2.UnitY * speed))
                    {
                        position.Y += speed;
                        moved = true;
                    }
                    
                }

                if (Raylib.IsKeyDown(KeyboardKey.A))
                {
                    if (!PositionIntersectsWithWall(position - Vector2.UnitX * speed))
                    {
                        position.X -= speed;
                        moved = true;
                    }
                   
                }
                else if (Raylib.IsKeyDown(KeyboardKey.D))
                {
                    if (!PositionIntersectsWithWall(position + Vector2.UnitX * speed))
                    {
                        position.X += speed;
                        moved = true;
                    }
                   
                }
                position.X = Math.Clamp(position.X, 0, 512);
                position.Y = Math.Clamp(position.Y, 0, 512);

             
                stopwatch.Restart();
            }
            if (lastMovementPacketSend.ElapsedMilliseconds > 100)
            {
                send();
                lastMovementPacketSend.Restart();
            }
       
     
        }

        public bool PositionIntersectsWithWall(Vector2 pos)
        {
            var origin = pos + Vector2.One * 8;
            var OFFSET = 2;
            foreach (var wall in SquareShooter.instance.gameManager.walls)
            {
                var radius = 16/2+ OFFSET + wall.rectangle.Width/1.75;
                var deltaX = origin.X - wall.Origin.X;
                var deltaY = origin.Y - wall.Origin.Y;
                if(deltaX * deltaX + deltaY * deltaY <= radius * radius) { return true; }
            }
            return false;
        }

        public bool IntersectsWithWall()
        {
      
            return PositionIntersectsWithWall(this.position);
        }



        



        
    }
}
