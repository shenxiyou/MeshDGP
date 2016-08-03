using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace GraphicResearchHuiZhao
{
    public class TetGen : Process
    {
        public string FileName = "tettmp";
        ManualResetEvent mre;

        public TetGen()
        {
            this.StartInfo.FileName = "tetgen.exe";
            
            this.StartInfo.CreateNoWindow = true;
            this.StartInfo.UseShellExecute = false;
            this.StartInfo.RedirectStandardOutput = true;
            this.EnableRaisingEvents = true;
        }

        public int Run(TriMesh mesh, string param)
        {
            string file = this.FileName + ".off";
            TriMeshIO.WriteToOffFile(file, mesh);
            return this.Run(file, param);
        }

        public int Run(string file, string param)
        {
            this.StartInfo.Arguments = param + " " + file;
            this.Exited += TetGen_Exited;
            this.Start();
            this.BeginOutputReadLine();
            this.mre = new ManualResetEvent(false);
            this.mre.WaitOne();
            return ExitCode;
        }

        public void RunAsync(TriMesh mesh, string param)
        {
            //string file = IOHuiZhao.Instance.GetPath()+"/"+ this.FileName + ".off";

            string file =   this.FileName + ".off";
            TriMeshIO.WriteToOffFile(file, mesh);
            this.RunAsync(file, param);
        }

        public void RunAsync(string file, string param)
        {
            this.StartInfo.Arguments = param + " " + file;
            this.Start();
            this.BeginOutputReadLine();
        }

        void TetGen_Exited(object sender, EventArgs e)
        {
            this.mre.Set();
        }
    }
}
