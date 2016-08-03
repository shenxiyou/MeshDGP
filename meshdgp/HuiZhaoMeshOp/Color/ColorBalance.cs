using System;
using System.Collections.Generic;
using System.Text; 

namespace GraphicResearchHuiZhao
{
    public class ColorBalance
    {
        //Single instance
        private static ColorBalance singleton = new ColorBalance();

        public static ColorBalance Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new ColorBalance();
                return singleton;
            }
        }


        public Color4 minColor = Color4.Red;
        public Color4 MinColor
        {
            get
            {

                return minColor;
            }
            set
            {
                minColor = value;

            }
        }


        public Color4 maxColor = Color4.Green;

        public Color4 MaxColor
        {
            get
            { 
                return maxColor;
            }
            set
            {
                maxColor = value;

            }
        }

        public EnumColorBalance enumColor = EnumColorBalance.OneColor;

        public EnumColorBalance EnumColor
        {
            get
            {
                return enumColor;
            }

            set
            {
                enumColor = value;
            }
        }

        private double markValue = 0;
        private double epsion = 1;

        private double minClamp = double.MinValue ;
        private double maxClamp = double.MaxValue ;
        public double MinClamp
        {
            get
            {
                return minClamp;
            }
            set
            {
                minClamp = value;
            }
        }


        public double MaxClamp
        {
            get
            {
                return maxClamp;
            }
            set
            {
                maxClamp = value;
            }
        }

        public double[] ColorClamp(double[] values, double min, double max)
        {
            double[] result = new double[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] < min)
                {
                    result[i] = min;
                }
                else if (values[i] > max)
                {
                    result[i] = max;
                }
                else
                {
                    result[i] = values[i];
                }
            }

            return result;
        }

        public double[] ColorUnit(double[] values)
        {
            double min  = TriMeshFunction.Instance.ComputeMin(values);

            double max = TriMeshFunction.Instance.ComputeMax(values);

            double range=max-min ;

            double[] result = new double[values.Length];

            for (int i = 0; i < values.Length; i++)
            {
                result[i] = (values[i] - min) / range;
            }

            return result;

        }

        public void UpdateMinMax(double[] values)
        {
            if (minClamp == double.MinValue)
            {
                minClamp = TriMeshFunction.Instance.ComputeMin(values);
            }

            if (maxClamp == double.MaxValue)
            {
                maxClamp = TriMeshFunction.Instance.ComputeMax(values);
            }
        }
        

        public Color4[] ComputeColor(double[] values)
        {
            UpdateMinMax(values);

            Color4[] result = new Color4[values.Length];
            for (int i = 0; i < values.Length; i++)
            {

                switch (enumColor)
                {
                    case EnumColorBalance.OneColor:

                        result[i] = Color4.SetColorRamp(minClamp, maxClamp, values[i]);
                        break;

                    case EnumColorBalance.TwoColor:

                        result[i] = Color4.SetColorRampBetweenTwoColor(minClamp, maxClamp, values[i], minColor, maxColor);
                        break;

                    case EnumColorBalance.MarkColor: 
                        if (values[i] < markValue + epsion && values[i] > markValue - epsion)
                        {
                            result[i] = Color4.Black;
                        }
                        else
                        {
                            result[i] = Color4.SetColorRamp(minClamp, maxClamp, values[i]);
                        }
                        break;
                }

               
            }

            return result;

        }







    }
}
 
