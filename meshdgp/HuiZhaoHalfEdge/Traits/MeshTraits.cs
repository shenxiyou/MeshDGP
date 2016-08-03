using System;
using System.Collections.Generic;
 

namespace GraphicResearchHuiZhao
{
    
    /// <summary>
    /// Mesh traits with bounding sphere and bounding box.
    /// </summary>
    [Serializable]
    public struct MeshTraits
    {
        /// <summary>
        /// Bounding sphere.
        /// </summary>
        public Sphere BoundingSphere;

        /// <summary>
        /// Bounding box.
        /// </summary>
        public AxisAlignedBox BoundingBox;

        /// <summary>
        /// Indicates whether the mesh loaded has face vertex normals.
        /// </summary>
        public bool HasFaceVertexNormals;

        /// <summary>
        /// Indicates whether the mesh loaded has texture coordinates.
        /// </summary>
        public bool HasTextureCoordinates;
    }
    
}
