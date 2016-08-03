using System;
using System.Collections.Generic;
using System.Text; 
 

namespace GraphicResearchHuiZhao
{
    public class GlobalFunction
    {
        public static void Output(string message)
        {

        }

        Random rnd = new Random();
        public const float scale = 3f;

        /// <summary>Returns a random Float in the range [-0.5*scale..+0.5*scale]</summary>
        public float GetRandom()
        {
            return (float)(rnd.NextDouble() - 0.5) * scale;
        }

        /// <summary>Returns a random Float in the range [0..1]</summary>
        public float GetRandom0to1()
        {
            return (float)rnd.NextDouble();
        }

        //public Bitmap GenerateNoise(int width, int height)
        //{
        //    Bitmap finalBmp = new Bitmap(width, height);
        //    Random r = new Random();

        //    for (int x = 0; x < width; x++)
        //    {
        //        for (int y = 0; y < height; y++)
        //        {
        //            int num = r.Next(0, 256);
        //            finalBmp.SetPixel(x, y, Color.FromArgb(255, num, num, num));
        //        }
        //    }

        //    return finalBmp;
        //}
    }
}
