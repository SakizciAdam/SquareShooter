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
    public class ConnectingScreen : GameScreen
    {


        

        public ConnectingScreen(SquareShooter square) : base(square)
        {
         
        }

        public override void Render()
        {
            if(!squareShooter.gameManager.connected)
            {
                RenderUtils.DrawCenteredString("Connecting...", 256, 180, 32);
                return;
            }
            RenderUtils.DrawCenteredString("Players", 256, 64, 32);
            int k = 0;
            foreach (var player in squareShooter.gameManager.Players)
            {
                RenderUtils.DrawCenteredString(player.username, 256, 110 + k * 40, 32);
                k++;
            }


        }

        public override void Update()
        {
            base.Update();

        }
    }
}
