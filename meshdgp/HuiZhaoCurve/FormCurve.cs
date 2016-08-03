using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
 
using System.Text;
using System.Windows.Forms;

 

namespace GraphicResearchHuiZhao
{
    public partial class FormCurve : Form
    {

        

        

        // Invoke the Changed event; called whenever list changes
        protected virtual void OnChanged(EventArgs e)
        {
            GlobalData.Instance.OnChanged(e);
        }

        private static FormCurve instance = null;
        public static FormCurve Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FormCurve();
                }
                return instance;
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

        private FormCurve()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            foreach (EnumCurveSimple type in Enum.GetValues(typeof(EnumCurveSimple)))
            {
                this.comboBoxCurve.Items.Add(type);
            }
            this.textBoxVertexNum.Text = ParameterCurve.Instance.VerticesNum.ToString();


            foreach (EnumCurveComplex type in Enum.GetValues(typeof(EnumCurveComplex))) 
            {
                this.comboBoxCurveComplexType.Items.Add(type);
            }

        }

        private void FormCurve_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing) return;

            e.Cancel = true;
            this.Hide();
        }

        private void comboBoxCurve_SelectedValueChanged(object sender, EventArgs e)
        {

            
            DrawCurve();
            
        }

        private void DrawCurve()
        {
            EnumCurveSimple curveType = (EnumCurveSimple)this.comboBoxCurve.SelectedItem;

            Mesh = ParameterCurve.Instance.CreateCurve(curveType);
            TriMeshUtil.ScaleToUnit(Mesh,0.9);
            TriMeshUtil.MoveToCenter(Mesh);
            GlobalSetting.DisplaySetting.DisplayMode = EnumDisplayMode.Vertex;
            OnChanged(EventArgs.Empty);
        }

        private void hScrollBarNum_ValueChanged(object sender, EventArgs e)
        {
            ParameterCurve.Instance.VerticesNum = this.hScrollBarNum.Value;
            this.textBoxVertexNum.Text = this.hScrollBarNum.Value.ToString();
            DrawCurve();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            ParameterCurve.Instance.VerticesNum = int.Parse(this.textBoxVertexNum.Text);
            
        }

        private void buttonBCPoint_Click(object sender, EventArgs e)
        {
            Mesh = ParameterCurve.Instance.CreateFourBezierControlPoint();


            OnChanged(EventArgs.Empty);
        }

        private void comboBoxCurveComplexType_SelectedValueChanged(object sender, EventArgs e)
        {
            EnumCurveComplex curveType = (EnumCurveComplex)this.comboBoxCurveComplexType.SelectedItem;

            ParameterCurve.Instance.currentCurve = curveType;

            switch (curveType)
            {

                case EnumCurveComplex.FourPointBezier:

                    Mesh = ParameterCurve.Instance.CreateFourBezierControlPoint();
                    break;

                case EnumCurveComplex.ThreePointBezier:

                    Mesh = ParameterCurve.Instance.CreateThreeBezierControlPoint();
                    break;

                case EnumCurveComplex.NPointBezier:

                    Mesh = ParameterCurve.Instance.CreateNBezierControlPoint();
                    break;

                case EnumCurveComplex.NURBS:

                    Mesh = ParameterCurve.Instance.CreateNURBSControlPoint();
                    break;

                case EnumCurveComplex.NPointBSpline:

                    Mesh = ParameterCurve.Instance.CreateNBSpineControlPoint();
                    break;

                case EnumCurveComplex.FourPointBSpline:

                    Mesh = ParameterCurve.Instance.CreateFourPointBSpline();
                    break;

                case EnumCurveComplex.NURBSCicle:

                    Mesh = ParameterCurve.Instance.CreateNURBSCicleControlPoint();
                    break;

                case EnumCurveComplex.NURBSEllipse:

                    Mesh = ParameterCurve.Instance.CreateNURBSEllipseControlPoint();
                    break;



             
            }

            TriMeshUtil.ScaleToUnit(Mesh,0.9);
            TriMeshUtil.MoveToCenter(Mesh);

            GlobalSetting.DisplaySetting.DisplayMode = EnumDisplayMode.PointsWithLine;
            ToolPool.Instance.Tool = new ToolCurveComplex(ToolPool.Instance.Width, ToolPool.Instance.Height, Mesh);
            OnChanged(EventArgs.Empty);
        }

        private void buttonView_Click(object sender, EventArgs e)
        {
            ToolPool.Instance.SwitchTool(EnumTool.View);
            
        }

        private void buttonSP_Click(object sender, EventArgs e)
        {
         
            ToolPool.Instance.Tool = new ToolCurveComplex(ToolPool.Instance.Width, ToolPool.Instance.Height,Mesh);
            GlobalSetting.DisplaySetting.DisplayMode = EnumDisplayMode.PointsWithLine;
        }

        private void comboBoxCurve_SelectedIndexChanged(object sender, EventArgs e)
        {
            DrawCurve();
        }
    }
}
