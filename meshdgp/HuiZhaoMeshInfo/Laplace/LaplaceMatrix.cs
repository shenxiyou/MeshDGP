using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public partial class LaplaceManager
    {
        public SparseMatrix BuildLaplaceGraph(TriMesh mesh)
        {
            SparseMatrix vv = BuildAdjacentMatrixVV(mesh);
            SparseMatrix degree = BuildMatrixDegree(mesh); 
            SparseMatrix graph = degree.Minus(vv);
            graph.SortElement();
            return graph;
        }

        public SparseMatrix BuildLaplaceTutte(TriMesh mesh)
        {
            SparseMatrix m = new SparseMatrix(mesh.Vertices.Count,
                                              mesh.Vertices.Count);
            foreach (TriMesh.Vertex vertex in mesh.Vertices)
            {
                foreach (TriMesh.Vertex neigh in vertex.Vertices)
                {
                    double result = -1d / vertex.VertexCount;
                    m.AddElementIfNotExist(vertex.Index,
                                 neigh.Index,
                                 result);
                }
                m.AddValueTo(vertex.Index, vertex.Index, 1);
            }
            m.SortElement();
            return m;
        }

        public SparseMatrix BuildLaplaceGraphNomalized(TriMesh mesh)
        {
            SparseMatrix m = new SparseMatrix(mesh.Vertices.Count,
                                               mesh.Vertices.Count);
            foreach (TriMesh.Vertex vertex in mesh.Vertices)
            {
                foreach (TriMesh.Vertex neigh in vertex.Vertices)  
                {
                    double result = -1d / Math.Sqrt(vertex.VertexCount 
                                                 * neigh.VertexCount);
                    m.AddValueTo(vertex.Index,
                                 neigh.Index, 
                                 result);
                }
                m.AddValueTo(vertex.Index, vertex.Index, 1);
            }
            m.SortElement();
            return m;
        }

        public SparseMatrix BuildLaplaceTutteSymmetrized(TriMesh mesh)
        {
            SparseMatrix tutte = BuildLaplaceTutte(mesh); 
            SparseMatrix sysmtutte = tutte.Add(tutte.Transpose());
            sysmtutte.Multiply(0.5); 
            return sysmtutte;
        }

        public SparseMatrix BuildLaplaceTutteSymmetrizedTwo(TriMesh mesh)
        {
            SparseMatrix tutte = BuildLaplaceTutte(mesh);
            SparseMatrix sysmtutte = tutte.Multiply(tutte.Transpose()); 
            return sysmtutte;
        }

        public SparseMatrix BuildLaplaceCot(TriMesh mesh)
        {
            int n = mesh.Vertices.Count;
            SparseMatrix L = new SparseMatrix(n, n);
            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                int c1 = mesh.Faces[i].GetVertex(0).Index;
                int c2 = mesh.Faces[i].GetVertex(1).Index;
                int c3 = mesh.Faces[i].GetVertex(2).Index;
                Vector3D v1 = mesh.Faces[i].GetVertex(0).Traits.Position;
                Vector3D v2 = mesh.Faces[i].GetVertex(1).Traits.Position;
                Vector3D v3 = mesh.Faces[i].GetVertex(2).Traits.Position;
                double cot1 = (v2 - v1).Dot(v3 - v1) / (v2 - v1).Cross(v3 - v1).Length();
                double cot2 = (v3 - v2).Dot(v1 - v2) / (v3 - v2).Cross(v1 - v2).Length();
                double cot3 = (v1 - v3).Dot(v2 - v3) / (v1 - v3).Cross(v2 - v3).Length();
                L.AddValueTo(c1, c2, -cot3 / 2); L.AddValueTo(c2, c1, -cot3 / 2);
                L.AddValueTo(c2, c3, -cot1 / 2); L.AddValueTo(c3, c2, -cot1 / 2);
                L.AddValueTo(c3, c1, -cot2 / 2); L.AddValueTo(c1, c3, -cot2 / 2);
            }
            for (int i = 0; i < n; i++)
            {
                double sum = 0;
                foreach (SparseMatrix.Element e in L.Rows[i])
                {
                    sum += e.value;
                }
                L.AddValueTo(i, i, -sum);
            }
            L.SortElement();
            return L; 
        }

        

    }
}
