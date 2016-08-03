using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public partial class ParameterCurve
    {

        public EnumCurveComplex currentCurve = EnumCurveComplex.FourPointBezier;

        public TriMesh CreateFourBezierControlPoint()
        {
            TriMesh controlPoint = new TriMesh();
            
            controlPoint.Vertices.Add(new VertexTraits(0, 0, 0));
            controlPoint.Vertices.Add(new VertexTraits(0.2, 0.2, 0));
            controlPoint.Vertices.Add(new VertexTraits(0.4, 0.5, 0));
            controlPoint.Vertices.Add(new VertexTraits(0.6, 0.4, 0));
            return controlPoint;
        }

        public  TriMesh CreateFourBezierCurve(TriMesh mesh)
        {
            TriMesh curve = new TriMesh();
            double x = 0;
            double y = 0;
            double z = 0;
            for (int t = 0; t < VerticesNum; t++)
            {
                double tt = (double)t / (double)VerticesNum;
                x = mesh.Vertices[0].Traits.Position.x * Math.Pow(1 - tt, 3)
                    + 3 * mesh.Vertices[1].Traits.Position.x * tt * Math.Pow(1 - tt, 2)
                    + 3 * mesh.Vertices[2].Traits.Position.x * Math.Pow(tt, 2) * (1 - tt)
                    + mesh.Vertices[3].Traits.Position.x * Math.Pow(tt, 3);
                y = mesh.Vertices[0].Traits.Position.y * Math.Pow(1 - tt, 3)
                    + 3 * mesh.Vertices[1].Traits.Position.y * tt * Math.Pow(1 - tt, 2)
                    + 3 * mesh.Vertices[2].Traits.Position.y * Math.Pow(tt, 2) * (1 - tt)
                    + mesh.Vertices[3].Traits.Position.y * Math.Pow(tt, 3);
                z = mesh.Vertices[0].Traits.Position.z * Math.Pow(1 - tt, 3)
                    + 3 * mesh.Vertices[1].Traits.Position.z * tt * Math.Pow(1 - tt, 2)
                    + 3 * mesh.Vertices[2].Traits.Position.z * Math.Pow(tt, 2) * (1 - tt)
                    + mesh.Vertices[3].Traits.Position.z * Math.Pow(tt, 3);
                curve.Vertices.Add(new VertexTraits(x, y, z));
            }
            return curve;
        }

        public TriMesh CreateThreeBezierControlPoint()
        {
            TriMesh controlPoint = new TriMesh();

            controlPoint.Vertices.Add(new VertexTraits(0, 0, 0));
            controlPoint.Vertices.Add(new VertexTraits(0.2, 0.2, 0));
            controlPoint.Vertices.Add(new VertexTraits(0.4, 0.5, 0));
            return controlPoint;
        }

        public TriMesh CreateThreeBezierCurve(TriMesh mesh)
        {
            TriMesh curve = new TriMesh();
            double x = 0;
            double y = 0;
            double z = 0;
            for (int t = 0; t < VerticesNum; t++)
            {
                double tt = (double)t / (double)VerticesNum;
                x = mesh.Vertices[0].Traits.Position.x * Math.Pow(1 - tt, 2)
                    + 2 * mesh.Vertices[1].Traits.Position.x * tt * (1 - tt)
                    + mesh.Vertices[2].Traits.Position.x * Math.Pow(tt, 2);
                y = mesh.Vertices[0].Traits.Position.y * Math.Pow(1 - tt, 2)
                   + 2 * mesh.Vertices[1].Traits.Position.x * tt * (1 - tt)
                   + mesh.Vertices[2].Traits.Position.y * Math.Pow(tt, 2);
                z = mesh.Vertices[0].Traits.Position.z * Math.Pow(1 - tt, 2)
                   + 2 * mesh.Vertices[1].Traits.Position.z * tt * (1 - tt)
                   + mesh.Vertices[2].Traits.Position.z * Math.Pow(tt, 2);
                curve.Vertices.Add(new VertexTraits(x, y, z));
            }
            return curve;
        }

        public int controlPoint = 6;
        public  TriMesh CreateNBezierControlPoint()
        {
            TriMesh BezierControlPoint = new TriMesh();
            Random r = new Random();
            BezierControlPoint.Vertices.Add(new VertexTraits(0, 0, 0));
            for (int t = 0; t < controlPoint; t++)
            {
                int x = r.Next(1, 10);
                BezierControlPoint.Vertices.Add(new VertexTraits(t * 0.1 + x * 0.01, x * 0.1, 0));
            }
            return BezierControlPoint;
        }
        public  TriMesh CreateNBezierCurve(TriMesh mesh)
        {
            TriMesh NBezierCurve = new TriMesh();
            double x = 0;
            double y = 0;
            double z = 0;
            for (int t = 0; t < VerticesNum; t++)
            {
                double tt = (double)t / (double)VerticesNum;
                for (int i = 0; i <= controlPoint; i++)
                {
                    x = mesh.Vertices[i].Traits.Position.x * XX(controlPoint, i) * Math.Pow(1 - tt, controlPoint - i) * Math.Pow(tt, i) + x;
                    y = mesh.Vertices[i].Traits.Position.y * XX(controlPoint, i) * Math.Pow(1 - tt, controlPoint - i) * Math.Pow(tt, i) + y;
                    z = mesh.Vertices[i].Traits.Position.z * XX(controlPoint, i) * Math.Pow(1 - tt, controlPoint - i) * Math.Pow(tt, i) + z;
                }
                NBezierCurve.Vertices.Add(new VertexTraits(x, y, z));
                x = 0;
                y = 0;
                z = 0;
            }

            return NBezierCurve;
        }
        private  int XX(int n, int a)
        {
            int sum = 0;
            int temp1 = n, temp2 = 1;
            if (a == 0 || a == n)
            {
                return 1;
            }

            for (int i = 1; i < a; i++)
            {
                temp1 = temp1 * (--n);
            }
            for (int i = 1; i <= a; i++)
            {
                temp2 = temp2 * i;
            }
            sum = temp1 / temp2;
            return sum;
        }

    }
}
