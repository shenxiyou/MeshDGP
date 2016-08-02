//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace GraphicResearchHuiZhao
//{
//    public partial class TriMeshSimpification
//    {
//        #region Vertex Decimation

//        private TriMesh.Edge GetMinCurvatureEdge(MinHeapTwo<TriMesh.Vertex> heap)
//        {
//            List<HeapNode<TriMesh.Vertex>> unMergable = new List<HeapNode<TriMesh.Vertex>>();
//            HeapNode<TriMesh.Vertex> node = heap.Pull();
//            while (node != null)
//            {
//                foreach (var edge in node.Item.Edges)
//                {
//                    if (TriMeshModify.IsMergeable(edge))
//                    {
//                        foreach (var item in unMergable)
//                        {
//                            heap.Add(item);
//                        }
//                        return edge;
//                    }
//                }
//                unMergable.Add(node);
//                node = heap.Pull();
//            }
//            return null;
//        }

//        public void DecimationGassian(int vertexPreserved)
//        {
//            MinHeapTwo<TriMesh.Vertex> heap = new MinHeapTwo<TriMesh.Vertex>(this.mesh.Vertices.Count);
//            HeapNode<TriMesh.Vertex>[] handle = new HeapNode<TriMesh.Vertex>[this.mesh.Vertices.Count];

//            foreach (TriMesh.Vertex v in this.mesh.Vertices)
//            {
//                double curvature = TriMeshUtil.ComputeGaussianCurvature(v);
//                handle[v.Index] = heap.Add(Math.Abs(curvature), v);
//            }

//            while (mesh.Vertices.Count > vertexPreserved)
//            {
//                TriMesh.Edge target = this.GetMinCurvatureEdge(heap);
//                if (target == null)
//                {
//                    break;
//                }

//                TriMesh.Edge min = this.GetMinCurvatureEdge(heap);

//                this.mesh.RemoveVertices.Clear();
//                TriMesh.Vertex v1 = TriMeshModify.MergeEdge(target);
//                foreach (var v in this.mesh.RemoveVertices)
//                {
//                    heap.Del(handle[v.Index]);
//                }

//                double v1Curvature = TriMeshUtil.ComputeGaussianCurvature(v1);
//                heap.Update(handle[v1.Index], Math.Abs(v1Curvature));
//                foreach (var v in v1.Vertices)
//                {
//                    double curvature = TriMeshUtil.ComputeGaussianCurvature(v);
//                    heap.Update(handle[v.Index], Math.Abs(curvature));
//                }
//            }

//            TriMeshUtil.FixIndex(mesh);

//        }

//        private TriMesh.Edge GetShortEdge(MinHeapTwo<TriMesh.Edge> heap)
//        {
//            List<HeapNode<TriMesh.Edge>> unMergable = new List<HeapNode<TriMesh.Edge>>();
//            HeapNode<TriMesh.Edge> node = heap.Pull();
//            while (node != null && !TriMeshModify.IsMergeable(node.Item))
//            {
//                unMergable.Add(node);
//                node = heap.Pull();
//            }

//            foreach (var item in unMergable)
//            {
//                heap.Add(item);
//            }
//            return node == null ? null : node.Item;
//        }

//        public void DecimationEdge(int vertexPreserved)
//        {
//            MinHeapTwo<TriMesh.Edge> heap = new MinHeapTwo<TriMesh.Edge>(this.mesh.Edges.Count);
//            HeapNode<TriMesh.Edge>[] handle = new HeapNode<TriMesh.Edge>[this.mesh.Edges.Count];

//            foreach (TriMesh.Edge edge in this.mesh.Edges)
//            {
//                double length = TriMeshUtil.ComputeEdgeLength(edge);
//                handle[edge.Index] = heap.Add(length, edge);
//            }

//            while (mesh.Vertices.Count > vertexPreserved)
//            {
//                TriMesh.Edge target = this.GetShortEdge(heap);
//                if (target == null)
//                {
//                    break;
//                }

//                this.mesh.RemoveEdges.Clear();
//                TriMesh.Vertex v1 = TriMeshModify.MergeEdge(target);
//                foreach (var edge in this.mesh.RemoveEdges)
//                {
//                    heap.Del(handle[edge.Index]);
//                }

//                foreach (var edge in v1.Edges)
//                {
//                    double length = TriMeshUtil.ComputeEdgeLength(edge);
//                    heap.Update(handle[edge.Index], length);
//                }
//            }
//            TriMeshUtil.FixIndex(mesh);
//        }

//        private TriMesh.Edge GetSmallFaceEdge(MinHeapTwo<TriMesh.Face> heap)
//        {
//            List<HeapNode<TriMesh.Face>> unMergable = new List<HeapNode<TriMesh.Face>>();
//            HeapNode<TriMesh.Face> node = heap.Pull();
//            while (node != null)
//            {
//                foreach (var edge in node.Item.Edges)
//                {
//                    if (TriMeshModify.IsMergeable(edge))
//                    {
//                        foreach (var item in unMergable)
//                        {
//                            heap.Add(item);
//                        }
//                        return edge;
//                    }
//                }
//                unMergable.Add(node);
//                node = heap.Pull();
//            }
//            return null;
//        }

//        public void DecimationArea(int vertexPreserved)
//        {
//            MinHeapTwo<TriMesh.Face> heap = new MinHeapTwo<TriMesh.Face>(this.mesh.Faces.Count);
//            HeapNode<TriMesh.Face>[] handle = new HeapNode<TriMesh.Face>[this.mesh.Faces.Count];

//            foreach (TriMesh.Face face in mesh.Faces)
//            {
//                double area = TriMeshUtil.ComputeAreaFace(face);
//                handle[face.Index] = heap.Add(area, face);
//            }

//            while (mesh.Vertices.Count > vertexPreserved)
//            {
//                TriMesh.Edge target = this.GetSmallFaceEdge(heap);
//                if (target == null)
//                {
//                    break;
//                }

//                this.mesh.RemoveFaces.Clear();
//                TriMesh.Vertex v1 = TriMeshModify.MergeEdge(target);
//                foreach (var face in this.mesh.RemoveFaces)
//                {
//                    heap.Del(handle[face.Index]);
//                }

//                foreach (var face in v1.Faces)
//                {
//                    double area = TriMeshUtil.ComputeAreaFace(face);
//                    heap.Update(handle[face.Index], area);
//                }
//            }
//            TriMeshUtil.FixIndex(mesh);
//        }

//        public void VertexDecimation(int vertexPreserved, EnumDecimationType type)
//        {
//            switch (type)
//            {
//                case EnumDecimationType.Gaussian:
//                    DecimationGassian1(vertexPreserved);
//                    break;
//                case EnumDecimationType.Length:
//                    DecimationEdge(vertexPreserved);
//                    break;
//                case EnumDecimationType.Area:
//                    DecimationArea(vertexPreserved);
//                    break;
//            }
//        }

//        #endregion

//        #region 没用堆的

//        private TriMesh.Edge GetMinCurvatureEdge()
//        {
//            TriMesh.Edge target = null;
//            double min = double.MaxValue;
//            foreach (var v in this.mesh.Vertices)
//            {
//                double curvature = Math.Abs(v.Traits.GaussianCurvature);
//                if (curvature < min)
//                {
//                    foreach (var edge in v.Edges)
//                    {
//                        if (TriMeshModify.IsMergeable(edge))
//                        {
//                            target = edge;
//                            min = curvature;
//                            break;
//                        }
//                    }
//                }
//            }
//            return target;
//        }

//        public void DecimationGassian1(int vertexPreserved)
//        {
//            TriMeshUtil.ComputeGaussianCurvature(mesh);

//            while (mesh.Vertices.Count > vertexPreserved)
//            {
//                TriMesh.Edge target = this.GetMinCurvatureEdge();
//                if (target == null)
//                {
//                    break;
//                }

//                TriMesh.Vertex v1 = TriMeshModify.MergeEdge(target);

//                TriMeshUtil.ComputeGaussianCurvature(v1);
//                foreach (var round in v1.Vertices)
//                {
//                    TriMeshUtil.ComputeGaussianCurvature(round);
//                }
//            }

//            TriMeshUtil.FixIndex(mesh);

//        }

//        private TriMesh.Edge GetSmallFaceEdge(TriMesh.FaceDynamicTrait<double> areaTraits)
//        {
//            TriMesh.Edge target = null;
//            double min = double.MaxValue;
//            foreach (var face in this.mesh.Faces)
//            {
//                double area = areaTraits[face];
//                if (area < min)
//                {
//                    foreach (var edge in face.Edges)
//                    {
//                        if (TriMeshModify.IsMergeable(edge))
//                        {
//                            target = edge;
//                            min = area;
//                            break;
//                        }
//                    }
//                }
//            }
//            return target;
//        }

//        public void DecimationArea1(int vertexPreserved)
//        {
//            TriMesh.FaceDynamicTrait<double> areaTraits = new TriMesh.FaceDynamicTrait<double>(this.mesh);
//            foreach (TriMesh.Face face in mesh.Faces)
//            {
//                areaTraits[face] = TriMeshUtil.ComputeAreaFace(face);
//            }

//            while (mesh.Vertices.Count > vertexPreserved)
//            {
//                TriMesh.Edge target = this.GetSmallFaceEdge(areaTraits);
//                if (target == null)
//                {
//                    break;
//                }

//                TriMesh.Vertex v1 = TriMeshModify.MergeEdge(target);
//                foreach (var face in v1.Faces)
//                {
//                    areaTraits[face] = TriMeshUtil.ComputeAreaFace(face);
//                }
//            }
//            TriMeshUtil.FixIndex(mesh);

//        }

//        private TriMesh.Edge GetShortEdge(TriMesh.EdgeDynamicTrait<double> lengthTraits)
//        {
//            TriMesh.Edge target = null;
//            double min = double.MaxValue;
//            foreach (var edge in this.mesh.Edges)
//            {
//                double length = lengthTraits[edge];
//                if (length < min && TriMeshModify.IsMergeable(edge))
//                {
//                    target = edge;
//                    min = length;
//                }
//            }
//            return target;
//        }

//        public void DecimationEdge1(int vertexPreserved)
//        {
//            TriMesh.EdgeDynamicTrait<double> lengthTraits = new TriMesh.EdgeDynamicTrait<double>(this.mesh);
//            foreach (TriMesh.Edge edge in this.mesh.Edges)
//            {
//                lengthTraits[edge] = TriMeshUtil.ComputeEdgeLength(edge);
//            }

//            while (mesh.Vertices.Count > vertexPreserved)
//            {
//                TriMesh.Edge target = this.GetShortEdge(lengthTraits);
//                if (target == null)
//                {
//                    break;
//                }

//                TriMesh.Vertex v1 = TriMeshModify.MergeEdge(target);
//                foreach (var edge in v1.Edges)
//                {
//                    lengthTraits[edge] = TriMeshUtil.ComputeEdgeLength(edge);
//                }
//            }

//            TriMeshUtil.FixIndex(mesh);

//        }

//        #endregion
//    }
//}
