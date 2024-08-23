using Raylib_cs;
using SquareShooter.Game.Player;
using SquareShooter.Game.Projectile;
using SquareShooter.Network;
using SquareShooter.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SquareShooter.Screen
{
    public class PlayScreen : GameScreen
    {
        
        public Stopwatch start=Stopwatch.StartNew();

        public PlayScreen(SquareShooter square) : base(square)
        {
          
        }

        public override void Render()
        {

            foreach (var player in squareShooter.gameManager.Players)
            {
                Vector2 pos=player.position;
                float rotation=player.rotation;

                if(player is NetworkPlayer)
                {
                    rotation = ((NetworkPlayer)player).drawRotation;
                    pos =((NetworkPlayer)player).drawPosition;   
                }

                Color color=player.hitFrame.ElapsedMilliseconds<100 ? Color.Red : Color.Black;
                Raylib.DrawRectanglePro(new Rectangle(pos.X, pos.Y, 16, 16),new Vector2(8,8), rotation * Raylib.RAD2DEG , color);

                Vector2 endPos = pos + MathUtils.Rotate(Vector2.UnitX, rotation) * 25;
                Raylib.DrawLineEx(pos, endPos, 4f, color);

                int width = (int)((float)player.health / 100f * 32f);
                Raylib.DrawRectanglePro(new Rectangle(pos.X-16, pos.Y + 20, 32f, 8), new Vector2(0,0), 0, Color.Gray);
                Raylib.DrawRectanglePro(new Rectangle(pos.X-16, pos.Y+20, width, 8), new Vector2(0,0), 0, Color.Red);
                
               
            }
            for(int i=0;i< squareShooter.gameManager.bullets.Count; i++)
            {
                var bullet = squareShooter.gameManager.bullets[i];
                Raylib.DrawRectanglePro(new Rectangle(bullet.position.X, bullet.position.Y, 8, 8), new Vector2(4, 4), MathUtils.GetAngle(Vector2.Zero, bullet.velocity) * Raylib.RAD2DEG, Color.Black);
            }

            foreach (var wall in squareShooter.gameManager.walls)
            {
                Raylib.DrawRectanglePro(new Rectangle(wall.rectangle.X, wall.rectangle.Y, wall.rectangle.Width, wall.rectangle.Height), new Vector2(4, 4), 0, Color.DarkGray);


            }

            
        }

        public override void PostRender()
        {
            if (start.Elapsed.TotalSeconds < 3)
            {
                RenderUtils.DrawCenteredString((3 - start.Elapsed.Seconds) + "!", 256, 256 - 48, 48);
            }
            if (start.Elapsed.TotalSeconds >= 3 && start.Elapsed.TotalSeconds <= 4)
            {
                RenderUtils.DrawCenteredString("GO!!!", 256, 256 - 48, 48);
            }
            //RenderUtils.DrawCenteredString(ClientPlayer.NetworkSmoothing+"", 256, 12, 12);
        }

        public override void Update()
        {
            squareShooter.gameManager.getMyPlayer().startedGame= start.Elapsed.TotalSeconds>=3;

        }
    }
}
