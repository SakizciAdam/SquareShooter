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
    public class LobbyScreen : GameScreen
    {
        private Rectangle join=new Rectangle(0,0,0,0), host = new Rectangle(0, 0, 0, 0), local = new Rectangle(0, 0, 0, 0);

        private int joinState=0,hostState=0,localState=0;

        public LobbyScreen(SquareShooter square) : base(square)
        {

        }

        public override void Render()
        {
           
            RenderUtils.DrawCenteredButton("Square Shooter", 120, 48);
            this.join=RenderUtils.DrawCenteredButton("Join A Game", 256, 32, joinState);
            this.local = RenderUtils.DrawCenteredButton("Join Local Game", 300, 32, localState);
            this.host=RenderUtils.DrawCenteredButton("Host A Game", 344, 32, hostState);
        }

        public override void Update()
        {
            base.Update();
            Vector2 mousePos = Raylib.GetMousePosition();
            localState=joinState = hostState = 0;
            
            if (Raylib.CheckCollisionPointRec(mousePos, this.join))
            {
                joinState = 1;
                if(Raylib.IsMouseButtonDown(0))
                {
                    joinState = 2;
                    squareShooter.currentScreen = new UsernameScreen(squareShooter,false);
                }
            }
            if (Raylib.CheckCollisionPointRec(mousePos, this.host))
            {
                hostState = 1;
                if (Raylib.IsMouseButtonDown(0))
                {
                    hostState = 2;
                    
                    squareShooter.currentScreen = new UsernameScreen(squareShooter,true);
                }
            }
            if (Raylib.CheckCollisionPointRec(mousePos, this.local))
            {
                localState = 1;
                if (Raylib.IsMouseButtonDown(0))
                {
                    localState = 2;

                    squareShooter.currentScreen = new UsernameScreen(squareShooter, false,true);
                }
            }


        }
    }
}
