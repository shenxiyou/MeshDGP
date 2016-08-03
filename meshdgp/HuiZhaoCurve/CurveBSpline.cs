using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public partial class ParameterCurve
    {

        public int  Point = 6;
        public  TriMesh CreateNBSpineControlPoint()
        {
            TriMesh BezierControlPoint = new TriMesh();
            Random r = new Random();
            BezierControlPoint.Vertices.Add(new VertexTraits(0, 0, 0));
            for (int t = 0; t < Point; t++)
            {
                int x = r.Next(1, 10);
                BezierControlPoint.Vertices.Add(new VertexTraits(t * 0.1 + x * 0.01, x * 0.1, 0));
            }
           
            return BezierControlPoint;
        }

        public TriMesh CreateFourPointBSplineCurve(TriMesh mesh)
        {
            TriMesh FourPointBSplineCurve = new TriMesh();
            double x = 0;
            double y = 0;
            double z = 0;
            for (int t = 0; t <= VerticesNum; t++)
            {
                double tt = (double)t / (double)VerticesNum;
                x = (double)1 / 6 * ((-Math.Pow(tt, 3) + 3 * Math.Pow(tt, 2) - 3 * tt + 1)) * mesh.Vertices[0].Traits.Position.x
                    + (double)1 / 6 * ((3 * Math.Pow(tt, 3) - 6 * Math.Pow(tt, 2) + 4)) * mesh.Vertices[1].Traits.Position.x
                    + (double)1 / 6 * ((-3 * Math.Pow(tt, 3) + 3 * Math.Pow(tt, 2) + 3 * tt + 1)) * mesh.Vertices[2].Traits.Position.x
                    + (double)1 / 6 * Math.Pow(tt, 3) * mesh.Vertices[3].Traits.Position.x;
                y = (double)1 / 6 * ((-Math.Pow(tt, 3) + 3 * Math.Pow(tt, 2) - 3 * tt + 1)) * mesh.Vertices[0].Traits.Position.y
                    + (double)1 / 6 * ((3 * Math.Pow(tt, 3) - 6 * Math.Pow(tt, 2) + 4)) * mesh.Vertices[1].Traits.Position.y
                    + (double)1 / 6 * ((-3 * Math.Pow(tt, 3) + 3 * Math.Pow(tt, 2) + 3 * tt + 1)) * mesh.Vertices[2].Traits.Position.y
                    + (double)1 / 6 * Math.Pow(tt, 3) * mesh.Vertices[3].Traits.Position.y;
                FourPointBSplineCurve.Vertices.Add(new VertexTraits(x, y, z));
            }
            return FourPointBSplineCurve;
        }

        public TriMesh CreateFourPointBSpline()
        {
            TriMesh BezierControlPoint = new TriMesh();

            BezierControlPoint.Vertices.Add(new VertexTraits(0, 0, 0));
            BezierControlPoint.Vertices.Add(new VertexTraits(0.2, 0.2, 0));
            BezierControlPoint.Vertices.Add(new VertexTraits(0.4, 0.5, 0));
            BezierControlPoint.Vertices.Add(new VertexTraits(0.6, 0.4, 0));
            return BezierControlPoint;
        }

        public  TriMesh CreateNBsplineCurve(TriMesh mesh)
        {
            TriMesh BezierCurve = new TriMesh();
            double x = 0;
            double y = 0;
            double z = 0;

            for (int i = 0; i < Point - 2; i++)
            {
                for (int t = 0; t <= VerticesNum; t++)
                {
                    double tt = (double)t / (double)VerticesNum;
                    //x = (double)1 / 6 * ((-Math.Pow(tt, 3) + 3 * Math.Pow(tt, 2) - 3 * tt + 1)) * mesh.Vertices[0].Traits.Position.x
                    //    + (double)1 / 6 * ((3 * Math.Pow(tt, 3) - 6 * Math.Pow(tt, 2) + 4)) * mesh.Vertices[1].Traits.Position.x
                    //    + (double)1 / 6 * ((-3 * Math.Pow(tt, 3) + 3 * Math.Pow(tt, 2) + 3 * tt + 1)) * mesh.Vertices[2].Traits.Position.x
                    //    + (double)1 / 6 * Math.Pow(tt, 3) * mesh.Vertices[3].Traits.Position.x;
                    //y = (double)1 / 6 * ((-Math.Pow(tt, 3) + 3 * Math.Pow(tt, 2) - 3 * tt + 1)) * mesh.Vertices[0].Traits.Position.y
                    //    + (double)1 / 6 * ((3 * Math.Pow(tt, 3) - 6 * Math.Pow(tt, 2) + 4)) * mesh.Vertices[1].Traits.Position.y
                    //    + (double)1 / 6 * ((-3 * Math.Pow(tt, 3) + 3 * Math.Pow(tt, 2) + 3 * tt + 1)) * mesh.Vertices[2].Traits.Position.y
                    //    + (double)1 / 6 * Math.Pow(tt, 3) * mesh.Vertices[3].Traits.Position.y;
                    x = (double)1 / 6 * ((-Math.Pow(tt, 3) + 3 * Math.Pow(tt, 2) - 3 * tt + 1)) * mesh.Vertices[i].Traits.Position.x
                        + (double)1 / 6 * ((3 * Math.Pow(tt, 3) - 6 * Math.Pow(tt, 2) + 4)) * mesh.Vertices[i + 1].Traits.Position.x
                        + (double)1 / 6 * ((-3 * Math.Pow(tt, 3) + 3 * Math.Pow(tt, 2) + 3 * tt + 1)) * mesh.Vertices[i + 2].Traits.Position.x
                        + (double)1 / 6 * Math.Pow(tt, 3) * mesh.Vertices[i + 3].Traits.Position.x;
                    y = (double)1 / 6 * ((-Math.Pow(tt, 3) + 3 * Math.Pow(tt, 2) - 3 * tt + 1)) * mesh.Vertices[i].Traits.Position.y
                       + (double)1 / 6 * ((3 * Math.Pow(tt, 3) - 6 * Math.Pow(tt, 2) + 4)) * mesh.Vertices[i + 1].Traits.Position.y
                       + (double)1 / 6 * ((-3 * Math.Pow(tt, 3) + 3 * Math.Pow(tt, 2) + 3 * tt + 1)) * mesh.Vertices[i + 2].Traits.Position.y
                       + (double)1 / 6 * Math.Pow(tt, 3) * mesh.Vertices[i + 3].Traits.Position.y;
                    BezierCurve.Vertices.Add(new VertexTraits(x, y, z));
                    x = 0;
                    y = 0;
                }
            }
            return BezierCurve;
        }

        

    }
    
}
