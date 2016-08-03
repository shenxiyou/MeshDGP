using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao
{
    public  class AniController
    {

        private static AniController singleton = new AniController();

        public static AniController Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new AniController();
                return singleton;
            }
        }

        private TriMesh Mesh
        {
            get
            {
                return GlobalData.Instance.TriMesh;
            }
            set
            {
                GlobalData.Instance.TriMesh = value;
            }
        }


        protected virtual void OnChanged(EventArgs e)
        {
            GlobalData.Instance.OnChanged(e);
        }




        private Timer timer = new Timer();

        private void timer_Tick(object sender, EventArgs e)
        {
            Run();
            OnChanged(EventArgs.Empty);
        }

        public void SetTimer()
        {
            timer.Interval = ConfigAniamtion.Instance.Time;

            timer.Tick += new EventHandler(timer_Tick);
        }

        public void Play()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        public void Run()
        {
           TriMeshUtil.TransformationRotation(GlobalData.Instance.TriMesh, new Vector3D(0, 0, 0.001d));

        }
    }
}
