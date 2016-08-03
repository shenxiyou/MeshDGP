using System;
using System.Collections.Generic;
using System.Text;
 

namespace GraphicResearchHuiZhao
{
    
    /// <summary>
    /// Edge traits with per-face vertex texture coordinates and normals.
    /// </summary>
    [Serializable]
    public class  EdgeTraits
    {
        
        public byte selectedFlag;
        /// <summary>
        /// Normal for display.
        /// </summary>
        public double Angle;

        public Color4 Color  ;

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
