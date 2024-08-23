using Raylib_cs;
using SquareShooter.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SquareShooter.Screen
{
    public class KickedScreen : GameScreen
    {
        private Rectangle exit = new Rectangle(0, 0, 0, 0);
        private int exitState;
        private string message;

        public KickedScreen(SquareShooter square,string message) : base(square)
        {
            this.message = message;
        }

        public override void Render()
        {
            RenderUtils.DrawCenteredString(message, 256, 64, 32);
            int k = 0;

            this.exit = RenderUtils.DrawCenteredButton("Back", 480, 32, exitState);
        }

        public override void Update()
        {
            base.Update();
            Vector2 mousePos = Raylib.GetMousePosition();
  
            exitState = 0;




            if (Raylib.CheckCollisionPointRec(mousePos, this.exit))
            {
                exitState = 1;
                if (Raylib.IsMouseButtonDown(0))
                {

                    squareShooter.currentScreen = new LobbyScreen(squareShooter);

                    exitState = 2;
                }
            }


        }
    }
}
