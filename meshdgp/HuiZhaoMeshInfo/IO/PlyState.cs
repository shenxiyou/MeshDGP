using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    
    /// <summary>
    /// Stores state during processing of an OBJ file.
    /// </summary>
    public class PlyFileProcessorState
    {
        public int t = 0;
        public int  countv = 0;
        public int countf = 0;
        public int  counte = 0;
        public int? Vnum = 0;
        public int? Fnum = 0;
        public int? Enum = 0;
        public bool startv = false;
        public bool startf = false;
        public bool starte = false;

        public List<Vector3D> VertexNormals = new List<Vector3D>();
        public List<Vector2D> VertexTextureCoords = new List<Vector2D>();
    }
 

}
