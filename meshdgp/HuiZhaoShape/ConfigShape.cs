using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public partial class ConfigShape
    {
        private static ConfigShape singleton = new ConfigShape();
        public static ConfigShape Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new ConfigShape();
                return singleton;
            }
        }

        private double scale = 0.01;
        [Category("Selection")]
        public double Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
            }
        }

        private bool selectionOnly = true;
        [Category("Selection")]
        public bool SelectionOnly
        {
            get
            {
                return selectionOnly;
            }
            set
            {
                selectionOnly = value;
            }
        }


        private string saveFileDir = "../..//workspace/SelSave";
        [Category("Selection")]
        public string SaveFileDir
        {
            get
            {
                return saveFileDir;
            }
            set
            {
                saveFileDir = value;
            }
        }

        private bool saveDefaultDir = true;
        [Category("Selection")]
        public bool SaveDefaultDir
        {
            get
            {
                return saveDefaultDir;
            }
            set
            {
                saveDefaultDir = value;
            }
        }


        private string vertexFile = "../..//workspace/shape/sphere200.obj";
        [Category("Selection")]
        public string VertexFile
        {
            get
            {
                return vertexFile;
            }
            set
            {
                vertexFile = value;
            }
        }

        private string edgeFile = "../..//workspace/shape/cylinder12.obj";
        [Category("Selection")]
        public string EdgeFile
        {
            get
            {
                return edgeFile;
            }
            set
            {
                edgeFile = value;
            }
        }


         [Category("TestShape")]
        public int Type
        {
            get
            {
                return TriMeshShape.Instance.Type;
            }
            set
            {
                TriMeshShape.Instance.Type = value;
            }
        }


      

        [Category("Square")]
        public int SquareLength
        {
            get
            {
                return TriMeshShape.Instance.SquareLength;
            }
            set
            {
                TriMeshShape.Instance.SquareLength = value;
            }
        }

        [Category("Square")]
        public int SquareWidth
        {
            get
            {
                return TriMeshShape.Instance.SquareWidth;
            }
            set
            {
                TriMeshShape.Instance.SquareWidth = value;
            }
        }


        [Category("Cylinder")]
        public int CylinderLength
        {
            get
            {
                return TriMeshShape.Instance.CylinderLength;
            }
            set
            {
                TriMeshShape.Instance.CylinderLength = value;
            }
        }

        [Category("Cylinder")]
        public int CylinderWidth
        {
            get
            {
                return TriMeshShape.Instance.CylinderWidth;
            }
            set
            {
                TriMeshShape.Instance.CylinderWidth = value;
            }
        }

        [Category("Cylinder")]
        public int CylinderHeight
        {
            get
            {
                return TriMeshShape.Instance.CylinderHeight;
            }
            set
            {
                TriMeshShape.Instance.CylinderHeight = value;
            }
        }


        [Category("Cylinder")]

        public double GridSizeX
        {
            get
            {
                return TriMeshShape.Instance.GridSizeX;
            }
            set
            {
                TriMeshShape.Instance.GridSizeX = value;
            }
        }

        [Category("Cylinder")]

        public double GridSizeY
        {
            get
            {
                return TriMeshShape.Instance.GridSizeY;
            }
            set
            {
                TriMeshShape.Instance.GridSizeY = value;
            }
        }

        [Category("Cylinder")]

        public double GridSizeZ
        {
            get
            {
                return TriMeshShape.Instance.GridSizeZ;
            }
            set
            {
                TriMeshShape.Instance.GridSizeZ = value;
            }
        }


        [Category("Circle")]
        public int CircleVertex
        {
            get
            {
                return TriMeshShape.Instance.CircleVertex;
            }
            set
            {
                TriMeshShape.Instance.CircleVertex = value;
            }
        }

        [Category("Gird")]
        public int GridX
        {
            get
            {
                return TriMeshShape.Instance.PlaneGridX;
            }
            set
            {
                TriMeshShape.Instance.PlaneGridX = value;
            }
        }

        [Category("Gird")]
        public int GridY
        {
            get
            {
                return TriMeshShape.Instance.PlaneGridY;
            }
            set
            {
                TriMeshShape.Instance.PlaneGridY = value;
            }
        }

        [Category("Gird")]
        public double PlaneGridSizeX
        {
            get
            {
                return TriMeshShape.Instance.PlaneGridSizeX;
            }
            set
            {
                TriMeshShape.Instance.PlaneGridSizeX = value;
            }
        }
        [Category("Gird")]
        public double PlaneGridSizeY
        {
            get
            {
                return TriMeshShape.Instance.PlaneGridSizeY;
            }
            set
            {
                TriMeshShape.Instance.PlaneGridSizeY = value;
            }
        }

        [Category("Cone")]
        public int ConeNum
        {
            get
            {
                return TriMeshShape.Instance.ConeNum;
            }
            set
            {
                TriMeshShape.Instance.ConeNum = value;
            }
        }


        [Category("Cone")]
        public double ConeHeight
        {
            get
            {
                return TriMeshShape.Instance.ConeHeight;
            }
            set
            {
                TriMeshShape.Instance.ConeHeight = value;
            }
        }


        [Category("CylinderUV")]
        public int CylinderUVm
        {
            get
            {
                return TriMeshShape.Instance.CylinderUVm;
            }
            set
            {
                TriMeshShape.Instance.CylinderUVm = value;
            }
        }

        [Category("CylinderUV")]
        public int CylinderUVn
        {
            get
            {
                return TriMeshShape.Instance.CylinderUVn;
            }
            set
            {
                TriMeshShape.Instance.CylinderUVn = value;
            }
        }

        [Category("CylinderUV")]
        public double CylinderUVr
        {
            get
            {
                return TriMeshShape.Instance.CylinderUVr;
            }
            set
            {
                TriMeshShape.Instance.CylinderUVr = value;
            }
        }

        [Category("CylinderUV")]
        public double CylinderUVl
        {
            get
            {
                return TriMeshShape.Instance.CylinderUVl;
            }
            set
            {
                TriMeshShape.Instance.CylinderUVl = value;
            }
        }

        [Category("CylinderUV")]
        public double CylinderUVMaxU
        {
            get
            {
                return TriMeshShape.Instance.CylinderUVMaxU;
            }
            set
            {
                TriMeshShape.Instance.CylinderUVMaxU = value;
            }
        }
        [Category("CylinderUV")]
        public double CylinderUVMaxV
        {
            get
            {
                return TriMeshShape.Instance.CylinderUVMaxV;
            }
            set
            {
                TriMeshShape.Instance.CylinderUVMaxV = value;
            }
        }


        [Category("CylinderUV")]
        public int CylinderUVDiff
        {
            get
            {
                return TriMeshShape.Instance.CylinderUVDiff;
            }
            set
            {
                TriMeshShape.Instance.CylinderUVDiff = value;
            }
        }


        [Category("Sphere")]
        public int SphereNum
        {
            get
            {
                return TriMeshShape.Instance.SphereNum;
            }
            set
            {
                TriMeshShape.Instance.SphereNum = value;
            }
        }

    }
}
