using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao
{
    public partial class FormWolfram : Form
    {
        public FormWolfram()
        {
            InitializeComponent();
            this.propertyGridConfig.SelectedObject = FormWolfram.Instance;
        }

        private static FormWolfram singleton = new FormWolfram();

        public static FormWolfram Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new FormWolfram();
                return singleton;
            }
        }

        public Bitmap PicBox
        {
            set 
            {
                this.pictureBox.Image = value;
                this.pictureBox.Refresh();
            }
            get
            {
                return (Bitmap)this.pictureBox.Image;
            }
        }

        private void propertyGridConfig_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {

        }

    }
}
