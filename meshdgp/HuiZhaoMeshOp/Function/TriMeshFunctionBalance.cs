using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public partial class TriMeshFunction
    {


        public double[] Clamp(double[] function, double min, double max)
        {
            double[] result = new double[function.Length];
            for (int i = 0; i < function.Length; i++)
            {
                if (function[i] < min)
                {
                    result[i] = min;
                }
                else if (function[i] > max)
                {
                    result[i] = max;
                }
                else
                {
                    result[i] = function[i];
                }
            }

            return result;
        }



        public double ComputeMax(double[] function)
        {
            double result = double.MinValue;
            for (int i = 0; i < function.Length; i++)
            {
                if (function[i] > result)
                {
                    result = function[i];
                }
            }
            return result;
        }

        public double ComputeMin(double[] function)
        {
            double result = double.MaxValue;
            for (int i = 0; i < function.Length; i++)
            {
                if (function[i] < result)
                {
                    result = function[i];
                }
            }
            return result;
        }

        public double[]  Unit(double[] function,bool nodal)
        {
            double min = ComputeMin(function);

            double max = ComputeMax(function);

            double range = max - min;

            double[] result = new double[function.Length];

            for (int i = 0; i < function.Length; i++)
            {
                result[i] = (function[i] - min) / range;
                if (nodal)
                {
                    if (function[i] < 0.001 && function[i] > -0.001)
                    {
                        result[i] = 0.0001;
                    }
                }
            }

            return result; 
        }

    }
}
