using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SquareShooter.Utils
{
    public static class RenderUtils
    {
        private static Stopwatch stopwatch = new Stopwatch();

        public static void init()
        {
            stopwatch.Start();
        }

        public static void DrawCenteredString(String text,int x,int y,int textSize=16)
        {
            int width = Raylib.MeasureText(text, textSize);



            Raylib.DrawText(text, x - width/2,y, textSize, Color.Black);
        }

        public static Rectangle DrawCenteredTextBox(String text, int y, int width,int textSize, int state = 0,String defaultText="")
        {
            
            
            
            return DrawTextBox(text, 256 - width / 2, y - (textSize + 2) / 2,width, textSize, state);
        }

        public static Rectangle DrawTextBox(String text, int x, int y,int width, int textSize, int state = 0, String defaultText = "")
        {
            if (text.Length == 0)
            {
                text = defaultText;
            }
            if (state == 1 && stopwatch.ElapsedMilliseconds % 1000 > 0 && stopwatch.ElapsedMilliseconds % 1000 < 500)
            {
                text += "_";
            }
   
            int xOffset = 3;

            int height = textSize + 2;
           
           
            Raylib.DrawRectangle(x, y, width + xOffset * 2, height, Color.Black);




            Raylib.BeginScissorMode(x, y, width + xOffset * 2, height);

            int textWidth = Raylib.MeasureText(text, textSize);
            int offset = 0;
            if(textWidth > width)
            {
                offset= width - textWidth;
            }
          
            Raylib.DrawText(text, x + offset + xOffset + 1, y + 2, textSize, Color.White);

            Raylib.EndScissorMode();

            

            return new Rectangle(x, y, width + xOffset * 2, height);
        }

        public static Rectangle DrawCenteredButton(String text, int y, int textSize,int state=0)
        {
            int width = Raylib.MeasureText(text, textSize);
            return DrawButton(text, 256 - width / 2, y - (textSize + 2) / 2, textSize,state);
        }

        public static Rectangle DrawButton(String text, int x, int y, int textSize,int state=0 )
        {
            int width = Raylib.MeasureText(text, textSize);
            int xOffset = 3;
         
            int height = textSize + 2;
            Color dark=state== 0 ? Color.Black : state==1 ? Color.DarkGray : state==-1 ? Color.LightGray : Color.Gray;
          
            Raylib.DrawRectangle(x, y, width+ xOffset*2, height, dark);
            Raylib.DrawText(text, x+ xOffset, y+1, textSize,Color.White);
            return new Rectangle ( x, y, width + xOffset * 2, height);
        }
    }
}
