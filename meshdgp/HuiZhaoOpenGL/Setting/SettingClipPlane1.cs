using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public class SettingClipPlane
    {
		private bool enable0 = false;

        private Plane plane0 = new Plane(0, 0, 1, 0);
	
        public bool Enable0
        {
            get { return enable0; }
            set { enable0 = value; }
        }

        public Plane Plane0
        {
            get { return plane0; }
            set { plane0 = value; }
        }
		private bool enable1 = false;

        private Plane plane1 = new Plane(0, 1, 0, 0);
	
        public bool Enable1
        {
            get { return enable1; }
            set { enable1 = value; }
        }

        public Plane Plane1
        {
            get { return plane1; }
            set { plane1 = value; }
        }
		private bool enable2 = false;

        private Plane plane2 = new Plane(0, 1, 1, 0);
	
        public bool Enable2
        {
            get { return enable2; }
            set { enable2 = value; }
        }

        public Plane Plane2
        {
            get { return plane2; }
            set { plane2 = value; }
        }
		private bool enable3 = false;

        private Plane plane3 = new Plane(1, 0, 0, 0);
	
        public bool Enable3
        {
            get { return enable3; }
            set { enable3 = value; }
        }

        public Plane Plane3
        {
            get { return plane3; }
            set { plane3 = value; }
        }
		private bool enable4 = false;

        private Plane plane4 = new Plane(1, 0, 1, 0);
	
        public bool Enable4
        {
            get { return enable4; }
            set { enable4 = value; }
        }

        public Plane Plane4
        {
            get { return plane4; }
            set { plane4 = value; }
        }
		private bool enable5 = false;

        private Plane plane5 = new Plane(1, 1, 0, 0);
	
        public bool Enable5
        {
            get { return enable5; }
            set { enable5 = value; }
        }

        public Plane Plane5
        {
            get { return plane5; }
            set { plane5 = value; }
        }
    }
}