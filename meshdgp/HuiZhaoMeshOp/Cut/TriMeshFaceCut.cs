using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class TriMeshFaceCut
    {
        TriMesh mesh;
        Dictionary<TriMesh.Vertex, TriMesh.Vertex> vMap;
        bool[] fFlag;

        List<TriMesh.Edge> removeEdge = new List<HalfEdgeMesh.Edge>();
        List<TriMesh.Vertex> removeVertex = new List<HalfEdgeMesh.Vertex>();

        public TriMeshFaceCut(TriMesh mesh)
        {
            this.mesh = mesh;
        }

        public TriMesh Cut(IEnumerable<TriMesh.Vertex> region)
        {
            Dictionary<int, TriMesh.Vertex> vMap = new Dictionary<int, HalfEdgeMesh.Vertex>();
            Dictionary<int, bool> fFlag = new Dictionary<int, bool>();

      
            TriMesh newMesh = new TriMesh();

            foreach (var v in region)
            {
                TriMesh.Vertex newV = new HalfEdgeMesh.Vertex(
                    new VertexTraits(v.Traits.Position));
                vMap[v.Index] = newV;
                newMesh.AppendToVertexList(newV);
            }

            foreach (var f in this.mesh.Faces)
            {
                bool inner = true;
                foreach (var v in f.Vertices)
                {
                    if (!vMap.ContainsKey(v.Index))
                    {
                        inner = false;
                        break;
                    }
                }
                if (inner)
                {
                    TriMesh.HalfEdge hf = f.HalfEdge;
                    newMesh.Faces.AddTriangles(
                        vMap[hf.FromVertex.Index],
                        vMap[hf.ToVertex.Index],
                        vMap[hf.Next.ToVertex.Index]);
                    fFlag[f.Index] = true;
                }
            }

            List<TriMesh.Edge> remove = new List<HalfEdgeMesh.Edge>();

            foreach (var e in this.mesh.Edges)
            {
                if (fFlag.ContainsKey(e.Face0.Index) 
                    && fFlag.ContainsKey(e.Face1.Index))
                {
                    remove.Add(e);
                }
            }
            foreach (var e in remove)
            {
                TriMeshModify.RemoveEdge(e);
            }

            foreach (var v in region)
            {
                bool inner = true;
                foreach (var round in v.Vertices)
                {
                    if (!vMap.ContainsKey(round.Index))
                    {
                        inner = false;
                    }
                }
                if (inner)
                {
                    this.mesh.RemoveVertex(v);
                }
            }

            return newMesh;
        }

        public TriMesh Cut(IEnumerable<TriMesh.Face> region)
        {
            this.vMap = new Dictionary<TriMesh.Vertex, HalfEdgeMesh.Vertex>();
            this.fFlag = new bool[this.mesh.Faces.Count];

            TriMesh newMesh = this.Create(region);
            this.Record(region);
            this.Remove(region);

            return newMesh;
        }

        TriMesh Create(IEnumerable<TriMesh.Face> region)
        {
            TriMesh newMesh = new TriMesh();

            foreach (var f in region)
            {
                TriMesh.Vertex[] arr = new HalfEdgeMesh.Vertex[3];
                int i = 0;
                foreach (var v in f.Vertices)
                {
                    if (!this.vMap.ContainsKey(v))
                    {
                        TriMesh.Vertex newV =
                            new HalfEdgeMesh.Vertex(new VertexTraits(v.Traits.Position));
                        this.vMap[v] = newV;
                        newMesh.AppendToVertexList(newV);
                    }
                    arr[i++] = this.vMap[v];
                }
                newMesh.Faces.AddTriangles(arr);
                this.fFlag[f.Index] = true;
            }

            return newMesh;
        }

        void Record(IEnumerable<TriMesh.Face> region)
        {
            this.removeEdge = new List<HalfEdgeMesh.Edge>();
            this.removeVertex = new List<HalfEdgeMesh.Vertex>();

            foreach (var e in this.mesh.Edges)
            {
                if ((e.Face0 == null || fFlag[e.Face0.Index]) &&
                    (e.Face1 == null || fFlag[e.Face1.Index]))
                {
                    removeEdge.Add(e);
                }
            }

            foreach (var v in this.mesh.Vertices)
            {
                bool inner = true;
                foreach (var f in v.Faces)
                {
                    if (!fFlag[f.Index])
                    {
                        inner = false;
                        break;
                    }
                }
                if (inner)
                {
                    removeVertex.Add(v);
                }
            }
        }

        public void Remove(IEnumerable<TriMesh.Face> region)
        { 
            foreach (var f in region)
            {
                TriMeshModify.RemoveFace(f);
            }

            foreach (var e in removeEdge)
            {
                TriMeshModify.RemoveEdge(e);
            }

            foreach (var v in removeVertex)
            {
                TriMeshModify.RemoveVertex(v);
            }
        }
    }
}
