using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public class CriteriaRange
    {
        private double max = 0;
        private double min = 0;
        private EnumColorItem item = EnumColorItem.Default;

        public EnumColorItem Item
        {
            get
            {
                return item;
            }
            set
            {
                item = value;
            }
        }

        public double Max
        {
            get
            {
                return max;
            }
            set
            {
                max = value;
            }
        }
        public double Min
        {
            get
            {
                return min;
            }
            set
            {
                min = value;
            }
        }
    }
}
