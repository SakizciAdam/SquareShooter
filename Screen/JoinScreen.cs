using Raylib_cs;
using SquareShooter.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SquareShooter.Screen
{
    public class JoinScreen : GameScreen
    {
        private Rectangle join=new Rectangle(0,0,0,0), ip = new Rectangle(0, 0, 0, 0);

        private string ipText="";

        private int joinState = 0;

        private string username;

        public JoinScreen(SquareShooter square,string username) : base(square)
        {
            this.username = username;
        }

        public override void Render()
        {
            RenderUtils.DrawCenteredString("IP", 256, 180, 32);
            this.ip = RenderUtils.DrawCenteredTextBox(ipText, 256,256, 24, 1,"IP");
            this.join=RenderUtils.DrawCenteredButton("Join", 300, 32, ipText.Length<3 ? -1 : joinState);
        }

        public override void Update()
        {
            base.Update();
            Vector2 mousePos = Raylib.GetMousePosition();
            joinState = 0;
            int key = Raylib.GetCharPressed();

            if (Raylib.IsKeyPressed(KeyboardKey.Backspace) && ipText.Length > 0)
            {
                ipText = ipText.Remove(ipText.Length - 1);
                return;
            }
            while (key > 0)
            {

                if ((key >= 32) && (key <= 125) && ipText.Length < 16)
                {
                    ipText += (char)key;


                }

                key = Raylib.GetCharPressed();
            }

            if (Raylib.CheckCollisionPointRec(mousePos, this.join))
            {
                joinState = 1;
                if(Raylib.IsMouseButtonDown(0)&&ipText.Length>=3)
                {
                    joinState = 2;
                    squareShooter.gameManager.tryToConnect(ipText,username);
                }
            }
  


        }
    }
}
