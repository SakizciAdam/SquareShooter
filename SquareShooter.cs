using Raylib_cs;
using SquareShooter.Game;
using SquareShooter.Network.Packet;
using SquareShooter.Screen;
using SquareShooter.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SquareShooter
{
    public class SquareShooter
    {
        public const int WIDTH = 512;
        public const int HEIGHT = 512;

        public readonly GameManager gameManager;
        public GameScreen currentScreen;
        public Camera2D camera;

        public static SquareShooter instance;

        public SquareShooter() {
            instance = this;
            gameManager = new GameManager(this);
            PacketManager.init();
            Raylib.InitWindow(WIDTH, HEIGHT, "Square Shooter");
            RenderUtils.init();
            camera = new Camera2D();
            camera.Offset = new Vector2(WIDTH,HEIGHT)*0.5F;
            camera.Target = new Vector2(WIDTH, HEIGHT) * 0.5F;
            camera.Rotation = 0.0f;
            camera.Zoom = 1.0f;
            currentScreen =new  LobbyScreen(this);
            while (!Raylib.WindowShouldClose())
            {
                Update();
                Raylib.BeginMode2D(camera);
                
                Raylib.ClearBackground(Color.RayWhite);

                Render();

                Raylib.EndMode2D();

                currentScreen.PostRender();

                Raylib.EndDrawing();
            }

            if (gameManager.gameServer != null)
            {
                gameManager.gameServer.StopServer();
            }

            Raylib.CloseWindow();
            System.Environment.Exit(0);
        }

       


        public void Render()
        {
            currentScreen.Render();
            
        }

        public void Update()
        {
            currentScreen.Update();
            gameManager.Update();
        }
    }
}
