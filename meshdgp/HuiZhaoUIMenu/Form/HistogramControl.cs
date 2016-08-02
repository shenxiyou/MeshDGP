using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms; 

namespace GraphicResearchHuiZhao
{
    public partial class HistogramControl : UserControl
    {
        public HistogramData data;

        public int coordHeight = 20;
        public int valueChartHeight = 120;
        public int bottomBorder = 60;
        public int leftBorder = 50;
        public int rightBorder = 20;
        public int coordYInterval = 20;

        public int ValidWidth
        {
            get { return this.Width - this.leftBorder - this.rightBorder; }
        }

        public int HistChartHeight
        {
            get { return this.Height - this.bottomBorder - this.valueChartHeight - this.coordHeight; }
        }

        public int LeftIndex
        {
            get
            {
                double value = (double)(this.leftBtn - this.leftBorder) / this.ValidWidth;
                double pos = (this.data.values.Count - 1) * value;
                return (int)Math.Ceiling(pos);
            }
        }

        public int RightIndex
        {
            get
            {
                double value = (double)(this.rightBtn - this.leftBorder) / this.ValidWidth;
                double pos = (this.data.values.Count - 1) * value;
                return (int)pos;
            }
        }

        int leftBtn;
        int rightBtn;
        bool leftDown;
        bool rightDown;

        int hlBtn;
        int hrBtn;
        bool hlDown;
        bool hrDown;

        public HistogramControl()
        {
            InitializeComponent();
            this.leftBtn = this.leftBorder;
            this.rightBtn = this.Width - this.rightBorder;
            this.hlBtn = this.leftBorder;
            this.hrBtn = this.Width - this.rightBorder;
        }

        public void UpdateHist(double[] values)
        {
            this.data = new HistogramData(values);
            this.Change();
        }

        public double[] ClampValue()
        {
            int w = this.ValidWidth / this.data.hist.Length;
            if (w < 2) w = 2;
            int left = (this.hlBtn - this.leftBorder) / w;
            int right = (this.hrBtn - this.leftBorder) / w;
            return this.data.Clamp(left, right);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (this.data != null)
            {
                this.DrawHistogramCoord(e.Graphics);
                this.DrawHistogram(e.Graphics);
                this.DrawValuesCoord(e.Graphics);
                this.DrawValues(e.Graphics);
                this.DrawBottom(e.Graphics);
                this.DrawHistBtn(e.Graphics);
            }
        }

        void DrawValuesCoord(Graphics g)
        {
            int bottom = this.Height - this.bottomBorder;
            double min = this.data.values[0];
            double max = this.data.values[this.data.values.Count - 1];
            int zero = bottom;
            if (min < 0)
            {
                zero += (int)(min / (max - min) * this.valueChartHeight);
                g.DrawLine(Pens.Red, this.leftBorder, zero, this.Width - this.rightBorder, zero);
            }

            double inv = (double)this.valueChartHeight / (max - min);
            int step = (int)(this.coordYInterval / inv);
            if (step < 1) step = 1;
            for (int i = (int)min; i < (int)max; i += step)
            {
                int y = (int)(zero - inv * i);
                g.DrawLine(Pens.Black, this.leftBorder - 10, y, this.leftBorder - 1, y);
                string str = string.Format("{0,5}", i);
                g.DrawString(str, this.Font, Brushes.Red, 1, y - this.Font.SizeInPoints / 2);
            }

            float w = (float)this.ValidWidth / this.data.values.Count;
            step = (int)(50 / w);
            for (int i = 0; i <= this.data.values.Count; i += step)
            {
                float x = i * w + this.leftBorder;
                float y = this.Height - this.bottomBorder;
                g.DrawLine(Pens.Red, x, y, x, y + 3);
                string str = i.ToString();
                x-= str.Length * this.Font.Size / 3;
                g.DrawString(str, this.Font, Brushes.Black, x, y + 5);
            }
        }

        void DrawValues(Graphics g)
        {
            int bottom = this.Height - this.bottomBorder;
            double width = (double)this.ValidWidth / (this.data.values.Count - 1);
            double min = this.data.values[0];
            double max = this.data.values[this.data.values.Count - 1];
            int zero = bottom;
            if (min < 0)
            {
                zero += (int)(min / (max - min) * this.valueChartHeight);
                g.DrawLine(Pens.Red, this.leftBorder, zero, this.Width - this.rightBorder, zero);
            }

            for (int i = 0; i < this.data.values.Count; i++)
            {
                Color4 c4 = TriMeshUtil.GradientColor((float)((this.data.values[i] - min) / (max - min)));
                Color c = Color.FromArgb((int)(c4.R), (int)(c4.G), (int)(c4.B));
                g.DrawLine(new Pen(c),
                    (int)(i * width) + this.leftBorder,
                    zero - (int)(this.data.values[i] / (max - min) * this.valueChartHeight),
                    (int)(i * width) + this.leftBorder,
                    zero);
            }
        }

        void DrawHistogramCoord(Graphics g)
        {
            double inv = (double)this.HistChartHeight / this.data.maxCount;
            int step = (int)(this.coordYInterval / inv);
            if (step < 1) step = 1;
            for (int i = 0; i < this.data.maxCount; i += step)
            {
                int y = (int)(this.HistChartHeight - inv * i);
                g.DrawLine(Pens.Black, this.leftBorder - 10, y, this.leftBorder - 1, y);
                string str = string.Format("{0,5}", i);
                g.DrawString(str, this.Font, Brushes.Red, 1, y - this.Font.SizeInPoints / 2);
            }

            int w = this.ValidWidth / this.data.hist.Length;
            if (w < 2) w = 2;
            for (int i = 0; i <= this.data.hist.Length; i++)
            {
                int x = i * w + this.leftBorder;
                g.DrawLine(Pens.Red, x, this.HistChartHeight, x, this.HistChartHeight + 3);
            }
            step = (int)(50 / w);
            for (int i = 0; i <= this.data.hist.Length; i+=step)
            {
                string str = i.ToString();
                float x = i * w + this.leftBorder - str.Length * this.Font.Size / 3;
                g.DrawString(str, this.Font, Brushes.Black, x, this.HistChartHeight + 5);
            };
        }

        void DrawHistogram(Graphics g)
        {
            int w = this.ValidWidth / this.data.hist.Length;
            if (w < 2) w = 2;
            for (int i = 0; i < this.data.hist.Length; i++)
            {
                double p = (double)this.data.hist[i] / this.data.maxCount;
                int l = (int)(p * this.HistChartHeight);
                Color4 c4 = TriMeshUtil.GradientColor((float)i / this.data.hist.Length);
                Color c = Color.FromArgb((int)(c4.R), (int)(c4.G), (int)(c4.B));
                g.FillRectangle(new SolidBrush(c), i * w + this.leftBorder, this.HistChartHeight - l, w - 1, l);
            }
        }

        void DrawHistBtn(Graphics g)
        {
            this.DrawHistBtn(g, new Point(this.hlBtn, this.HistChartHeight));
            this.DrawHistBtn(g, new Point(this.hrBtn, this.HistChartHeight));
        }

        void DrawHistBtn(Graphics g, Point top)
        {
            Point left = new Point(top.X - 5, top.Y + 8);
            Point right = new Point(top.X + 5, top.Y + 8);
            g.DrawPolygon(Pens.Black, new[] { top, left, right });
            g.DrawLine(Pens.Red, top.X, 0, top.X, top.Y);
        }

        void DrawBottom(Graphics g)
        {
            g.DrawLine(Pens.Red, this.leftBorder, this.HistChartHeight, this.Width - this.rightBorder, this.HistChartHeight);

            int mid = this.Height - this.bottomBorder - this.valueChartHeight;
            g.DrawLine(Pens.Black, this.leftBorder, mid, this.Width - this.rightBorder, mid);

            int bottom = this.Height - this.bottomBorder;
            g.DrawLine(Pens.Black, this.leftBorder, bottom, this.Width - this.rightBorder, bottom);
            g.DrawLine(Pens.Red, this.leftBorder - 1, 0, this.leftBorder - 1, bottom);

            this.DrawBtn(g, new Point(this.leftBtn, bottom), true);
            this.DrawBtn(g, new Point(this.rightBtn, bottom), false);        }

        void DrawBtn(Graphics g, Point top, bool isLeft)
        {
            Point left = new Point(top.X - 5, top.Y + 8);
            Point right = new Point(top.X + 5, top.Y + 8);
            g.DrawPolygon(Pens.Black, new[] { top, left, right });
            g.DrawLine(Pens.Red, top.X, this.Height - this.bottomBorder - this.valueChartHeight, top.X, top.Y);

            double value = (double)(top.X - this.leftBorder) / this.ValidWidth;
            double pos = (this.data.values.Count - 1) * value;
            int i = isLeft ? (int)Math.Ceiling(pos) : (int)pos;
            string str = string.Format("{0}:{1:F3}", i, this.data.values[i]);
            g.DrawString(str, this.Font, Brushes.Red, top.X - str.Length * this.Font.Size / 3, top.Y + 15);
        }

        private void HistogramControl_MouseDown(object sender, MouseEventArgs e)
        {
            float r = 10f;
            int y = this.Height - this.bottomBorder;
            if (GetDistance(e.Location, new Point(this.leftBtn, y)) < r)
            {
                this.leftDown = true;
            }
            else if (GetDistance(e.Location, new Point(this.rightBtn, y)) < r)
            {
                this.rightDown = true;
            }
            else if (GetDistance(e.Location, new Point(this.hlBtn, this.HistChartHeight)) < r)
            {
                this.hlDown = true;
            }
            else if (GetDistance(e.Location, new Point(this.hrBtn, this.HistChartHeight)) < r)
            {
                this.hrDown = true;
            }
        }

        private void HistogramControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.leftDown || this.rightDown || this.hlDown || this.hrDown)
            {
                this.DrawBottom(this.CreateGraphics());
            }
        }

        private void HistogramControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.leftDown)
            {
                this.leftBtn = e.X;
                this.Change();
            }
            else if (this.rightDown)
            {
                this.rightBtn = e.X;
                this.Change();
            }
            else if (this.hlDown)
            {
                int w = this.ValidWidth / this.data.hist.Length;
                if (w < 2) w = 2;
                this.hlBtn = (int)((double)(e.X - this.leftBorder) / w + 0.5d) * w + this.leftBorder;
                this.hlBtn = Clamp(this.hlBtn, this.leftBorder, this.hrBtn);
                this.hlDown = false;
                this.Refresh();
                this.ClampValue();
            }
            else if (this.hrDown)
            {
                int w = this.ValidWidth / this.data.hist.Length;
                if (w < 2) w = 2;
                this.hrBtn = (int)((double)(e.X - this.leftBorder) / w + 0.5d) * w + this.leftBorder;
                this.hrBtn = Clamp(this.hrBtn, this.leftBorder, this.hrBtn);
                this.hrDown = false;
                this.Refresh();
            }
        }

        private void HistogramControl_Resize(object sender, EventArgs e)
        {
            this.numericUpDown1.Maximum = this.ValidWidth;
            if (this.data != null && this.ValidWidth > 0)
            {
                this.Change();
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            this.data.Build((int)this.numericUpDown1.Value);
            this.Change();
        }

        void Change()
        {
            this.leftBtn = Clamp(this.leftBtn, this.leftBorder, this.rightBtn);
            this.rightBtn = Clamp(this.rightBtn, this.leftBtn, this.Width - this.rightBorder);
            double min = (double)(this.leftBtn - this.leftBorder) / this.ValidWidth;
            double max = (double)(this.rightBtn - this.leftBorder) / this.ValidWidth;
            this.data.Build((int)this.numericUpDown1.Value, min, max);

            this.leftDown = false;
            this.rightDown = false;
            this.Refresh();
        }

        static float GetDistance(Point p1, Point p2)
        {
            return (float)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        static int Clamp(int x, int min, int max)
        {
            if (x < min)
            {
                return min;
            }
            if (x > max)
            {
                return max;
            }
            return x;
        }
    }
}
