using System;
using System.Collections.Generic;
using System.Text;
 

namespace GraphicResearchHuiZhao
{
    public partial class TriMeshVertexColorRender
    {
        //Single instance
        private static TriMeshVertexColorRender singleton = new TriMeshVertexColorRender();

        public static TriMeshVertexColorRender Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new TriMeshVertexColorRender();
                return singleton;
            }
        }

        private TriMeshVertexColorRender()
        {

        } 
       
        private int eigenCount = 10;

        public int EigenCount
        {
            get
            {
                return eigenCount;
            }
            set
            {
                eigenCount = value;

            }
        }

        public int eigenIndex = 0;
        public int EigenIndex
        {
            get
            {
                return eigenIndex;
            }
            set
            {

                eigenIndex = value;
            }
        }

        public Eigen eigen = null;


        public double[] ColorEigenVector(TriMesh mesh, int index)
        {
            if (eigen == null)
            {
                eigen = EigenManager.Instance.ComputeEigen(mesh, eigenCount);
            }

            if (eigenIndex > eigenCount)
            {
                eigen = EigenManager.Instance.ComputeEigen(mesh, eigenCount);
            }
            return eigen.GetEigenVector(eigenIndex).ToArray();
        }

        public double[] SetColorEigen(TriMesh mesh)
        {
           double[] values= ColorEigenVector(mesh, eigenIndex);
           ColorizeVertex(mesh, values);

           return values;
        }

      

        public void ColorizeVertex(TriMesh Mesh, double[] values)
        {
            Color4[] result = ColorBalance.Instance.ComputeColor(values); 
            for (int i = 0; i < values.Length; i++)
            {
                Mesh.Vertices[i].Traits.Color = result[i]; 
            }

        }





        public double[] SetBiharmanicDistanceColor(TriMesh Mesh )
        {

            double[] values = TriMeshFunction.ComputeDistanceBiharmonic(Mesh);
             
            return values;
        }





        public double[] SetDiffusionDistanceColor(TriMesh Mesh )
        {

            double[] values = TriMeshFunction.ComputeDistanceDiffusion(Mesh,1.5);



            return values;

            

        }

        //public void SetCurvatureColor(TriMesh Mesh)
        //{
        //    //float cscale=0.8;
        //    //CurvatureLib.Init(Mesh);
        //    //PrincipalCurvature[] curv = CurvatureLib.ComputeCurvature();
        //    //Vector4D[] dcurv = CurvatureLib.ComputeCurD(); 

        //    //for (int i = 0; i < Mesh.Vertices.Count; i++) 
        //    //{
        //    //    float H = 0.5f * (curv[i].max+curv[i].min);
        //    //    float K = curv[i].max*curv[i].min;
        //    //    float h = 4.0f / 3.0f *Math.Abs(atan2(H*H-K,H*H*Math.Sign(H)));
        //    //    float s =Math.PI*2 *Math.Atan((2.0f*H*H-K)*cscale);
        //    //    Mesh.Vertices[i].Traits.Color =new Color4(h,s,1.0,0.0);
        //    }
        //}

        //public void SetCurvatureGray(TriMesh Mesh)
        //{
        //    float cscale=0.8;
        //    CurvatureLib.Init(Mesh);
        //    PrincipalCurvature[] curv = CurvatureLib.ComputeCurvature();
        //    Vector4D[] dcurv = CurvatureLib.ComputeCurD(); 

        //    for (int i = 0; i < Mesh.Vertices.Count; i++) 
        //    {
        //       float H = 0.5f * (curv[i].max+curv[i].min);
        //        float c = (Math.Atan(H*cscale) + Math.PI*2) /Math.PI;
        //        c = Math.Sqrt(c);
        //        int C = int(Math.Min(Math.Max(256.0 * c, 0.0), 255.99));
        //        Mesh.Vertices[i].Traits.Color=new Color4(C,C,C,C);
        //    }
        //}



        //public double[] SetColor(EnumFunction vertexItem,TriMesh Mesh)
        //{
        //    double[] value = null;
        //    switch (vertexItem)
        //    {
        //        case EnumFunction.MeanCurvature:
        //            value = TriMeshUtil.ComputeMeanCurvature(Mesh);
                     
        //            break;
        //        case EnumFunction.Gaussian:
        //            value = TriMeshUtil.ComputeGaussianCurvatureIntegrated(Mesh); 
                     
        //            break;
        //        case EnumFunction.PrincipalMax:
        //           // value = TriMeshUtil.ComputePricipalCurvature(Mesh) ;
                    
        //            break;
        //        case EnumFunction.PrincipalMin:
        //           // value = TriMeshUtil.ComputePricipalCurvature(Mesh)[1];
                   
        //            break;
        //        case EnumFunction.BiharmonicDistance:
        //            value = SetBiharmanicDistanceColor(Mesh);

        //            break;
        //        case EnumFunction.DiffusionDistance:
        //            value = SetDiffusionDistanceColor(Mesh);

        //            break;

              

        //        case EnumFunction.EigenVector:
        //            value = SetColorEigen(Mesh);
        //            break;


        //    }

        //    ColorizeVertex(Mesh, value);
        //    return value;
        //}
        
    }
}
