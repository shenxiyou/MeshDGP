using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace GraphicResearchHuiZhao
{
    internal class ColorRange
    {
        public int Lower { get; set; }
        public int Upper { get; set; }

        public ColorRange()
        { }
        public ColorRange(int lower, int upper)
        {
            Lower = lower;
            Upper = upper;
        }
        public int this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return Lower;
                    case 1: return Upper;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
            set
            {
                switch (index)
                {
                    case 0: Lower = value; break;
                    case 1: Upper = value; break;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }

        internal static ColorRange ToRange(int[] range)
        {
            if (range == null) return null;
            Debug.Assert(range.Length == 2);
            return new ColorRange(range[0], range[1]);
        }
    }
}
