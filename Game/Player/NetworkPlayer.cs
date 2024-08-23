using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using Raylib_cs;

namespace SquareShooter.Game.Player
{
    //Host
    public class NetworkPlayer : GamePlayer
    {

        public static float NetworkSmoothing = 0.005f;
        public Vector2 drawPosition=Vector2.Zero;
        public float drawRotation = 0;

        public NetworkPlayer(String username) : base(username)
        {
            
        }

        public void MatchPosition()
        {
            Console.WriteLine("Matched position!");
            this.drawPosition = this.position;
        }

        public override void Update()
        {
            //I know I can do these with vectors(such as using normalized methods) but I wanted to debug
            float targetPosX = GetSafeFloat(this.position.X);
            float targetPosY = GetSafeFloat(this.position.Y);
            float currentPosX = GetSafeFloat(this.drawPosition.X);
            float currentPosY = GetSafeFloat(this.drawPosition.Y);

            float distance=(float)Math.Sqrt(Math.Pow(targetPosX - currentPosX,2) + Math.Pow(targetPosY - currentPosY,2));
            if(distance< NetworkSmoothing)
            {
                //MatchPosition();
            }
            currentPosX += NormalizeFloat((targetPosX - currentPosX)) * NetworkSmoothing *distance;
            currentPosY += NormalizeFloat((targetPosY - currentPosY)) * NetworkSmoothing * distance;

            this.drawPosition=new Vector2(currentPosX, currentPosY);


            this.drawRotation += (this.rotation - this.drawRotation) * NetworkSmoothing;
            //MatchPosition();
        }

        public float NormalizeFloat(float f)
        {
            return f == 0 ? 0 : f > 0 ? 1 : -1;
        }

        public float GetSafeFloat(float x)
        {
            if (float.IsNaN(x))
            {
                Console.WriteLine("INVALID FLOAT!");
                return 0;
            }

            return x;


        }



        



        
    }
}
