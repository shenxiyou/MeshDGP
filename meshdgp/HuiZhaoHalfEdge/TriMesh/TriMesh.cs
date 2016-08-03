
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
    public partial class TriMesh : HalfEdgeMesh 
    {
       

       

        #region Fields
        /// <summary>
        /// The custom traits for the mesh.
        /// </summary>
        public MeshTraits Traits;

        private string fileName = "";
        public string ModelName = "";

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes an empty mesh.
        /// </summary>
        public TriMesh()
        {
            trianglesOnly = true;
            
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the name of the file for this mesh.
        /// </summary>
        /// <remarks>
        /// This is set automatically when a mesh is loaded from a file.
        /// </remarks>
        public string FileName
        {
            get { return fileName; }
            set { 
                fileName = value;
                String[] tokens = fileName.Split("\\".ToCharArray());
                String lastToekn = tokens[tokens.Length - 1];
                int dotIndex = lastToekn.IndexOf(lastToekn);
                ModelName = lastToekn.Substring(0, dotIndex);
            }
        }
        #endregion



        /// Removes all elements from the mesh.
        /// </summary>
        public override void Clear()
        {
            base.Clear();
            Traits = new MeshTraits();
        }


    }
}
