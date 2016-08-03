using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public class ColorCombine
    {
        public string First;
        public string Second;
        public string Third;

        public ColorCombine(string first, string second)
        {
            this.First = first;
            this.Second = second;
        }
    }

    public class ColorCombineList
    {
        public List<ColorCombine> cL = new List<ColorCombine>();
      
        public void Init()
        {
            ColorCombine cc = new ColorCombine("6599FF", "FF9900");
            cL.Add(cc);



            cc = new ColorCombine("CCFFCC", "66CCFF");
            cL.Add(cc);

            cc = new ColorCombine("CCCCCC", "996699");
            cL.Add(cc);

        }

    }
}
