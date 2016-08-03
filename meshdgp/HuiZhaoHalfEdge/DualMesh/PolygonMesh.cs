 

using System;
using System.Collections.Generic; 
using System.Text;


namespace GraphicResearchHuiZhao
{
     
    [Serializable]
    public partial class PolygonMesh : HalfEdgeMesh 
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
        public PolygonMesh()
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
