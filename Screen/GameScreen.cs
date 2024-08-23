using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SquareShooter.Screen
{
    public abstract class GameScreen
    {

        public readonly SquareShooter squareShooter;
        public GameScreen(SquareShooter squareShooter)
        {
            this.squareShooter = squareShooter;
        }

        

        public virtual void Update()
        {
            if(squareShooter.gameManager.getMyPlayer() != null)
            {
                squareShooter.gameManager.getMyPlayer().startedGame = false;
            }
            
            squareShooter.camera.Offset = new Vector2(SquareShooter.WIDTH, SquareShooter.HEIGHT) * 0.5F;
            squareShooter.camera.Target = new Vector2(SquareShooter.WIDTH, SquareShooter.HEIGHT) * 0.5F;
            squareShooter.camera.Rotation = 0.0f;
            squareShooter.camera.Zoom = 1.0f;
        }
        public virtual void PostRender()
        {

        }
        public abstract void Render();
    }
}
