using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao
{
    public partial class FormDatabase : Form
    {
        public FormDatabase()
        {
            InitializeComponent();
            this.propertyGridDB.SelectedObject = ConfigDB.Instance;
        }

        private static FormDatabase singleton = new FormDatabase();


        public static FormDatabase Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new FormDatabase();
                return singleton;
            }
        }

        private void FormDatabase_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing) return;
            e.Cancel = true;
            this.Hide();
        }
    }
}
