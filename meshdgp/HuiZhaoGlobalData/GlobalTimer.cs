using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Threading;

namespace GraphicResearchHuiZhao
{
    public class GlobalTimer
    {
        private System.Timers.Timer animationTimer;
        public double DefaultInterval = 15;

        public delegate void OnTimerEventDelegate();

        private static GlobalTimer instance = new GlobalTimer();
        public static GlobalTimer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GlobalTimer();
                }
                return instance;
            }
        }

        
        private GlobalTimer()
        {
            InitTimer();
        }

        public void InitTimer()
        {
            animationTimer = new System.Timers.Timer();
            animationTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

            animationTimer.Interval = DefaultInterval;
            animationTimer.Enabled = true;
            PauseTimer();
        }

        public void PauseTimer()
        {
            animationTimer.Stop();
        }

        public void StartTimer()
        {
            animationTimer.Start();
        }

        public void SetSpeed(double speed)
        {
            animationTimer.Interval = speed;
        }

        public void OnTimedEvent(object source, ElapsedEventArgs e)
        {

            //if (MainForm.Instance.MeshView3D.InvokeRequired == true)
            //{

            //    MainForm.Instance.MeshView3D.Invoke(new OnTimerEventDelegate(MainForm.Instance.MeshView3D.Refresh));
            //}



        }

        public void DisableTimer()
        {
            animationTimer.Enabled = false;
        }

    }
}
