using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao
{
    public partial class FormConfigFunc : Form
    {
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


        private static FormConfigFunc singleton = new FormConfigFunc();

        public static FormConfigFunc Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new FormConfigFunc();
                return singleton;
            }
        }

        public HistogramControl Hist
        {
            get
            {
                return this.histogramControl;
            }
        }

        public void Refresh()
        {
            this.propertyGridConfig.Refresh();
            this.DisplayValue(TriMeshFunction.Instance.Function);
            this.histogramControl.UpdateHist(TriMeshFunction.Instance.Function);

        }

        public void DisplayValue(double[] function)
        {
            this.dataGridViewFuncvalue.Rows.Clear();


            Morse morse = new Morse(Mesh, TriMeshFunction.Instance.Function);
            EnumMorseVertexType[] morseType= morse.ComputeMorse(Mesh, TriMeshFunction.Instance.Function);

            int[] m = morse.ComputeMorseChange(Mesh, function);

            for (int i = 0; i < TriMeshFunction.Instance.Function.Length; i++)
            {
                this.dataGridViewFuncvalue.Rows.Add(i.ToString(), function[i].ToString(),morseType[i],m[i]);
            }

            this.textBoxMorse.Text = morse.BuildMorseTheory(Mesh, TriMeshFunction.Instance.Function);
        }


        // An event that clients can use to be notified whenever the
        // elements of the list change.
        public event MeshChangedDelegate Changed;

        protected virtual void OnChanged(EventArgs e)
        {
            

            if (Changed != null)
                Changed(this, e);
        }

        public FormConfigFunc()
        {
            InitializeComponent();
            this.propertyGridConfig.SelectedObject =  ConfigFunction.Instance;


        }

        private void propertyGridConfig_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {

            if (e.ChangedItem.Label =="MinClamp" || e.ChangedItem.Label =="MaxClamp")
            {


                double[] function = TriMeshFunction.Instance.Clamp(TriMeshFunction.Instance.Function, ConfigFunction.Instance.MinClamp, ConfigFunction.Instance.MaxClamp);
                GlobalData.Instance.ColorVis = TriMeshFunction.Instance.Unit(function, ConfigFunction.Instance.Nodal);
               // this.DisplayValue(function);
                GlobalData.Instance.OnChanged(e);
                return;
            }

           
           

            OnChanged(EventArgs.Empty);

        }

        private void FormFuncConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing) return;
            e.Cancel = true;

            this.Hide();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            this.Refresh();
        }
    }
}
