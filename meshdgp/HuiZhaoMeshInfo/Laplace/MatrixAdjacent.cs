


using System;
using System.Collections.Generic;
using System.Text;



namespace GraphicResearchHuiZhao
{

    public partial class LaplaceManager
    {


        public SparseMatrix BuildTwoRingVV(TriMesh mesh)
        {
            int n = mesh.Vertices.Count;
            SparseMatrix L = new SparseMatrix(n, n, 6);

            for (int i = 0; i < n; i++)
            {
                foreach (TriMesh.Vertex v1 in mesh.Vertices[i].Vertices)
                    foreach (TriMesh.Vertex v2 in mesh.Vertices[v1.Index].Vertices)
                        L.AddElementIfNotExist(i, v2.Index, 1);
            }
            L.SortElement();
            return L;
        }

        public int[][] BuildMatrixAAdjInfo(SparseMatrix A)
        {
            if (A == null)
                throw new Exception("A matrix is null");

            int[][] adj = new int[A.ColumnSize][];

            Set<int> s = new Set<int>();
            for (int i = 0; i < A.ColumnSize; i++)
            {
                s.Clear();

                List<SparseMatrix.Element> col = A.Columns[i];
                foreach (SparseMatrix.Element e in col)
                {
                    List<SparseMatrix.Element> row = A.Rows[e.i];
                    foreach (SparseMatrix.Element e2 in row)
                        s.Add(e2.j);
                }
                adj[i] = s.ToArray();
            }

            return adj;
        }

        public SparseMatrix BuildAdjacentMatrixVV(TriMesh mesh)
        {
            SparseMatrix m = new SparseMatrix(mesh.Vertices.Count, 
                                              mesh.Vertices.Count);
            foreach (var center in mesh.Vertices)
            {
                foreach (var round in center.Vertices)
                {
                    m.AddElementIfNotExist(center.Index, 
                                      round.Index, 1.0);
                }
            } 
            m.SortElement();
            return m;
        }

        public SparseMatrix BuildAdjacentMatrixFV(TriMesh mesh)
        {
            SparseMatrix m = new SparseMatrix(mesh.Faces.Count, 
                                              mesh.Vertices.Count);
            foreach (var face in mesh.Faces)
            {
                foreach (var v in face.Vertices)
                {
                    m.AddElementIfNotExist(face.Index, v.Index, 1.0);
                }
            } 
            m.SortElement();
            return m;
        }

        public SparseMatrix BuildAdjacentMatrixFF(TriMesh mesh)
        {
            SparseMatrix m = new SparseMatrix(mesh.Faces.Count, 
                                              mesh.Faces.Count);

            foreach (var center in mesh.Faces)
            {
                foreach (var round in center.Faces)
                {
                    m.AddElementIfNotExist(center.Index, round.Index, 1.0);
                }
            } 
            m.SortElement();
            return m;
        }

        //private bool IsContainVertex(TriMesh.Face face, int vIndex)
        //{             
        //    int v1 = face.GetVertex(0).Index;
        //    int v2 = face.GetVertex(1).Index;
        //    int v3 = face.GetVertex(2).Index; 
        //    return (v1 == vIndex) || (v2 == vIndex) || (v3 == vIndex);
        //}


        public SparseMatrix BuildAdjacentMatrixVE(TriMesh mesh)
        {
            SparseMatrix m = new SparseMatrix(mesh.Vertices.Count, 
                                              mesh.Edges.Count);

            foreach (var v in mesh.Vertices)
            {
                foreach (var edge in v.Edges)
                {
                    m.AddElementIfNotExist(v.Index, edge.Index, 1.0);
                }
            }
            m.SortElement();
            return m;
        }


        public SparseMatrix BuildAdjacentMatrixVF(TriMesh mesh)
        {
            SparseMatrix m = new SparseMatrix(mesh.Vertices.Count, 
                                              mesh.Faces.Count);
            foreach (var v in mesh.Vertices)
            {
                foreach (var face in v.Faces)
                {
                    m.AddElementIfNotExist(v.Index, face.Index, 1.0);
                }
            }
            m.SortElement();
            return m;
        }

        public SparseMatrix BuildAdjacentMatrixEE(TriMesh mesh)
        {
            SparseMatrix m = new SparseMatrix(mesh.Edges.Count, 
                                              mesh.Edges.Count);
            foreach (var center in mesh.Edges)
            {
                foreach (var v in new[] { center.Vertex0, center.Vertex1 })
                {
                    foreach (var round in v.Vertices)
                    {
                        m.AddElementIfNotExist(center.Index, round.Index, 1.0);
                    }
                }
            }
            m.SortElement();
            return m;
        }

        public SparseMatrix BuildAdjacentMatrixEV(TriMesh mesh)
        {
            SparseMatrix m = new SparseMatrix(mesh.Edges.Count, 
                                              mesh.Vertices.Count);
            foreach (var edge in mesh.Edges)
            {
                m.AddElementIfNotExist(edge.Index, edge.Vertex0.Index, 1.0);
                m.AddElementIfNotExist(edge.Index, edge.Vertex1.Index, 1.0);
            }
            m.SortElement();
            return m;
        }

        public SparseMatrix BuildAdjacentMatrixEF(TriMesh mesh)
        {
            SparseMatrix m = new SparseMatrix(mesh.Edges.Count, 
                                              mesh.Faces.Count);
            foreach (var edge in mesh.Edges)
            {
                m.AddElementIfNotExist(edge.Index, edge.Face0.Index, 1.0);
                m.AddElementIfNotExist(edge.Index, edge.Face1.Index, 1.0);
            }
            m.SortElement();
            return m;
        }



        public SparseMatrix BuildAdjacentMatrixFE(TriMesh mesh)
        {
            SparseMatrix m = new SparseMatrix(mesh.Faces.Count, 
                                              mesh.Edges.Count);
            foreach (var face in mesh.Faces)
            {
                foreach (var edge in face.Edges)
                {
                    m.AddElementIfNotExist(face.Index, edge.Index, 1.0);
                }
            }
            m.SortElement();
            return m;
        }



        public SparseMatrix BuildMatrixDegree(TriMesh mesh)
        {
            SparseMatrix m = new SparseMatrix(mesh.Vertices.Count,
                                              mesh.Vertices.Count);
            foreach (TriMesh.Vertex vertex in mesh.Vertices)
            {

                m.AddElementIfNotExist(vertex.Index,
                                      vertex.Index, vertex.VertexCount);
                
            }
            m.SortElement();
            return m;
        }



    }
}
