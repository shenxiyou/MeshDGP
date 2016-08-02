using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace GraphicResearchHuiZhao
{
    public partial class FormConfigNPR : Form
    {
        public FormConfigNPR()
        {
            InitializeComponent();

            this.InitTabLine();
            this.InitColorGrid();
        }

        private static FormConfigNPR singleton = new FormConfigNPR();


        public static FormConfigNPR Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new FormConfigNPR();
                return singleton;
            }
        }

        public TriMesh Mesh
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


        private void FormConfigNPR_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing) return;
            e.Cancel = true; 
            this.Hide();
            GlobalSetting.DisplaySetting.DisplayMode = EnumDisplayMode.Basic;
        }

        private void InitTabLine()
        { 

            for (int i = 0; i < NPRLines.All.Length; i++)
            {
                int index = i;
                CheckBox cb = new CheckBox();
                this.flowLine.Controls.Add(cb);
                cb.Text = NPRLines.All[i].Name;
                cb.AutoSize = true;
                cb.CheckedChanged += (object sender, EventArgs e) =>
                {
                    ConfigNPR.Instance.Enable[index] = cb.Checked;
                    OnChanged(EventArgs.Empty);
                };
            }
            this.basicGrid.SelectedObject = ConfigNPR.Instance;
            this.basicGrid.PropertyValueChanged += this.propertyGrid_PropertyValueChanged;
        }

        void InitColorGrid()
        {
            this.colorGrid.SelectedObject = new ConfigColorLine();
            this.colorGrid.PropertyValueChanged += this.propertyGrid_PropertyValueChanged;
        }

        void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            GlobalData.Instance.OnChanged(EventArgs.Empty);
        }


    }
}
