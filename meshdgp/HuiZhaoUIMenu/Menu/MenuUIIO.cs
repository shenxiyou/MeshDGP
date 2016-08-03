using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Threading;

namespace GraphicResearchHuiZhao
{
    public partial class MenuUIIO
    {
        private static MenuUIIO instance = null;
        public static MenuUIIO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MenuUIIO();
                }
                return instance;
            }
        }

        MeshLoader meshLoader = new MeshLoader();

        ToolStripProgressBar progressBar = new ToolStripProgressBar();


        public SaveFileDialog saveFileDialog = new SaveFileDialog();
        public OpenFileDialog openFileDialog = new OpenFileDialog();

        public string SetUpSaveDialog()
        {
            this.saveFileDialog.Filter = "Mesh files (*.obj)|*.obj|Mesh files (*.off)|*.off|Mesh files (*.ply)|*.ply|All files (*.*)|*.*";
            this.saveFileDialog.OverwritePrompt = true;
            this.saveFileDialog.FileName = "";
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return null;
            }
            else
            {
                return this.saveFileDialog.FileName.ToLower();
            }
        }


        public string SetUpOpenDialog()
        {
            this.openFileDialog.Filter = "Mesh files (*.obj)|*.obj|Mesh files (*.off)|*.off|Mesh files (*.ply)|*.ply|All files (*.*)|*.*";
            this.openFileDialog.CheckFileExists = true;
            this.openFileDialog.FileName = "";
            this.openFileDialog.Title = "Open Manifold Mesh";
            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return null;
            }
            else
            {
                return this.openFileDialog.FileName.ToLower();
            }
        }


        public void OpenTriMesh()
        {
             String filename =  SetUpOpenDialog();

             if (filename == null)
                 return;

             else
             {

                 
                 if (filename.EndsWith(".obj"))
                 {
                     NonManifoldMesh mesh = new NonManifoldMesh();
                     mesh.ReadObj(filename);

                     MeshRecord rec = new MeshRecord(filename, mesh);

                     GlobalData.Instance.SimpleMesh = rec;


                 }
             }
             GlobalData.Instance.OnChanged(EventArgs.Empty);

        }


        public void OpenBigMesh()
        {
             String filename =  SetUpOpenDialog();

             if (filename == null)
                 return;

             else
             { 
                 if (meshLoader != null && meshLoader.IsAlive)
                 {
                     meshLoader.Abort();

                 }
                 this.progressBar.Visible = true;
                 FileInfo fileInfo = new FileInfo(filename);
                 if (fileInfo.Length > 1048576)  // Open files larger than 1MB in their own thread
                 {
                     meshLoader = new MeshLoader();
                     meshLoader.MeshProcessFinished += new EventHandler(meshLoader_MeshLoaded);
                     meshLoader.Load(filename);

                    
                     

                 }
                 else
                 {
                     
                     GlobalData.Instance.TriMesh = TriMeshIO.ReadFile(filename);
                     this.progressBar.Visible = false;
                 }
             }
        }

        public void meshLoader_MeshLoaded(object sender, EventArgs e)
        {
            //if (!this.meshView3D.InvokeRequired)
            //{
            //    GlobalData.Instance.TriMesh = meshLoader.Mesh;
            //    this.progressBar.Visible = false;
            //}
            //else
            //{
            //    EventHandler meshLoadedHandler = new EventHandler(meshLoader_MeshLoaded);
            //    this.meshView3D.BeginInvoke(meshLoadedHandler, new object[2] { sender, e });
            //}
        }


        public void SaveMesh()
        {
            string filename = SetUpSaveDialog();

            if (filename == null)
                return;

            else
            {
                string ext = Path.GetExtension(filename);
                if (ext == ".obj")
                {
                    TriMeshIO.WriteToObjFile(filename, GlobalData.Instance.TriMesh);
                }

                if (ext == ".ply")
                {
                    TriMeshIO.WriteToPlyFile(filename, GlobalData.Instance.TriMesh);
                }
            } 
           
        }

        public void SaveSelection()
        {
            string filename = SetUpSaveDialog();

            if (filename == null)
                return;

            else
            {
                TriMeshIO.WriteSelection(filename, GlobalData.Instance.TriMesh);

            }

        }


        public void SaveMeshWithTexture()
        {
            string filename = SetUpSaveDialog();

            if (filename == null)
                return;

            else
            {
                TriMeshIO.WriteToObjFileWithTexture(filename, GlobalData.Instance.TriMesh);

            }

        }


        public void OpenManiFoldFile()
        {
            String filename =  SetUpOpenDialog();

            if (filename == null)
                return;

            else
            { 
                GlobalData.Instance.TriMesh = TriMeshIO.ReadFile(filename); 
            }

            GlobalData.Instance.OnChanged(EventArgs.Empty);
        }


        public void AddOneMore()
        {
            String filename = SetUpOpenDialog();

            if (filename == null)
                return;

            else
            {
                GlobalData.Instance.AllMeshes.Add(TriMeshIO.ReadFile(filename));

                GlobalData.Instance.TriMesh = GlobalData.Instance.AllMeshes[GlobalData.Instance.AllMeshes.Count - 1];
            }

            GlobalData.Instance.OnChanged(EventArgs.Empty);
        }


        public void OpenManiFoldFileTwo()
        {
            String filename = SetUpOpenDialog();

            if (filename == null)
                return;

            else
            {
                GlobalData.Instance.MeshTwo = TriMeshIO.ReadFile(filename);
            }

            GlobalData.Instance.OnChanged(EventArgs.Empty);
        }



        public void OpenQuadMesh()
        {
            String filename = SetUpOpenDialog();

            if (filename == null)
                return;

            else
            {
                
                        
                        QuadMesh mesh = QuadMesh.FromOffFile(filename);

                        QuadMeshUtil.ScaleToUnit(mesh, 1.0);
                        QuadMeshUtil.MoveToCenter(mesh);
                        QuadMeshUtil.ComputeNormal(mesh);
                        GlobalData.Instance.QuadMesh = mesh;
                       
                
            } 
        }

        public void Release()
        {
            GlobalData.Instance.TriMesh = null; 
            GlobalData.Instance.OnChanged(EventArgs.Empty);
        }

        public void ClearQuad()
        {
            GlobalData.Instance.QuadMesh = null;
            GlobalData.Instance.OnChanged(EventArgs.Empty);
        }
    }
}
