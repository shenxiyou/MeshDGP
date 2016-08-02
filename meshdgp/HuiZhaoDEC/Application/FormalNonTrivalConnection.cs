//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace GraphicResearchHuiZhao
//{
//    public struct TransportData
//    {
//        public double delta;
//        public double sign;
//        public double[] omega;
//        public double[] alphaI;
//        public double[] alphaJ;
//    }

//    public class PriorNonTrivalConnection
//    {
//        public int[] MapRow;

//        public TreeCoTree treeCotree;

//        public double[] Theta; //Connection betwenn incident faces

//        #region Util

//        protected double ComputeParallTransport(double phi,TriMesh.HalfEdge hf)
//        {
//            if (hf.OnBoundary || hf.Opposite.OnBoundary)
//            {
//                return 0;
//            }

//            Vector3D e = hf.FromVertex.Traits.Position - hf.ToVertex.Traits.Position;

//            Vector3D al, bl;
//            FaceFrame(hf.Face.HalfEdge, out al, out bl);

//            Vector3D ar, br;
//            FaceFrame(hf.Opposite.Face.HalfEdge, out ar, out br);

//            double deltaL = Math.Atan2(e.Dot(bl), e.Dot(al));
//            double deltaR = Math.Atan2(e.Dot(br), e.Dot(ar));

//            return (phi - deltaL) + deltaR;
//        }

//        #endregion


//        public List<List<TriMesh.HalfEdge>> Append1RingBases(TriMesh mesh)
//        {
//            MapRow = new int[mesh.Vertices.Count];
//            List<List<TriMesh.HalfEdge>> cycles = new List<List<HalfEdgeMesh.HalfEdge>>();

//            foreach (TriMesh.Vertex vertex in mesh.Vertices)
//            {
//                if (vertex.OnBoundary)
//                {
//                    MapRow[vertex.Index] = vertex.Index;
//                    continue;
//                }

//                List<TriMesh.HalfEdge> cycle = new List<HalfEdgeMesh.HalfEdge>();

//                foreach (TriMesh.HalfEdge vhf in vertex.Halfedges)
//                {
//                    cycle.Add(vhf);
//                }

//                MapRow[vertex.Index] = cycle.Count;

//                cycles.Add(cycle);
//            }

//            return cycles;
//        }

//        public void AppendDirectionalConstraints(TriMesh mesh,List<List<TriMesh.HalfEdge>> cycles)
//        {
//            TriMesh.Face faceTransport = mesh.Faces[3];
//            DynamicTree<TriMesh.Face> transportTree = new DynamicTree<HalfEdgeMesh.Face>();
//            transportTree.Root = new TreeNode<HalfEdgeMesh.Face>(faceTransport);

//            Queue<TreeNode<HalfEdgeMesh.Face>> queue = new Queue<TreeNode<HalfEdgeMesh.Face>>();
//            queue.Enqueue(transportTree.Root);

//            bool[] processedFlag = new bool[mesh.Faces.Count];

//            while (queue.Count != 0)
//            {
//                TreeNode<HalfEdgeMesh.Face> currentFaceNode = queue.Dequeue();

//                TriMesh.HalfEdge startHf = currentFaceNode.Attribute.HalfEdge;
//                TriMesh.HalfEdge currentHf = startHf;
//                do{
//                    TriMesh.Face neighborFace = currentHf.Opposite.Face;

//                    if (processedFlag[neighborFace.Index] == false &&
//                        neighborFace != faceTransport &&
//                        neighborFace.OnBoundary
//                        )
//                    {
//                        TreeNode<HalfEdgeMesh.Face> neighNode = new TreeNode<HalfEdgeMesh.Face>(neighborFace);
//                        currentFaceNode.AddChild(neighNode);
//                        queue.Enqueue(neighNode);

//                        TransportData td = new TransportData();
//                        td.delta = ComputeParallTransport(0, currentHf);
//                        td.sign = currentHf.FromVertex.Index > currentHf.ToVertex.Index ? -1:1;
//                        td.omega = 
//                    }

//                    currentHf = currentHf.Next;
//                }while(currentHf != startHf);


//            }


//        }


//        public void Build(TriMesh mesh)
//        {
//            if (treeCotree == null)
//            {
//                treeCotree = new TreeCoTree(mesh);
//            }
        
//            List<List<TriMesh.HalfEdge>> basisCycles = Append1RingBases(mesh);
//            int nContractibleCycles = basisCycles.Count;

//            List<List<TriMesh.HalfEdge>> dualCycles = treeCotree.ExtractHonologyGenerator(mesh);
//            //Append to basisCycle
//            foreach (List<TriMesh.HalfEdge> dualCycle in dualCycles)
//            {
//                basisCycles.Add(dualCycle);
//            }

//            int nBasisCycles = basisCycles.Count;





//        }

//    }
//}
