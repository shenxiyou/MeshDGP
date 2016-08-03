

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GraphicResearchHuiZhao
{
    public partial class HalfEdgeMesh 
    {
         

        #region HalfedgeCollection class
        /// <summary>
        /// Type allowing halfedges to be accessed like an array.
        /// </summary>
        [Serializable]
        public class HalfedgeCollection : IEnumerable<HalfEdge>
        {
            #region Fields
            readonly HalfEdgeMesh  mesh;
            #endregion

            #region Constructors
            internal HalfedgeCollection(HalfEdgeMesh  m)
            {
                mesh = m;
            }
            #endregion

            #region Properties
            /// <summary>
            /// Accesses the halfedges in a mesh.
            /// </summary>
            /// <param name="index">The index of the halfedge.</param>
            /// <returns>The indexed <see cref="Halfedge"/>.</returns>
            public HalfEdge this[int index]
            {
                get
                {
                    return mesh.halfEdges[index];
                }
            }

            /// <summary>
            /// The number of halfedges in the mesh.
            /// </summary>
            public int Count
            {
                get
                {
                    return mesh.halfEdges.Count;
                }
            }
            #endregion

            #region Iterators
            /// <summary>
            /// Provides an enumerator for the halfedges of the mesh.
            /// </summary>
            /// <returns>A halfedge of the mesh.</returns>
            public IEnumerator<HalfEdge> GetEnumerator()
            {
                foreach (HalfEdge h in mesh.halfEdges)
                {
                    yield return h;
                }
            }

            /// <summary>
            /// Useless IEnumerable.GetEnumerator() implementation.
            /// </summary>
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }


            //public void Remove(HalfEdge halfedge)
            //{
            //    mesh.RemoveHalfedge(halfedge);
            //}
            #endregion
        }
        #endregion

       
    }
}