using System;
using System.Collections.Generic;
using System.Text;
 

namespace GraphicResearchHuiZhao
{
   
    /// <summary>
    /// Face traits with per-face vertex texture coordinates and normals.
    /// </summary>
    [Serializable]
    public class  FaceTraits
    {

        private byte selectedFlag;
        /// <summary>
        /// Normal for display.
        /// </summary>
        public Vector3D Normal;

        public Color4 Color ;

        public Vector3D Direction;

        public byte SelectedFlag
        {
            get
            {
                return selectedFlag;
            }
            set
            {
                selectedFlag = value;
            }
        }

        
    }
  
}
