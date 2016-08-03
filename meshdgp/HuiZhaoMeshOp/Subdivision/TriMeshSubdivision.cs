
using System;
using System.Collections;
using System.Collections.Generic;  
using System.Text; 


namespace GraphicResearchHuiZhao
{

    public partial class TriMeshSubdivision
    {
        TriMesh.Vertex[] vMap;
        TriMesh.Vertex[] eMap;
        TriMesh.Vertex[] faceMap;

        public TriMesh Mesh = null;
        public TriMeshSubdivision(TriMesh Mesh)
        {
            this.Mesh = Mesh;
        }

       
        public TriMesh SubDivision(EnumSubdivision method)
        {
            TriMesh mesh = null;
            switch (method)
            {
                case EnumSubdivision.Loop:
                    mesh = SubdivisionLoop();
                    break;
                case EnumSubdivision.Sqrt3:
                    mesh = SubdivisonSqrt3();
                    break;
                case EnumSubdivision.Butterfly:
                    mesh =SubdivitionModifiedButtefly(0);
                    Console.WriteLine("V={0},E={1},F={2}", mesh.Vertices.Count, mesh.Edges.Count, mesh.Faces.Count);
                    break;
                case EnumSubdivision.Selected:
                    mesh = SubdivisionSelectedLoop();
                    break;
                default:
                    break;
            }

            TriMeshUtil.SetUpNormalVertex(mesh);
            return mesh;
        }
    }
}
