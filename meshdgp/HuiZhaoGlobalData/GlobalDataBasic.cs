using System;
using System.Collections.Generic;
using System.Text; 
using System.IO;
 
 
 



namespace GraphicResearchHuiZhao
{
    public partial class GlobalData
    {
        private static GlobalData instance = null;
        public static GlobalData Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GlobalData();
                }
                return instance;
            }
        }

        private GlobalData()
        {

        }




        // An event that clients can use to be notified whenever the
        // elements of the list change.
        public event MeshChangedDelegate Changed;

        // Invoke the Changed event; called whenever list changes
        public   void OnChanged(EventArgs e)
        {
            if (Changed != null)
                Changed(this, e);
        }
        

       

        private TriMesh triMesh = null;
        public TriMesh TriMesh
        {
            get { return triMesh; }
            set
            {         
                triMesh = value;
                
                 
            }
        }


        private QuadMesh quadMesh = null;
        public QuadMesh QuadMesh
        {
            get { return quadMesh; }
            set
            {
                quadMesh = value;

            }
        }

        private MeshRecord simpleMesh = null;
        public MeshRecord SimpleMesh
        {
            get { return simpleMesh; }
            set
            {
                
                simpleMesh = value;
 
            }
        }

        private TetMesh tetMesh = null;

        public TetMesh TetMesh
        {
            get
            {
                return tetMesh;
            }
            set
            {
                tetMesh = value;
            }
        }

        public List<TriMesh> ske = new List<TriMesh>();

        public List<TriMesh> AllMeshes = new List<TriMesh>();
      
    }



}
