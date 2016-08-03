using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class CRectangle
    {
        private double locationX = 0;
        private double locationY = 0;
        public double Width = 0;
        public double Height = 0;
        public CRectangle(double locationX, double locationY, double Width, double Height)
        {
            this.locationX = locationX;
            this.locationY = locationY;
            this.Width = Width;
            this.Height = Height;

        }

        public bool Contains(double x, double y)
        {
            if (x < (this.locationX + Width) &&
                this.locationX < x &&
                this.locationY < y &&
                y < (this.locationY + Height))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
