using Raylib_cs;
using SquareShooter.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SquareShooter.Screen
{
    public class UsernameScreen : GameScreen
    {

        private Rectangle join = new Rectangle(0, 0, 0, 0), ip = new Rectangle(0, 0, 0, 0);

        private string usernameText = "";

        private int buttonState = 0;

        private bool host,local;

        public UsernameScreen(SquareShooter squareShooter,bool host,bool local=false) : base(squareShooter)
        {
            this.host = host;
            this.local = local;
        }

       


        public override void Render()
        {
            RenderUtils.DrawCenteredString("Username", 256, 180, 32);
            this.ip = RenderUtils.DrawCenteredTextBox(usernameText, 250, 256, 24, 1, "Username");
            this.join = RenderUtils.DrawCenteredButton("Next", 300, 32, usernameText.Length< 3 ? -1 : buttonState);
        }

        public override void Update()
        {
            base.Update();
            Vector2 mousePos = Raylib.GetMousePosition();
            buttonState = 0;

            int key = Raylib.GetCharPressed();

            if (Raylib.IsKeyPressed(KeyboardKey.Backspace) && usernameText.Length > 0)
            {
                usernameText = usernameText.Remove(usernameText.Length - 1);
                return;
            }
            while (key > 0)
            {

                if ((key >= 32) && (key <= 125) && usernameText.Length < 16)
                {
                    usernameText += (char)key;


                }

                key = Raylib.GetCharPressed();
            }
            if (Raylib.CheckCollisionPointRec(mousePos, this.join)&&usernameText.Length>0)
            {
                buttonState = 1;
                if (Raylib.IsMouseButtonDown(0))
                {
                   
                    buttonState = 2;
                    if (host)
                    {
                        this.squareShooter.gameManager.host(usernameText);
                        return;
                    }
                    if (local)
                    {
                        squareShooter.gameManager.tryToConnect(GetLocalIPAddress(), this.usernameText);
                        return;
                    }
                    this.squareShooter.currentScreen = new JoinScreen(squareShooter,usernameText);
                }
            }



        }

        public string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}
