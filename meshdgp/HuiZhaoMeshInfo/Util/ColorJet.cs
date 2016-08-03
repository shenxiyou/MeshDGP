using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public class ColorJet
    {

        public static Color4 Jet(double x_in)
        {
            // Only important if the number of colors is small. In which case the rest is
            // still wrong anyway
            // x = linspace(0,1,jj)' * (1-1/jj) + 1/jj;
            //
            const double rone = 0.8;
            const double gone = 1.0;
            const double bone = 1.0;
            double r;
            double g;
            double b;
            double x = x_in;
            x = (x_in < 0 ? 0 : (x > 1 ? 1 : x));

            if (x < 1f / 8f)
            {
                r = 0;
                g = 0;
                b = bone * (0.5 + (x) / (1f / 8f) * 0.5);
            }
            else if (x < 3f / 8f)
            {
                r = 0;
                g = gone * (x - 1f / 8f) / (3f / 8f - 1f / 8f);
                b = bone;
            }
            else if (x < 5f / 8f)
            {
                r = rone * (x - 3f / 8f) / (5f / 8f - 3f / 8f);
                g = gone;
                b = (bone - (x - 3f / 8f) / (5f / 8f - 3f / 8f));
            }
            else if (x < 7f / 8f)
            {
                r = rone;
                g = (gone - (x - 5f / 8f) / (7f / 8f - 5f / 8f));
                b = 0;
            }
            else
            {
                r = (rone - (x - 7f / 8f) / (1f - 7f / 8f) * 0.5);
                g = 0;
                b = 0;
            }
            return new Color4(r, g, b);
        }

        public static Color4[] Jet(double[] arr, double min, double max)
        {
            Color4[] color = new Color4[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                color[i] = Jet((arr[i] - min) / (max - min));
            }
            return color;
        }

        public static Color4[] Jet(double[] arr, bool normalize)
        {
            if (normalize)
            {
                double min = double.MaxValue;
                double max = double.MinValue;
                for (int i = 0; i < arr.Length; i++)
                {
                    if (arr[i] < min)
                    {
                        min = arr[i];
                    }
                    if (arr[i] > max)
                    {
                        max = arr[i];
                    }
                }
                return Jet(arr, min, max);
            }
            else
            {
                return Jet(arr, 0, 1);
            }
        }
    }
}
