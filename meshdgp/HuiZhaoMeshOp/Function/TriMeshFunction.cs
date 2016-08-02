using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
  

    public partial class TriMeshFunction
    {

        //Single instance
        private static TriMeshFunction singleton = new TriMeshFunction();

        public static TriMeshFunction Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new TriMeshFunction();
                return singleton;
            }
        }

        public EnumFunction FunctionType = EnumFunction.X;

        public TriMeshFunction()
        {

        }


        public double[] Function = null;

        public double[] ComputeFunction(TriMesh mesh)
        {
            double[] function=null;
            switch (FunctionType)
            {

                case EnumFunction.X:
                    function = ComputeX(mesh);
                     break;

                case EnumFunction.Y :
                     function = ComputeY(mesh);
                     break;

                case EnumFunction.Z :
                     function = ComputeZ(mesh);
                     break;

                case EnumFunction.CosX :

                     function = ComputeCosX(mesh, ConfigFunction.Instance.Cos);
                     break;
                case EnumFunction.CosY:

                     function = ComputeCosY(mesh, ConfigFunction.Instance.Cos);
                     break;
                case EnumFunction.CosZ:

                     function = ComputeCosZ(mesh, ConfigFunction.Instance.Cos);
                     break;

                case EnumFunction.Harmonic:

                     function = ComputeHarmonic(mesh);
                     break;
                case EnumFunction.SmoothedCurvature:

                     function = ComputeSmoothedCurvature(mesh);
                     break;

                case EnumFunction.DiffusionDistance:

                     function = ComputeDistanceDiffusion(mesh, ConfigFunction.Instance.Diffustion);
                     break;

                case EnumFunction.CommuteTime:

                     function = ComputeDistanceCommuteTime(mesh);
                     break;


                case EnumFunction.BiharmonicDistance:

                     function = ComputeDistanceBiharmonic(mesh);
                     break;

                case EnumFunction.ADFDistance:

                     function = ComputeDistanceADF(mesh, ConfigFunction.Instance.ADF);
                     break;
                case EnumFunction.Gaussian:
                     
                     function = TriMeshUtil.ComputeGaussianCurvatureIntegrated(mesh);
                     break;

                case EnumFunction.MeanCurvature:

                     function = TriMeshUtil.ComputeMeanCurvature(mesh);
                     break;

                case EnumFunction.GaussianVertex:
                     CurvatureLib.Init(mesh);
                     function = CurvatureLib.ComputeGaussianCurvature();
                     
                     break;
                case EnumFunction.MeanCurvatureVertex:

                     CurvatureLib.Init(mesh);
                     function = CurvatureLib.ComputeMeanCurvature();
                     break;
                case EnumFunction.PrincipalMaxAbs:

                     CurvatureLib.Init(mesh);
                     function = CurvatureLib.ComputePrincipalMaxAbs();
                     break;
                case EnumFunction.PrincipalMinAbs:

                     CurvatureLib.Init(mesh);
                     function = CurvatureLib.ComputePrincipalMinAbs();
                     break;

                case EnumFunction.PrincipalMax:

                     CurvatureLib.Init(mesh);
                     function = CurvatureLib.ComputePrincipalMax();
                     break;
                case EnumFunction.PrincipalMin:

                     CurvatureLib.Init(mesh);
                     function = CurvatureLib.ComputePrincipalMin();
                     break;

                case EnumFunction.EigenVector:

                     function = ComputeEigenVector(mesh, ConfigFunction.Instance.EigenIndex);
                     break;

                case EnumFunction.External:

                     function = Function;
                     break;

            }

            this.Function = function;

            return function;
        }

        public double[] ComputeCosX(TriMesh mesh,double cos)
        {
            double[] function = new double[mesh.Vertices.Count];

            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                function[i] = Math.Cos(mesh.Vertices[i].Traits.Position.x*cos);
            }

            return function;
        }

        public double[] ComputeCosY(TriMesh mesh, double cos)
        {
            double[] function = new double[mesh.Vertices.Count];

            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                function[i] = Math.Cos(mesh.Vertices[i].Traits.Position.y * cos);
            }

            return function;
        }

        public double[] ComputeCosZ(TriMesh mesh, double cos)
        {
            double[] function = new double[mesh.Vertices.Count];

            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                function[i] = Math.Cos(mesh.Vertices[i].Traits.Position.z * cos);
            }

            return function;
        }
        

        public double[] ComputeY(TriMesh mesh)
        {
            return TriMeshUtil.GetY(mesh).ToArray();
        }

        public double[] ComputeX(TriMesh mesh)
        {
            return TriMeshUtil.GetX(mesh).ToArray();
        }


        public double[] ComputeZ(TriMesh mesh)
        {
            return TriMeshUtil.GetZ(mesh).ToArray();
        }


        public double[] ComputeHarmonic(TriMesh mesh)
        {
            FunctionHarmonic har=new FunctionHarmonic();
            har.Minimum = ConfigFunction.Instance.HarmonicMin;
            har.Maximum = ConfigFunction.Instance.HarmonicMax;

            double[] function = har.ComputeHarmonicFunction(mesh);

            return function;
        }

        public double[] ComputeSmoothedCurvature(TriMesh mesh)
        {
            FunctionSmoothedCurvature har = new FunctionSmoothedCurvature();
            
            double[] function = har.ComputeFunction(mesh);

            return function;
        }

    }
}
