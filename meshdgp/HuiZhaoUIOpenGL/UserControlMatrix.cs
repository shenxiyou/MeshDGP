using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao
{
    public partial class UserControlMatrix : UserControl
    {

        public Matrix4D matrix;

        public String Title = "None";

        public Matrix4D Matrix
        {
            get
            {
                return matrix;
            }

            set
            {
                matrix = value;
                ShowMatrix();
            }
        }

        private int marginOffsetX = 5;
        private int marginOffsetY = 30;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int boxWidth = this.Width;
            int boxHeight = this.Height - marginOffsetX;

            Graphics graphics = e.Graphics;


            //Draw two lines

            //Line 1 
            graphics.DrawLine(Pens.Black, new Point(marginOffsetX, marginOffsetY), new Point(marginOffsetX, boxHeight - marginOffsetX));
            graphics.DrawLine(Pens.Black, new Point(marginOffsetX, marginOffsetY), new Point(marginOffsetX + 5, marginOffsetY));
            graphics.DrawLine(Pens.Black, new Point(marginOffsetX, boxHeight - marginOffsetX), new Point(marginOffsetX + 5, boxHeight - marginOffsetX));


            int xOffset = boxWidth - 2 * marginOffsetX;
            //Line 2
            graphics.DrawLine(Pens.Black, new Point(marginOffsetX + xOffset, marginOffsetY), new Point(marginOffsetX + xOffset, boxHeight - marginOffsetX));
            graphics.DrawLine(Pens.Black, new Point(marginOffsetX + xOffset, marginOffsetY), new Point(marginOffsetX + xOffset - 5, marginOffsetY));
            graphics.DrawLine(Pens.Black, new Point(marginOffsetX + xOffset, boxHeight - marginOffsetX), new Point(marginOffsetX + xOffset - 5, boxHeight - marginOffsetX));


            int currentX = marginOffsetX;
            int currentY = marginOffsetY;

            int intervalX = boxWidth/4-3;
            int intervalY = (boxHeight-marginOffsetY)/4-3;

            Font titleFont = new Font(FontFamily.GenericSerif,10);

            graphics.DrawString(Title,titleFont, Brushes.Black, new PointF(marginOffsetX, marginOffsetX));

            Font font = new Font(FontFamily.GenericSansSerif, 9);

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    currentX = (marginOffsetX+5) + i * intervalX;
                    currentY = (marginOffsetY+5) + j * intervalY;

                    double value = Math.Round(matrix[i,j], 2);

                    string label = value.ToString();

                    graphics.DrawString(label,font,Brushes.Black, new PointF(currentX, currentY));

                }
            }
             
        }

        public UserControlMatrix()
        {
            InitializeComponent();
        }

        public void ShowMatrix()
        {
        }
    }
}
