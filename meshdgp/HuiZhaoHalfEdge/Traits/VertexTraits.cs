using System;
using System.Collections.Generic;
using System.Text;
 
 

namespace GraphicResearchHuiZhao
{
 
    
    /// <summary>
    /// Vertex traits with position, normal, and principle curvatures.
    /// </summary>
    [Serializable]
    public class VertexTraits
    {
        /// <summary>
        /// Point position.
        /// </summary>
        public Vector3D Position; 
        /// <summary>
        /// Vertex normal for computations.
        /// </summary>
        public Vector3D Normal;

        public Vector2D UV;

        /// <summary>
        /// Distance.
        /// </summary>
        public double Distance;

        //#if CURVATURE
        /// <summary>
        /// Maximum principle curvature direction.
        /// </summary>
        public Vector3D MaxCurvatureDirection;

        /// <summary>
        /// Minimum principle curvature direction.
        /// </summary>
        public Vector3D MinCurvatureDirection;

        /// <summary>
        /// Maximum principle curvature.
        /// </summary>
        public double MaxCurvature;

        /// <summary>
        /// Minimum principle curvature.
        /// </summary>
        public double MinCurvature;
        //#endif


        /// <summary>
        /// Point tangent.
        /// </summary>
        public Vector3D Tangent;

        /// <summary>
        /// Point tangent.
        /// </summary>
        public Vector3D Binormal;

        public double GaussianCurvature;

        public double MeanCurvature;

        public byte selectedFlag;



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

        private  Color4 color  ;

        public Color4 Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        } 

        


        public int FixedIndex;
       

        /// <summary>
        /// Initializes vertex traits with the specified position.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        /// <param name="z">The z-coordinate.</param>
        public VertexTraits(double x, double y, double z)
             
        {
            Position = new Vector3D(x, y, z);

            SelectedFlag = 0;
            
        }

        public VertexTraits(Vector3D position)
            
        {
            Position = position;

            SelectedFlag = 0;

        }
    }
   
}
