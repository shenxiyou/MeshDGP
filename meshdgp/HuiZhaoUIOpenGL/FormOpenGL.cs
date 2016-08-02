using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicResearchHuiZhao
{
    public partial class FormOpenGL : Form
    { 
        
        
        protected virtual void OnChanged(EventArgs e)
        {
            GlobalData.Instance.OnChanged(e);
        }

        private static FormOpenGL singleton = new FormOpenGL();



      
        public static FormOpenGL Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new FormOpenGL();
                return singleton;
            }
        }

        private  FormOpenGL()
        {
            try
            {
                FormAttributeOpenGL.SetUp();


                InitializeComponent();

                this.propertyGridDisplaySetting.SelectedObject = GlobalSetting.DisplaySetting;  
                this.propertyGridMaterial.SelectedObject = GlobalSetting.MaterialSetting; 

                InitLight();
            }
            catch (Exception e)
            {

            }
        }

        private void FormOpenGL_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing) return;
            e.Cancel = true;

            this.Hide();
        }

        private void propertyGridDisplaySetting_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            OnChanged(EventArgs.Empty);
        }

        private void tabPageEnableCaps_Enter(object sender, EventArgs e)
        {
            this.dataGridViewEnableCaps.Rows.Clear();

            List<KeyValuePair<string, string>> info = OpenGLManager.Instance.GetOpenGLPEnableCapsInfo();

            for (int i = 0; i < info.Count; i++)
            {
                this.dataGridViewEnableCaps.Rows.Add(i.ToString(),info[i].Key, info[i].Value);
            }
        }

        
        private void InitLight()
        {
            this.treeViewLight.Nodes.Clear();
            this.treeViewLight.Nodes.Add("LightSetting");

            this.treeViewLight.Nodes.Add("Light0");
            this.treeViewLight.Nodes.Add("Light1");
            this.treeViewLight.Nodes.Add("Light2");
            this.treeViewLight.Nodes.Add("Light3");
            this.treeViewLight.Nodes.Add("Light4");
            this.treeViewLight.Nodes.Add("Light5");
            this.treeViewLight.Nodes.Add("Light6");
            this.treeViewLight.Nodes.Add("Light7");


            this.propertyGridLight.SelectedObject = GlobalSetting.Light0Setting;
        }

        private void treeViewLight_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            switch (e.Node.Text)
            {

                case "LightSetting":
                    this.propertyGridLight.SelectedObject = GlobalSetting.SettingLight;
                    break;
                case "Light0":
                    this.propertyGridLight.SelectedObject = GlobalSetting.Light0Setting;
                    break;
                case "Light1":
                    this.propertyGridLight.SelectedObject = GlobalSetting.Light1Setting;
                    break;
                case "Light2":
                    this.propertyGridLight.SelectedObject = GlobalSetting.Light2Setting;
                    break;
                case "Light3":
                    this.propertyGridLight.SelectedObject = GlobalSetting.Light3Setting;
                    break;
                case "Light4":
                    this.propertyGridLight.SelectedObject = GlobalSetting.Light4Setting;
                    break;
                case "Light5":
                    this.propertyGridLight.SelectedObject = GlobalSetting.Light5Setting;
                    break;
                case "Light6":
                    this.propertyGridLight.SelectedObject = GlobalSetting.Light6Setting;
                    break;
                case "Light7":
                    this.propertyGridLight.SelectedObject = GlobalSetting.Light7Setting;
                    break;
                 
                    

            }
        }


        private  OpenFileDialog openFileDialog=new OpenFileDialog();

        private void buttonLoadTexture_Click(object sender, EventArgs e)
        {
            this.openFileDialog.Filter = "Picture files (*.jpeg;*.png;*.bmp;*.jpg;*.dds)|*.jpeg;*.png;*.bmp;*.jpg;*.dds|All files (*.*)|*.*";
            this.openFileDialog.CheckFileExists = true;
            this.openFileDialog.FileName = "";
            if (openFileDialog.ShowDialog() != DialogResult.OK) return;

            string filename = this.openFileDialog.FileName;


            OpenGLManager.Instance.LoadTexture(filename);


            pictureBoxTexture.ImageLocation = filename;

            DataGridTextureRefresh();
         
        }

        

        private void DataGridTextureRefresh()
        {
            this.dataGridViewTexture.Rows.Clear();
            for (int i = 0; i < OpenGLManager.Instance.TextureList.Count; i++)
            {
                int enable = 0;
                if (OpenGLManager.Instance.TextureList[i].ID > 0)
                {
                    enable = 1;
                }
                this.dataGridViewTexture.Rows.Add(i.ToString(), 
                                                  OpenGLManager.Instance.TextureList[i].ID.ToString(),
                                                  OpenGLManager.Instance.TextureList[i].FileName,
                                                  enable);
            }
        }

       

       

      

       

        private void tabPageTexture_Enter(object sender, EventArgs e)
        {
            this.propertyGridTextureInfo.SelectedObject = GlobalSetting.TextureSetting;
        }

        private void propertyGridTextureInfo_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            OnChanged(EventArgs.Empty);
        }

        private void tabPageOpenGLCard_Enter(object sender, EventArgs e)
        {
            this.dataGridViewOpenGLCard.Rows.Clear();

            List<KeyValuePair<string, string>> info = OpenGLManager.Instance.GetOpenGLCardInfo();

            for (int i = 0; i < info.Count; i++)
            {
                this.dataGridViewOpenGLCard.Rows.Add(info[i].Key,info[i].Value);
            }
        }

        private void tabPageOpenGLPName_Enter(object sender, EventArgs e)
        {

            this.dataGridViewPName.Rows.Clear();

            List<KeyValuePair<string, string>> info = OpenGLManager.Instance.GetOpenGLPNameInfo();
            for (int i = 0; i < info.Count; i++)
            {
                this.dataGridViewPName.Rows.Add(i.ToString(),info[i].Key, info[i].Value);
            }


            
        }

        

        private void propertyGridClipPlane_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            OnChanged(EventArgs.Empty);
        }

        

        private void propertyGridFog_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            OnChanged(EventArgs.Empty);
        }

        private void tabPageFog_Enter(object sender, EventArgs e)
        {
            this.propertyGridFog.SelectedObject = GlobalSetting.FogSetting;
        }

        private void tabPageClipPlane_Enter(object sender, EventArgs e)
        {
            this.propertyGridClipPlane.SelectedObject = GlobalSetting.ClipPlaneSetting;
        }

        private void tabPageDepth_Enter(object sender, EventArgs e)
        {
            this.propertyGridDepth.SelectedObject = GlobalSetting.TestSetting;
        }

        private void propertyGridDepth_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            OnChanged(EventArgs.Empty);
        }

        private void tabPageBlend_Enter(object sender, EventArgs e)
        {
            this.propertyGridBlend.SelectedObject = GlobalSetting.BlendSetting;
        }

        private void propertyGridBlend_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            OnChanged(EventArgs.Empty);
        }

        private void tabPageEnableCap_Enter(object sender, EventArgs e)
        {
            this.propertyGridEnableCap.SelectedObject = GlobalSetting.EnalbeCapsSetting;
        }

        private void propertyGridEnableCap_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            OnChanged(EventArgs.Empty);
        }

        private void dataGridViewTexture_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {

                string value = this.dataGridViewTexture[e.ColumnIndex, e.RowIndex].Value.ToString();

                if (value == "1")
                {
                    OpenGLManager.Instance.CurrentTexture = OpenGLManager.Instance.TextureList[e.RowIndex];
                    OpenGLManager.Instance.CurrentTexture.ID = OpenGLManager.Instance.InitTexture(OpenGLManager.Instance.CurrentTexture.FileName);
        
                    pictureBoxTexture.ImageLocation = OpenGLManager.Instance.TextureList[e.RowIndex].FileName;

                     
                }
                else
                {
                    OpenGLManager.Instance.ReleaseTexture(OpenGLManager.Instance.TextureList[e.RowIndex].ID);
                    OpenGLManager.Instance.TextureList[e.RowIndex].ID = 0;
                }
                
                OnChanged(EventArgs.Empty);
            }

           
        }

       

        private void propertyGridLight_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            OnChanged(EventArgs.Empty);
        }

       
        

    }
}
