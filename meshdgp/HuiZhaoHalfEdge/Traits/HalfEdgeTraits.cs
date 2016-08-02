using System;
using System.Collections.Generic;
using System.Text;
 

namespace GraphicResearchHuiZhao
{
     
    /// <summary>
    /// Halfedge traits with per-face vertex texture coordinates and normals.
    /// </summary>
    [Serializable]
    public class HalfedgeTraits
    {
        /// <summary>
        /// U,V Texture coordinate.
        /// </summary>
        public Vector2D TextureCoordinate;

        /// <summary>
        /// Normal for display.
        /// </summary>
        public Vector3D Normal;

        public string Reserve;
    }
   
}
