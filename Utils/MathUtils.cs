using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SquareShooter.Utils
{
    public static class MathUtils
    {

        public static Vector2 Rotate(Vector2 vec, double radians)
        {
            Vector2 newVec = new Vector2();
            newVec.X = vec.X * (float)Math.Cos(radians) - vec.Y * (float)Math.Sin(radians);
            newVec.Y = vec.X * (float)Math.Sin(radians) + vec.Y * (float)Math.Cos(radians);
            return newVec;
        }

        public static Vector2 RotateAroundOrigin(Vector2 vec,Vector2 origin, double radians)
        {

            double cosTheta = Math.Cos(radians);
            double sinTheta = Math.Sin(radians);
            return new Vector2
            {
                X =
                    (int)
                    (cosTheta * (vec.X - origin.X) -
                    sinTheta * (vec.Y - origin.Y) + origin.X),
                Y =
                    (int)
                    (sinTheta * (vec.X - origin.X) +
                    cosTheta * (vec.Y - origin.Y) + origin.Y)
            };
        }

        public static float[] RotateRectangle(Vector2 position,Vector2 size,float radians)
        {
            float[] result = new float[8];

            Vector2 origin = position + size / 2;

            result[0] = (float)Math.Cos(radians) * (position.X - origin.X) - (float)Math.Sin(radians) * (position.Y - origin.Y) + origin.X;
            result[1] = (float)Math.Sin(radians) * (position.X - origin.X) + (float)Math.Cos(radians) * (position.Y - origin.Y) + origin.Y;
            result[2] = (float)Math.Cos(radians) * (position.X+size.X - origin.X) - (float)Math.Sin(radians) * (position.Y+size.Y - origin.Y) + origin.X;
            result[3] = (float)Math.Sin(radians) * (position.X+size.X - origin.X) + (float)Math.Cos(radians) * (position.Y + size.Y - origin.Y) + origin.Y;
            result[4] = (float)Math.Cos(radians) * (position.X + size.X - origin.X) - (float)Math.Sin(radians) * (position.Y- origin.Y) + origin.X;
            result[5] = (float)Math.Sin(radians) * (position.X + size.X - origin.X) + (float)Math.Cos(radians) * (position.Y - origin.Y) + origin.Y;
            result[6] = (float)Math.Cos(radians) * (position.X- origin.X) - (float)Math.Sin(radians) * (position.Y+size.Y - origin.Y) + origin.X;
            result[7] = (float)Math.Sin(radians) * (position.X - origin.X) + (float)Math.Cos(radians) * (position.Y+size.Y - origin.Y) + origin.Y;
            return result;
        }

        public static float GetAngle(Vector2 to,Vector2 from)
        {
            

            return (float)Math.Atan2(to.Y - from.Y, to.X - from.X);
        }
    }
}
