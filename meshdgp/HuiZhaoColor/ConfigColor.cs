using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class ConfigColor
    {
        private static ConfigColor singleton = new ConfigColor();

        public static ConfigColor Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new ConfigColor();
                return singleton;
            }
        }

        private ColorScheme _scheme = ColorScheme.Random;
        private Luminosity _luminosity = Luminosity.Bright;
        private int _numberToGenerate = 84;


   
        public ColorScheme Scheme
        {
            get
            {
                return _scheme;
            }
            set
            {
                _scheme = value;
            }
        }

        public Luminosity Luminosity
        {
            get
            {
                return _luminosity;
            }
            set
            {
                _luminosity = value;
            }
        }

        public int NumberToGenerate
        {
            get
            {
                return _numberToGenerate;
            }
            set
            {
                _numberToGenerate = value;
            }
        }

    }
}
