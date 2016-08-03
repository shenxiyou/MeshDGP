using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace GraphicResearchHuiZhao
{
    public class Metro
    {
        public event EventHandler Completed;

        int index;
        DiffInfo[] info = new DiffInfo[2];
        double hausdorff;
        List<string> rawInfo = new List<string>();

        public DiffInfo Forward
        {
            get { return this.info[0]; }
        }

        public DiffInfo Backward
        {
            get { return this.info[1]; }
        }

        public double Hausdorff
        {
            get { return this.hausdorff; }
        }

        public string[] Raw
        {
            get { return this.rawInfo.ToArray(); }
        }

        public Metro()
        {

        }

        public void Run(string file1, string file2)
        {
            this.rawInfo.Clear();
            this.info = new DiffInfo[2];

            Process process = new Process();
            process.StartInfo.FileName = "metro.exe";
            process.StartInfo.Arguments = string.Format("{0} {1}", file1, file2);
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.EnableRaisingEvents = true;
            process.OutputDataReceived += process_OutputDataReceived;
            process.Exited += process_Exited;
            process.Start();
            process.BeginOutputReadLine();
        }

        void process_Exited(object sender, EventArgs e)
        {
            this.OnCompleted();
        }

        void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            try
            {
                if (e.Data != null && !e.Data.StartsWith("Sampling"))
                {
                    this.rawInfo.Add(e.Data);
                    if (e.Data.StartsWith("Forward"))
                    {
                        this.index = 0;
                    }
                    else if (e.Data.StartsWith("Backward"))
                    {
                        this.index = 1;
                    }
                    else if (e.Data.StartsWith("  max"))
                    {
                        string num = e.Data.Substring(9, 8);
                        this.info[index].Max = double.Parse(num);
                    }
                    else if (e.Data.StartsWith("  mean"))
                    {
                        string num = e.Data.Substring(9, 8);
                        this.info[index].Mean = double.Parse(num);
                    }
                    else if (e.Data.StartsWith("  RMS"))
                    {
                        string num = e.Data.Substring(9, 8);
                        this.info[index].RMS = double.Parse(num);
                    }
                    else if (e.Data.StartsWith("Hausdorff"))
                    {
                        string num = e.Data.Substring(20, 8);
                        this.hausdorff = double.Parse(num);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        protected void OnCompleted()
        {
            if (this.Completed != null)
            {
                this.Completed(this, EventArgs.Empty);
            }
        }
    }

    public struct DiffInfo
    {
        public double Max;
        public double Mean;
        public double RMS;
    }
}
