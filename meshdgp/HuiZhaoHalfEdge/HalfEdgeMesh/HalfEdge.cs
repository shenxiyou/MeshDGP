

using System;
using System.Collections.Generic;

namespace GraphicResearchHuiZhao
{
    public partial class HalfEdgeMesh 
    {
        /// <summary>
        /// A halfedge of the mesh.
        /// </summary>
        [Serializable]

        public class HalfEdge
        { 
            public HalfedgeTraits Traits=new HalfedgeTraits();
            private Edge edge;
            private Face face;
            private HalfEdge nextHalfEdge; 
            private HalfEdge oppositeHalfEdge;
            private HalfEdge previousHalfEdge;
            private Vertex vertex;
            private int index; 
            public HalfEdge() { } 
            public HalfEdge(HalfedgeTraits halfedgeTraits)
            {
                Traits = halfedgeTraits;
            }
            public bool OnBoundary
            {
                get
                {
                    return face == null;
                }
            }
            public Vertex FromVertex
            {
                get
                {
                    return Opposite.ToVertex;
                }
            } 


            public Edge Edge
            {
                get
                {
                    return edge;
                }
                set
                {
                    edge = value;
                }
            } 
            public Face Face
            {
                get
                {
                    return face;
                }
                set
                {
                    face = value;
                }
            } 
            public HalfEdge Next
            {
                get
                {
                    return nextHalfEdge;
                }
                 set
                {
                    nextHalfEdge = value;
                }
            } 
           
            public HalfEdge Opposite
            {
                get
                {
                    return oppositeHalfEdge;
                }
                 set
                {
                    oppositeHalfEdge = value;
                }
            } 
            public HalfEdge Previous
            {
                get
                {
                    return previousHalfEdge;
                }
                 set
                {
                    previousHalfEdge = value;
                }
            } 
            public Vertex ToVertex
            {
                get
                {
                    return vertex;
                }
                set
                {
                    vertex = value;
                }
            }
           

            public int Index
            {
                get
                {
                    return index;
                }
                set
                {
                    index = value;
                }
            }
            public HalfEdgeMesh Mesh
            {
                get
                {
                    return vertex.Mesh;
                }
            } 
        }
    }
}
