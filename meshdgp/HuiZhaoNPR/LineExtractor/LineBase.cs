using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Drawing;

namespace GraphicResearchHuiZhao
{
    public abstract class LineBase
    {
        public EnumLine Type  ;

        public virtual string Name
        {
            get { return this.GetType().Name; }
        }

        private Color color;
        public virtual Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public LineBase()
        {
            color = Color.Black;
        }

       
    }

   
}
