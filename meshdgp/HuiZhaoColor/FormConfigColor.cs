using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media;

namespace GraphicResearchHuiZhao
{
    public partial class FormConfigColor : Form
    {
        public FormConfigColor()
        {
            InitializeComponent();
            this.propertyGridColor.SelectedObject = ConfigColor.Instance;
        }


        private static FormConfigColor singleton = new FormConfigColor();

        public static FormConfigColor Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new FormConfigColor();
                return singleton;
            }
        }


        public System.Drawing.Color Color = System.Drawing.Color.AliceBlue;
        private void FormAnimationConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing) return;
            e.Cancel = true;

            this.Hide();
        }

        private void propertyGridColor_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            GenerateColors();
        }

        private void GenerateColors()
        {
            var colors = GreateColor.GetColors(ConfigColor.Instance.Scheme, ConfigColor.Instance.Luminosity, ConfigColor.Instance.NumberToGenerate);


            this.flowLayoutPanel1.Controls.Clear();
            foreach (var c in colors)
            { 
                Button b = new Button();
                b.BackColor = System.Drawing.Color.FromArgb(c.A, c.R, c.G, c.B);
                b.Click += b_Click; 
                this.flowLayoutPanel1.Controls.Add(b);
            }
            
        }

        void b_Click(object sender, EventArgs e)
        {
           Button b= sender as Button ;
           this.Color = System.Drawing.Color.FromArgb(b.BackColor.R, b.BackColor.G, b.BackColor.B);
        }
    }
}
