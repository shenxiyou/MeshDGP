using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao
{
    public partial class FormMVPMatrix : Form
    {

 

        
 
        protected virtual void OnChanged(EventArgs e)
        {
            GlobalData.Instance.OnChanged(e);
        }

        private static FormMVPMatrix singleton = new FormMVPMatrix();


        public static FormMVPMatrix Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new FormMVPMatrix();
                return singleton;
            }
        }

        public FormMVPMatrix()
        {
           

            try
            {
                FormAttributeTransform.SetUp();

                InitializeComponent();

                MatrixView.Title = "View";
                MatrixModel.Title = "Model";

                MatrixProjection.Title = "Projection";
                MatrixWhole.Title = "Whole";

                this.propertyGridSetting.SelectedObject = SettingTransform.Instance;

                SetMatrixValues();

            }
            catch (Exception e)
            {

            }
        }

        private void SetMatrixValues()
        {
            MatrixView.Matrix = TransformController.Instance.ViewMatrix;
            MatrixModel.Matrix = TransformController.Instance.ModelMatrix;

            MatrixProjection.Matrix = TransformController.Instance.ProjectionMatrix;
            MatrixWhole.Matrix = TransformController.Instance.ModelMatrix * TransformController.Instance.ViewMatrix * TransformController.Instance.ProjectionMatrix;          
        }

        private void numericModelValueChanged(object sender, EventArgs e)
        {
            double modelPositionX = (double)numericModelTranslateX.Value;
            double modelPositionY = (double)numericModelTranslateY.Value;
            double modelPositionZ = (double)numericModelTranslateZ.Value;

            double modelRotationX = (double)numericModelRotationX.Value;
            double modelRotationY = (double)numericModelRotationY.Value;
            double modelRotationZ = (double)numericModelRotationZ.Value;

     
            modelRotationX = modelRotationX * Math.PI / 180;
            modelRotationY = modelRotationY * Math.PI / 180;
            modelRotationZ = modelRotationZ * Math.PI / 180;

            double modelScaleX = (double)numericModelScaleX.Value;
            double modelScaleY = (double)numericModelScaleY.Value;
            double modelScaleZ = (double)numericModelScaleZ.Value;

            double modelShearX = (double)numericModelShearingX.Value;
            double modelShearY = (double)numericModelShearingY.Value;
            double modelShearZ = (double)numericModelShearingZ.Value;

            double modelMirrorX = (double)mirrorX.Value;
            double modelMirrorY = (double)mirrorY.Value;
            double modelMirrorZ = (double)mirrorZz.Value;

            TransformController.Instance.ModelMatrix = Matrix4D.Identity();

            TransformController.Instance.ModelScale(modelScaleX, modelScaleY, modelScaleZ);
            TransformController.Instance.ModelRotate(modelRotationX, modelRotationY, modelRotationZ);
            TransformController.Instance.ModelTranslate(modelPositionX, modelPositionY, modelPositionZ);
            TransformController.Instance.ModelMirror(modelMirrorX, modelMirrorY, modelMirrorZ);
            TransformController.Instance.ModelShear(modelShearX, modelShearY, modelShearZ);
            
            

            OnChanged(EventArgs.Empty);
            SetMatrixValues();

            this.Refresh();

            
        }

        

        private void numericViewValueChanged(object sender, EventArgs e)
        {
            double viewRotationX = (double)numbericViewRotationX.Value;
            double viewRotationY = (double)numbericViewRotationY.Value;
            double viewRotationZ = (double)numbericViewRotationZ.Value;

            viewRotationX = viewRotationX * Math.PI / 180;
            viewRotationY = viewRotationY * Math.PI / 180;
            viewRotationZ = viewRotationZ * Math.PI / 180;

            double viewPositionX = (double)numericViewPositionX.Value;
            double viewPositionY = (double)numericViewPositionY.Value;
            double viewPositionZ = (double)numericViewPositionZ.Value;

            TransformController.Instance.ViewMatrix = Matrix4D.Identity();            
            TransformController.Instance.TranslateCamera(viewPositionX, viewPositionY, viewPositionZ);
            TransformController.Instance.RotateCamera(viewRotationX, viewRotationY, viewRotationZ);

            OnChanged(EventArgs.Empty);
            SetMatrixValues();

            this.Refresh();

            

        }

        private void FormMVPMatrix_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing) return;
            e.Cancel = true;

            this.Hide();

        }

        private void propertyGridSetting_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            TransformController.Instance.ModelMatrix = Matrix4D.Identity();

            //TransformController.Instance.ModelScale(SettingTransform.Instance.ModelScale);

            //if (SettingTransform.Instance.UseRotationAxis)
            //{
            //    TransformController.Instance.ModelRotate(SettingTransform.Instance.RotationAxis, SettingTransform.Instance.RotationV);
            //}
            //else
            //{
            //    TransformController.Instance.ModelRotate(SettingTransform.Instance.ModelRotation);
            //}


            //TransformController.Instance.ModelTranslate(SettingTransform.Instance.ModelTranslate);
            //TransformController.Instance.ModelMirror(SettingTransform.Instance.ModelMirror);
            //TransformController.Instance.ModelShear(SettingTransform.Instance.ModelShearing);

            //TransformController.Instance.ViewMatrix = Matrix4D.Identity();
            //TransformController.Instance.TranslateCamera(SettingTransform.Instance.EyePos);
            //TransformController.Instance.RotateCamera(SettingTransform.Instance.EyeDirection);

            //TransformController.Instance.ModelRotate(SettingTransform.Instance.RotationAxis, SettingTransform.Instance.RotationV);

            TransformController.Instance.ModelQuaternion(SettingTransform.Instance.Quaternion, SettingTransform.Instance.QuaternionDegree);

            OnChanged(EventArgs.Empty);
            SetMatrixValues();
            this.Refresh();
        }

         
    }
}
