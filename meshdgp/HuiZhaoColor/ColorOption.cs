using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class ColorOptions
    {
        public ColorScheme ColorScheme { get; set; }
        public Luminosity Luminosity { get; set; }

        public ColorOptions()
        { }
        public ColorOptions(ColorScheme scheme, Luminosity luminosity)
        {
            ColorScheme = scheme;
            Luminosity = luminosity;
        }
    }
}
