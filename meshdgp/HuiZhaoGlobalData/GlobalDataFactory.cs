using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public partial class GlobalData
    {
        public double[] ColorVis;
        public List<BindBox> boxes;
        public List<TriMesh.Edge> tree;
        public List<TriMesh.Edge> cotree;

        public List<List<TriMesh.HalfEdge>> Generators;

        public Vector3D[] FaceVectors;

        public int N;


        public TriMesh MeshOne;
        public TriMesh MeshTwo;
        public TriMesh MeshThree;


        public PolygonMesh DualMesh;
        

        

    }
}
