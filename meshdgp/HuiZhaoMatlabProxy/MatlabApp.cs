using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public partial class MatlabProxy
    {
        public double[,] TestARAP(int[] knownIndex, double[,] knownValue3V)
        {

            CD(AppPath);
         //   OutPut(mesh);
            OutPut(knownIndex, "b");
            OutPut(knownValue3V, "bc");
            string sss=matlab.Execute(@"U = arap(V,F,b,bc,'Energy','spokes')");
            Console.WriteLine(sss);
            //double[,] pos = GetMatrix("U");


            double[,] pos = GetMatrix("V");

            Console.WriteLine(pos.GetLength(0));
            Console.WriteLine(pos.GetLength(1));
            return pos;
            
        }

       

    }
}
