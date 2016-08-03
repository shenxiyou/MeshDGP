


using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
 

namespace GraphicResearchHuiZhao
{
    /// <summary>
    /// A triangular mesh with custom traits for point positions, normals, and curvature.
    /// </summary>
    [Serializable]
    public partial class QuadMesh : HalfEdgeMesh 
    {
 
        /// <summary>
        /// The custom traits for the mesh.
        /// </summary>
        public MeshTraits Traits;

        private string fileName = string.Empty;

        public string ModelName = string.Empty;
 

   
        /// <summary>
        /// Initializes an empty mesh.
        /// </summary>
        public QuadMesh()
        {
            trianglesOnly = false;
        }
       
 
    
        public string FileName
        {
            get { return fileName; }
            internal set
            {
                fileName = value;
                String[] tokens = fileName.Split("\\".ToCharArray());
                String lastToekn = tokens[tokens.Length - 1];
                int dotIndex = lastToekn.IndexOf(lastToekn);
                ModelName = lastToekn.Substring(0, dotIndex);
            }
        }

     

    }
}
