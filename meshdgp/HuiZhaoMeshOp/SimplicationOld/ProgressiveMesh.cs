//using System;
//using System.Collections.Generic; 
 

//namespace GraphicResearchHuiZhao
//{
//    public class TriMeshProgreesiveMesh
//    {


//        private TriMesh mesh;

//        public List<MergeEventBlock> EventList;

//        public int CurrentEventIndex;


//        public TriMeshProgreesiveMesh(TriMesh mesh)
//        {
//            this.mesh = mesh;
//            foreach (TriMesh.Vertex v in mesh.Vertices)
//            {
//                v.Traits.FixedIndex = v.Index;
//            }

//            //Simpifcation Using Cluster
//            EventList = new List<MergeEventBlock>();
//            CurrentEventIndex = -1;
//        } 


//        #region Merge Event Block
//        //Save every action
//        public struct MergeEventBlock
//        {
//            public int v1Index;
//            public int v2Index;
//            public int v1SharedIndex;
//            public int v2SharedIndex;

//            public Vector3D v1Position;
//            public Vector3D v2Position;
//            public Vector3D newPosition;
//        }

//        public void WriteProgressToFile()
//        {

//        }

//        public void ReadFromProgress()
//        {


//        }

//        #endregion

          
      
//        //For progressive mesh
//        public TriMesh.Vertex MergeVertiesWithRecord(TriMesh.Edge edge, VertexTraits vt, List<MergeEventBlock> blockList)
//        {
//            TriMesh.Vertex v1= edge.Vertex0;
//            TriMesh.Vertex v2 = edge.Vertex1;
//            MergeEventBlock eventBlock = new MergeEventBlock();
//            eventBlock.v1Index = v1.Traits.FixedIndex;
//            eventBlock.v2Index = v2.Traits.FixedIndex;

//            eventBlock.v1Position = v1.Traits.Position;
//            eventBlock.v2Position = v2.Traits.Position;

//            int count = 0;
//            foreach (TriMesh.HalfEdge hf1 in v1.Halfedges)
//            {
//                foreach (TriMesh.HalfEdge hf2 in v2.Halfedges)
//                {
//                    if (hf1.ToVertex == hf2.ToVertex)
//                    {
//                        count++;
//                    }
//                }
//            }

//            //Find TriMesh.Halfedge which share same to Vertex
//            v1.Traits.Position.x = vt.Position.x;
//            v1.Traits.Position.y = vt.Position.y;
//            v1.Traits.Position.z = vt.Position.z;
//            eventBlock.newPosition = vt.Position;

//            TriMesh.HalfEdge v1Tov2 = null;
//            TriMesh.HalfEdge v2Tov1 = null;

//            v1Tov2 = v1.FindHalfedgeTo(v2);
//            v2Tov1 = v1Tov2.Opposite;

//            foreach (TriMesh.HalfEdge hf1 in v1.Halfedges)
//            {
//                if (hf1.ToVertex != v2)
//                {
//                    v1.HalfEdge = hf1;
//                    break;
//                }
//            }

//            TriMesh.HalfEdge starthf = v1Tov2.Previous.Opposite;
//            TriMesh.HalfEdge endhf = v2Tov1.Previous.Opposite;
//            eventBlock.v1SharedIndex = starthf.ToVertex.Traits.FixedIndex;
//            eventBlock.v2SharedIndex = endhf.ToVertex.Traits.FixedIndex;

//            List<TriMesh.HalfEdge> v2Neibors = new List<TriMesh.HalfEdge>();
//            //Get v2 neibor
//            foreach (TriMesh.HalfEdge hfitem in v2.Halfedges)
//            {
//                if (hfitem.ToVertex != v1)
//                {
//                    v2Neibors.Add(hfitem);
//                }
//            }

//            foreach (TriMesh.HalfEdge hfitem in v2Neibors)
//            {
//                hfitem.Opposite.ToVertex = v1;
//            }



//            //Deal first  shared face
//            if (!v1Tov2.OnBoundary)
//            {

//                TriMesh.HalfEdge v1HF1 = v1Tov2.Previous.Opposite;
//                TriMesh.HalfEdge v2HF1 = v1Tov2.Next.Opposite;

//                v1HF1.Opposite.Next = v2HF1.Next;
//                v2HF1.Next.Previous = v1HF1.Opposite;
//                v1HF1.Opposite.Previous = v2HF1.Previous;
//                v2HF1.Previous.Next = v1HF1.Opposite;
//                v1HF1.Opposite.ToVertex = v1;
//                v1HF1.Opposite.Face = v2HF1.Face;
//                v1HF1.Opposite.Face.HalfEdge = v1HF1.Opposite;

//                v1HF1.ToVertex.HalfEdge = v1HF1.Opposite;
//                mesh.RemoveHalfedge(v2HF1);
//                v2HF1.Next = null;
//                v2HF1.Previous = null;

//                mesh.RemoveHalfedge(v2HF1.Opposite);
//                v2HF1.Opposite.Opposite = null;
//                v2HF1.Opposite.Next = null;
//                v2HF1.Opposite.Previous = null;
//                v2HF1.Opposite = null;


//                mesh.RemoveHalfedge(v1Tov2);
//                v1Tov2.Next = null;
//                v1Tov2.Previous = null;


//                mesh.RemoveEdge(v2HF1.Edge);
//                //Console.WriteLine("[Remove Edge:" + v2HF1.Edge.Index + "]");

//                v2HF1.Edge.HalfEdge0 = null;

//                mesh.RemoveFace(v1Tov2.Face);
//                v1Tov2.Face = null;

//            }
//            else if (v1Tov2.OnBoundary)
//            {
//                v1Tov2.Previous.Next = v1Tov2.Next;
//                v1Tov2.Next.Previous = v1Tov2.Previous;
//            }


//            //Deal second  shared face
//            if (!v2Tov1.OnBoundary)
//            {
//                TriMesh.HalfEdge v1HF2 = v2Tov1.Next.Opposite;
//                TriMesh.HalfEdge v2HF2 = v2Tov1.Previous.Opposite;

//                v1HF2.Opposite.Next = v2HF2.Next;
//                v2HF2.Next.Previous = v1HF2.Opposite;
//                v1HF2.Opposite.Previous = v2HF2.Previous;
//                v2HF2.Previous.Next = v1HF2.Opposite;
//                v1HF2.ToVertex = v1;
//                v1HF2.Opposite.Face = v2HF2.Face;
//                v1HF2.Opposite.Face.HalfEdge = v1HF2.Opposite;
//                v2HF2.ToVertex.HalfEdge = v1HF2;

//                mesh.RemoveHalfedge(v2HF2);
//                v2HF2.Next = null;
//                v2HF2.Previous = null;

//                mesh.RemoveHalfedge(v2HF2.Opposite);
//                v2HF2.Opposite.Opposite = null;
//                v2HF2.Opposite.Next = null;
//                v2HF2.Opposite.Previous = null;
//                v2HF2.Opposite = null;

//                mesh.RemoveHalfedge(v2Tov1);
//                v2Tov1.Next = null;
//                v2Tov1.Previous = null;

//                v1Tov2.Opposite = null;
//                v2Tov1.Opposite = null;

//                mesh.RemoveEdge(v2HF2.Edge);
//                // Console.WriteLine("[Remove Edge:" + v2HF2.Edge.Index + "]");

//                v2HF2.Edge.HalfEdge0 = null;

//                mesh.RemoveFace(v2Tov1.Face);
//                v2Tov1.Face = null;
//            }
//            else if (v2Tov1.OnBoundary)
//            {
//                v2Tov1.Previous.Next = v2Tov1.Next;
//                v2Tov1.Next.Previous = v2Tov1.Previous;
//            }

//            mesh.RemoveEdge(v2Tov1.Edge);

//            v2Tov1.Edge.HalfEdge0 = null;

//            mesh.RemoveVertex(v2);
//            v2.HalfEdge = null;


//            TriMeshUtil.FixIndex(mesh);
            
//            blockList.Add(eventBlock);

//            return v1;
//        }

       

        
//        public void ControlBlock(int type)
//        {
//            switch (type)
//            {
//                case -1: //Vertex Split Process

//                    if (CurrentEventIndex < 0)
//                    {
//                        CurrentEventIndex = 0;
//                        Console.WriteLine("Warning! There is no futher event for v-s");
//                        break;
//                    }

//                    MergeEventBlock block = EventList[CurrentEventIndex];
//                    TriMesh.Vertex targetVertex = null;
//                    TriMesh.Vertex startVertex = null;
//                    TriMesh.Vertex stopVertex = null;
//                    Vector3D v1Position;
//                    Vector3D v2Position;

//                    //Find Verties
//                    foreach (TriMesh.Vertex item in mesh.Vertices)
//                    {
//                        if (item.Traits.FixedIndex == block.v1Index)
//                            targetVertex = item;
//                        if (item.Traits.FixedIndex == block.v1SharedIndex)
//                            startVertex = item;
//                        if (item.Traits.FixedIndex == block.v2SharedIndex)
//                            stopVertex = item;
//                    }

//                    v1Position = block.v1Position;
//                    v2Position = block.v2Position;

//                    TriMesh.Vertex newOne =TriMeshModify.VertexSplit(targetVertex, startVertex, stopVertex, v1Position, v2Position, block.v2Index);

//                    targetVertex.Traits.SelectedFlag = 1;
//                    newOne.Traits.SelectedFlag = 1;

//                    //CurrentEventIndex--;
//                    break;
//                case 1:
//                    if (CurrentEventIndex > EventList.Count - 1)
//                    {
//                        CurrentEventIndex = EventList.Count - 1;
//                        Console.WriteLine("Warning! There is no futher event for merge");
//                        break;
//                    }

//                    MergeEventBlock block2 = EventList[CurrentEventIndex];
//                    TriMesh.Vertex v1 = null;
//                    TriMesh.Vertex v2 = null;
//                    Vector3D newPosition;

//                    foreach (TriMesh.Vertex item in mesh.Vertices)
//                    {
//                        if (item.Traits.FixedIndex == block2.v1Index)
//                            v1 = item;
//                        if (item.Traits.FixedIndex == block2.v2Index)
//                            v2 = item;
//                    }
//                    newPosition = block2.newPosition;

//                    TriMesh.Edge edge = v1.FindEdgeTo(v2);

//                    if (edge != null && TriMeshModify.IsMergeable(edge))
//                    {
//                        TriMeshModify.MergeEdge(edge, newPosition);
//                    }
//                    else
//                    {
//                        Console.WriteLine("Very wrong situation");
//                    }

//                    v1.Traits.SelectedFlag = 2;

//                    break;
//                default:
//                    break;
//            }
//        }
       


         
      
//    }
//}
