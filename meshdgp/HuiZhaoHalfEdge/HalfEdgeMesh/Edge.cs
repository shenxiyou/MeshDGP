

using System;
using System.Collections.Generic;

namespace GraphicResearchHuiZhao
{
    public partial class HalfEdgeMesh 
    {
        /// <summary>
        /// An edge of the mesh.
        /// </summary>
        [Serializable]
        public class Edge
        { 
            public EdgeTraits Traits=new EdgeTraits();
            private HalfEdge halfedge;
            private int index;  
            public Edge() { } 
            public Edge(EdgeTraits edgeTraits)
            {
                Traits = edgeTraits;
            } 
            public Face Face0
            {
                get
                {
                    return halfedge.Face;
                }
            } 
            public Face Face1
            {
                get
                {
                    return halfedge.Opposite.Face;
                }
            } 
            public HalfEdge HalfEdge0
            {
                get
                {
                    return halfedge;
                }
                set
                {
                    halfedge = value;
                }
            } 
            public HalfEdge HalfEdge1
            {
                get
                {
                    return halfedge.Opposite;
                }
                set
                {
                    halfedge.Opposite = value;
                }
            }
            public Vertex Vertex0
            {
                get
                {
                    return halfedge.ToVertex;
                }
            }
            public Vertex Vertex1
            {
                get
                {
                    return halfedge.Opposite.ToVertex;
                }
            }
            public bool OnBoundary
            {
                get
                {
                    return halfedge.OnBoundary ||
                           halfedge.Opposite.OnBoundary;
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
          

            /// <summary>
            /// The mesh the edge belongs to.
            /// </summary>
            public HalfEdgeMesh Mesh
            {
                get
                {
                    return halfedge.Mesh;
                }
            }

            
            

        }
    }
}
