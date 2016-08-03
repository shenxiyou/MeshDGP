using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Design;

namespace GraphicResearchHuiZhao
{
    public class ConfigTet
    {
        private static ConfigTet singleton = new ConfigTet();


        public static ConfigTet Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new ConfigTet();
                return singleton;
            }
        }


        bool lq = true;
        decimal maxRatio = 2;
        decimal minAngle = 0;

        bool la = false;
        decimal maxVolume = 0.1M;

        bool lf = true;
        bool le = true;
        bool ln = false;
        bool lg = false;
        bool lk = false;

        [Category("Remeshing")]
        [DisplayName("-q")]
        [DefaultValue(true)]
        [Description("Refines mesh (to improve mesh quality)")]
        public bool ParamLq
        {
            get { return this.lq; }
            set { this.lq = value; }
        }

        [Category("Remeshing")]
        [DisplayName("MaxRatio")]
        [DefaultValue(2)]
        [Description("Maximum allowable radius-edge ratio")]
        [Editor(typeof(NumericScrollBarEditor), typeof(UITypeEditor))]
        [Range(0.1d, 10d, 0.1d)]
        public decimal MaxRatio
        {
            get { return this.maxRatio; }
            set { this.maxRatio = value; }
        }

        [Category("Remeshing")]
        [DisplayName("MinAngle")]
        [DefaultValue(0)]
        [Description("Minimum allowable dihedral angle")]
        [Editor(typeof(NumericScrollBarEditor), typeof(UITypeEditor))]
        [Range(0, 180, 1)]
        public decimal MinAngle
        {
            get { return this.minAngle; }
            set { this.minAngle = value; }
        }

        [Category("Remeshing")]
        [DisplayName("-a")]
        [DefaultValue(false)]
        [Description("Imposes a maximum volume constraint on all tetrahedra")]
        public bool ParamLa
        {
            get { return this.la; }
            set { this.la = value; }
        }

        [Category("Remeshing")]
        [DisplayName("MaxVolume")]
        [DefaultValue(0.001)]
        [Description("Max Volume")]
        [Editor(typeof(NumericScrollBarEditor), typeof(UITypeEditor))]
        [Range(0, 0.001, 0.00001)]
        public decimal MaxVolume
        {
            get { return this.maxVolume; }
            set { this.maxVolume = value; }
        }

        [Category("Output")]
        [DisplayName("-f")]
        [DefaultValue(true)]
        [Description("Outputs all faces to .face file")]
        public bool ParamLf
        {
            get { return this.lf; }
            set { this.lf = value; }
        }

        [Category("Output")]
        [DisplayName("-e")]
        [DefaultValue(true)]
        [Description("Outputs all edges to .edge file")]
        public bool ParamLe
        {
            get { return this.le; }
            set { this.le = value; }
        }

        [Category("Output")]
        [DisplayName("-n")]
        [DefaultValue(true)]
        [Description("Outputs tetrahedra neighbors to .neigh file")]
        public bool ParamLn
        {
            get { return this.ln; }
            set { this.ln = value; }
        }

        [Category("Output")]
        [DisplayName("-g")]
        [DefaultValue(false)]
        [Description("Outputs mesh to .mesh file for viewing by Medit")]
        public bool ParamLg
        {
            get { return this.lg; }
            set { this.lg = value; }
        }

        [Category("Output")]
        [DisplayName("-k")]
        [DefaultValue(false)]
        [Description("Outputs mesh to .vtk file for viewing by Paraview")]
        public bool ParamLk
        {
            get { return this.lk; }
            set { this.lk = value; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('-');
            if (this.lq)
            {
                sb.Append('q');
                sb.Append(this.maxRatio.ToString());
                sb.Append('/');
                sb.Append(this.minAngle.ToString());
                if (this.la)
                {
                    sb.Append('a');
                    sb.Append(this.maxVolume.ToString());
                }
            }
            if (this.lf) sb.Append('f');
            if (this.le) sb.Append('e');
            if (this.ln) sb.Append('n');
            if (this.lg) sb.Append('g');
            if (this.lk) sb.Append('k');

            return sb.ToString();
        }
    }
}
