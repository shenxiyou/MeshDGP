using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class HistogramData
    {
        public List<double> values;
        public int[] hist;
        public int maxCount;

        public HistogramData(double[] values)
        {
            this.values = new List<double>(values);
            this.values.Sort();
        }

        public void Build(int column)
        {
            this.Build(column, 0d, 1d);
        }

        public void Build(int column, double minPercent, double maxPercent)
        {
            int begin = (int)Math.Ceiling((this.values.Count - 1) * minPercent);
            int end = (int)((this.values.Count - 1) * maxPercent) + 1;
            if (begin != end)
            {
                this.hist = new int[column];
                double step = (this.values[end - 1] - this.values[begin]) / (column - 1);
                if (step < double.Epsilon) step = 1;
                this.maxCount = 0;
                for (int i = begin; i < end; i++)
                {
                    int index = (int)((this.values[i] - this.values[begin]) / step);
                    this.hist[index]++;
                    if (this.hist[index] > this.maxCount)
                    {
                        this.maxCount = this.hist[index];
                    }
                }
            }
        }

        public double[] Clamp(int histBegin, int histEnd)
        {
            double[] arr = new double[this.values.Count];
            int leftSum = 0;
            for (int i = 0; i < histBegin; i++)
            {
                leftSum += this.hist[i];
            }
            int rightSum = 0;
            for (int i = histEnd; i < this.hist.Length; i++)
            {
                rightSum += this.hist[i];
            }
            for (int i = 0; i < this.values.Count; i++)
            {
                if (i < leftSum)
                {
                    arr[i] = this.values[leftSum];
                }
                else if (i > this.values.Count - rightSum)
                {
                    arr[i] = this.values[this.values.Count - rightSum];
                }
                else
                {
                    arr[i] = this.values[i];
                }
            }
            return arr;
        }
    }
}
