
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
 

namespace GraphicResearchHuiZhao
{

    public partial class TriMesh
    {
        public SparseMatrix BuildLaplaceMatrixDual()
        {
            // build dual Laplacian weight matrix L

            int vn = this.Vertices.Count;
            int fn = this.Faces.Count;
            SparseMatrix L = new SparseMatrix(fn, vn, 6);

            for (int i = 0; i < fn; i++)
            {
                int f1 = this.Faces[i].GetFace(0).Index;
                int f2 = this.Faces[i].GetFace(1).Index;
                int f3 = this.Faces[i].GetFace(2).Index;

                Vector3D dv =  this.DualGetVertexPosition(i);
                Vector3D dv1 = this.DualGetVertexPosition(f1);
                Vector3D dv2 = this.DualGetVertexPosition(f2);
                Vector3D dv3 = this.DualGetVertexPosition(f3);
                Vector3D u = dv - dv3;
                Vector3D v1 = dv1 - dv3;
                Vector3D v2 = dv2 - dv3;
                Vector3D normal = (v1.Cross(v2)).Normalize();
                Matrix3D M = new Matrix3D(v1, v2, normal);
                Vector3D coord = M.Inverse() * u;
                double alpha;

                alpha = 1.0 / 3.0;

                L.AddValueTo(i, this.Faces[i].GetVertex(0).Index, alpha);
                L.AddValueTo(i, this.Faces[i].GetVertex(1).Index, alpha);
                L.AddValueTo(i, this.Faces[i].GetVertex(2).Index, alpha);

                alpha = coord[0] / 3.0;

                L.AddValueTo(i, this.Faces[f1].GetVertex(0).Index, -alpha);
                L.AddValueTo(i, this.Faces[f1].GetVertex(1).Index, -alpha);
                L.AddValueTo(i, this.Faces[f1].GetVertex(2).Index, -alpha);

                alpha = coord[1] / 3.0;

                L.AddValueTo(i, this.Faces[f2].GetVertex(0).Index, -alpha);
                L.AddValueTo(i, this.Faces[f2].GetVertex(1).Index, -alpha);
                L.AddValueTo(i, this.Faces[f2].GetVertex(2).Index, -alpha);

                alpha = (1.0 - coord[0] - coord[1]) / 3.0;

                L.AddValueTo(i, this.Faces[f3].GetVertex(0).Index, -alpha);
                L.AddValueTo(i, this.Faces[f3].GetVertex(1).Index, -alpha);
                L.AddValueTo(i, this.Faces[f3].GetVertex(2).Index, -alpha);


            }

            L.SortElement();
            return L;
        }


        public  NonManifoldMesh DualCreateMesh(TriMesh mesh)
        {
            NonManifoldMesh dualMesh = new NonManifoldMesh();
            dualMesh.VertexPos = mesh.DualCreateVertexPosition();
            dualMesh.FaceIndex = mesh.DualCreateFaceIndex();
            dualMesh.VertexCount = mesh.Faces.Count;
            dualMesh.FaceCount = dualMesh.FaceIndex.Length / 3;
            dualMesh.ScaleToUnitBox();
            dualMesh.MoveToCenter();
            dualMesh.ComputeFaceNormal();
            dualMesh.ComputeVertexNormal();
            
            return dualMesh;

        }




        //public double[][] ComputeLaplacianDual()
        //{
        //    SparseMatrix L = this.BuildLaplaceMatrixDual();

        //    double[][] lap = ComputeLaplacianBasic(L);

            

        //    return lap;
        //}
         

        public Vector3D DualGetVertexPosition(int faceIndex)
        {
            Vector3D dualposition=new Vector3D(0,0,0);
            foreach (Vertex vertex in this.Faces[faceIndex].Vertices)
            {
                dualposition += vertex.Traits.Position;
            }
            dualposition = dualposition / 3;
            return dualposition;
        }

        public double[] DualCreateVertexPosition()
        {
            double[] dualVertexPos = new double[this.Faces.Count * 3];

            for (int i = 0; i < dualVertexPos.Length; i++)
                dualVertexPos[i] = 0.0;

            for (int i = 0 ; i < this.Vertices.Count; i++ )
                foreach (Face face in this.Vertices[i].Faces ) 
                {
                
                    int b = face.Index * 3;
                    dualVertexPos[b] += this.Vertices[i].Traits.Position.x ;
                    dualVertexPos[b + 1] += this.Vertices[i].Traits.Position.y;
                    dualVertexPos[b + 2] += this.Vertices[i].Traits.Position.z;
                }

            for (int i = 0; i < dualVertexPos.Length; i++)
                dualVertexPos[i] /= 3.0; 
            

            return dualVertexPos;
        }

        private int[] DualCreateFaceIndex()
        {
            int fn = this.Faces.Count;
            int[] dualFaceIndex = new int[fn * 3 * 3];

            int cur = 0;
            for (int i = 0; i < this.Faces.Count; i++)
            {
                int neighbornum = this.Faces[i].FaceCount;


                int f1;
                int f2;
                int f3;

                if (neighbornum == 3)
                {
                    f1 = this.Faces[i].GetFace(0).Index;
                    f2 = this.Faces[i].GetFace(1).Index;
                    f3 = this.Faces[i].GetFace(2).Index;


                    dualFaceIndex[cur++] = f1;
                    dualFaceIndex[cur++] = f2;
                    dualFaceIndex[cur++] = f3;
                }

            }

            return dualFaceIndex;
        }






    }
}
