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
    public class HostScreen : GameScreen
    {
        private Rectangle start=new Rectangle(0,0,0,0), exit = new Rectangle(0, 0, 0, 0);
        private int startState,exitState;
        private string username;
        public bool host = true;

        public HostScreen(SquareShooter square,string username) : base(square)
        {
            this.username= username;

            if (this.username == "")
            {
                this.host=false;
            }
        }

        public override void Render()
        {
            RenderUtils.DrawCenteredString("Players", 256, 64, 32);
            int k = 0;
            foreach(var player in squareShooter.gameManager.Players)
            {
                RenderUtils.DrawCenteredString(player.username, 256, 110+k*40, 32);
                k++;
            }
            if(host)
            {
                this.start = RenderUtils.DrawCenteredButton("Start", 380, 32, startState);
            }
            
            this.exit = RenderUtils.DrawCenteredButton("Back", 480, 32, exitState);
        }

        public override void Update()
        {
            base.Update();
            Vector2 mousePos = Raylib.GetMousePosition();
            startState = 0;
            exitState = 0;



            if (Raylib.CheckCollisionPointRec(mousePos, this.start)&&host)
            {
                startState = 1;
                if(Raylib.IsMouseButtonDown(0))
                {
                    startState = 2;
                    SquareShooter.instance.gameManager.StartGameHost();
                }
            }


            if (Raylib.CheckCollisionPointRec(mousePos, this.exit))
            {
                exitState = 1;
                if (Raylib.IsMouseButtonDown(0))
                {
                    if (host)
                    {
                        squareShooter.gameManager.gameServer.StopServer();
                        squareShooter.gameManager.gameServer = null;
                    } else
                    {
                        squareShooter.gameManager.gameClient.Stop();
                        squareShooter.gameManager.gameClient = null;
                    }
                    squareShooter.currentScreen = new LobbyScreen(squareShooter);

                    exitState = 2;
                }
            }


        }
    }
}
