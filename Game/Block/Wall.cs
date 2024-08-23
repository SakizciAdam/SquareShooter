using Raylib_cs;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SquareShooter.Game.Block
{
    public class Wall
    {

        public Rectangle rectangle;

        public Vector2 Origin=>rectangle.Position+rectangle.Size*0.5f;
        public System.Drawing.Rectangle SystemRectangle => new System.Drawing.Rectangle((int)rectangle.X,(int) rectangle.Y, (int)rectangle.Width, (int)rectangle.Height);
        public Wall(Rectangle rec)
        {
            this.rectangle= rec;
        }

   

    }
}
