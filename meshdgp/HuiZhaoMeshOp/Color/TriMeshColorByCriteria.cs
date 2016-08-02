using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public class TriMeshColorByCriteria
    {
        

        //Single instance
        private static TriMeshColorByCriteria singleton = new TriMeshColorByCriteria();

        public static TriMeshColorByCriteria Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new TriMeshColorByCriteria();
                return singleton;
            }
        }

       
         

        public void ColorVertex(TriMesh mesh, double[] vertexValue, double min,double max)
        {
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                if (vertexValue[i] > min && vertexValue[i]   < max)
                {
                    mesh.Vertices[i].Traits.SelectedFlag = 1;
                }
            }
        }

        public void ColorEdge(TriMesh mesh, double[] edgeValue, double min, double max)
        {
            for (int i = 0; i < mesh.Edges.Count; i++)
            {
                if (edgeValue[i]   > min && edgeValue[i]   < max)
                {
                    mesh.Edges[i].Traits.SelectedFlag = 1;
                }
            }
        }

        public void ColorFace(TriMesh mesh, double[] faceValue, double min, double max)
        {
            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                if (faceValue[i] > min && faceValue[i] < max)
                {
                    mesh.Faces[i].Traits.SelectedFlag = 1;
                }
            }
        }

        public double[] ColorVertexByGaussin(TriMesh mesh, double min, double max)
        {
            double[] gaussin = TriMeshUtil.ComputeGaussianCurvatureIntegrated(mesh);

            ColorVertex(mesh, gaussin, min, max);

            return gaussin;
        } 

        public double[] ColorVertexByMeanCurvature(TriMesh mesh, double min, double max)
        {
            double[] mean = TriMeshUtil.ComputeMeanCurvature(mesh);

            ColorVertex(mesh, mean, min, max);

            return mean;
        }



        


        public double[] ColorEdgeByDihedralAngle(TriMesh mesh, double dehidraAngleMin, double dehidraAngleMax)
        {
            double[] angles = TriMeshUtil.ComputeDihedralAngle(mesh);

            for (int i = 0; i < angles.Length; i++)
            {
                angles[i] *= (360 / Math.PI);
                
            }

            ColorEdge(mesh, angles, dehidraAngleMin, dehidraAngleMax);

            return angles;
        }

       



      

      

        public double[] SetColor(string item, TriMesh mesh, CriteriaInfo criteriaInfo)
        {
            TriMeshUtil.ClearMeshColor(mesh);

            double[] value = null;
            
            if (item == "DehidraAngleMin")
            {
                value= ColorEdgeByDihedralAngle(mesh, criteriaInfo.DehidraAngleMin,criteriaInfo.DehidraAngleMax);
            }

            if (item == "DehidraAngleMax")
            {
                value = ColorEdgeByDihedralAngle(mesh, criteriaInfo.DehidraAngleMin, criteriaInfo.DehidraAngleMax);
            }
           

            if (item == "GaussinMin")
            {
                value = ColorVertexByGaussin(mesh, criteriaInfo.GaussinMin, criteriaInfo.GaussinMax);

            }

            if (item == "GaussinMax")
            {
                value = ColorVertexByGaussin(mesh, criteriaInfo.GaussinMin, criteriaInfo.GaussinMax);

            }

            if (item == "MeanCurvatureMax")
            {
                value = ColorVertexByMeanCurvature(mesh, criteriaInfo.MeanCurvatureMin, criteriaInfo.MeanCurvatureMax);

            }

            if (item == "MeanCurvatureMin")
            {
                value = ColorVertexByMeanCurvature(mesh, criteriaInfo.MeanCurvatureMin, criteriaInfo.MeanCurvatureMax);

            }

            return value;
            
        }


        public void SetRange(TriMesh  mesh,EnumColorItem item, ref CriteriaRange criteriaRange)
        {
            double[] range=null;
            switch (item)
            {
                case EnumColorItem.DihedralAngle :
                    range = TriMeshUtil.ComputeDihedralAngle(mesh);                     
                    break;

                case EnumColorItem.Gaussian:
                    range = TriMeshUtil.ComputeGaussianCurvatureIntegrated(mesh);
                    break;
                case EnumColorItem.Mean:
                    range = TriMeshUtil.ComputeMeanCurvature(mesh);
                    break;
            }

            criteriaRange.Max =TriMeshFunction.Instance.ComputeMax(range);
            criteriaRange.Min = TriMeshFunction.Instance.ComputeMin(range);

             
        }
        

    }
}
