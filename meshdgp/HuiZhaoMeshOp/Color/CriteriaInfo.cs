using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public class CriteriaInfo
    {
        private double dehidralMin = 0;
        private double dehidralMax = 180;

        private double edgeLengthMin = 0;
        private double edgeLengthMax = 0;

        private double faceAreaMin = 0;
        private double faceAreaMax = 0;

        private double vertexAreaMin = 0;
        private double vertexAreaMax = 0;


        private double gaussinMin = 0;
        private double gaussinMax = 0;

        private double meanCurvatureMin = 100;
        private double meanCurvatureMax = 100;  
 

        public double DehidraAngleMin
        {
            get
            {
                return dehidralMin;
            }
            set
            {
                dehidralMin = value;
            }
        }

        public double DehidraAngleMax
        {
            get
            {
                return dehidralMax;
            }
            set
            {
                dehidralMax = value;
            }
        }

        public double EdgeLengh
        {
            get
            {
                return edgeLengthMin;
            }
            set
            {
                edgeLengthMin = value;
            }
        }

        public double FaceAreaMin
        {
            get
            {
                return faceAreaMin;
            }
            set
            {
                faceAreaMin = value;
            }
        }
        public double FaceAreaMax
        {
            get
            {
                return faceAreaMax;
            }
            set
            {
                faceAreaMax = value;
            }
        }

        public double VertexAreaMax
        {
            get
            {
                return vertexAreaMax;
            }

            set
            {
                vertexAreaMax = value;
            }
        }

        public double VertexAreaMin
        {
            get
            {
                return vertexAreaMin;
            }

            set
            {
                vertexAreaMin = value;
            }
        }


        public double GaussinMax
        {
            get
            {
                return gaussinMax;
            }
            set
            {
                gaussinMax = value;
            }
        }

        public double GaussinMin
        {
            get
            {
                return gaussinMin;
            }
            set
            {
                gaussinMin = value;
            }
        }


        public double MeanCurvatureMax
        {
            get
            {
                return meanCurvatureMax;
            }
            set
            {
                meanCurvatureMax = value;
            }
        }

        public double MeanCurvatureMin
        {
            get
            {
                return meanCurvatureMin;
            }
            set
            {
                meanCurvatureMin = value;
            }
        }


       
    
    }
}
