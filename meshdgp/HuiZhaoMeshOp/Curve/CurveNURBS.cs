using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public partial class ParameterCurve
    {
        
 
        private int circleAll = 8;
        private int ellipseAll = 8;
        public TriMesh CreateNURBSControlPoint()
        {
            TriMesh controlPoint = new TriMesh();

            #region 随机线段
            Random r = new Random();
            controlPoint.Vertices.Add(new VertexTraits(0, 0, 0));
            for (int t = 0; t < ConfigCurve.Instance.ControlPointNum; t++)
            {
                int x = r.Next(1, 10);
                controlPoint.Vertices.Add(new VertexTraits(t * 0.1 + x * 0.01, x * 0.1, 0));
            }
            #endregion

            #region 半圆
            //controlPoint.Vertices.Add(new VertexTraits(0.1, 0, 0));
            //controlPoint.Vertices.Add(new VertexTraits(0.1, 0.1, 0));
            //controlPoint.Vertices.Add(new VertexTraits(0, 0.1, 0));
            //controlPoint.Vertices.Add(new VertexTraits(-0.1, 0.1, 0));
            //controlPoint.Vertices.Add(new VertexTraits(-0.1, 0, 0));
            #endregion

            #region 圆1
            //controlPoint.Vertices.Add(new VertexTraits(1, 0, 0));
            //controlPoint.Vertices.Add(new VertexTraits(1, 1, 0));
            //controlPoint.Vertices.Add(new VertexTraits(0, 1, 0));
            //controlPoint.Vertices.Add(new VertexTraits(-1, 1, 0));
            //controlPoint.Vertices.Add(new VertexTraits(-1, 0, 0));
            //controlPoint.Vertices.Add(new VertexTraits(-1, -1, 0));
            //controlPoint.Vertices.Add(new VertexTraits(0, -1, 0));
            //controlPoint.Vertices.Add(new VertexTraits(1, -1, 0));
            //controlPoint.Vertices.Add(new VertexTraits(1, 0, 0));
            #endregion

            #region 圆2
            //controlPoint.Vertices.Add(new VertexTraits(0, 0, 0));
            //controlPoint.Vertices.Add(new VertexTraits(-1, 0, 0));
            //controlPoint.Vertices.Add(new VertexTraits(-0.5, 0.865, 0));
            //controlPoint.Vertices.Add(new VertexTraits(0, 1.73, 0));
            //controlPoint.Vertices.Add(new VertexTraits(0.5, 0.865, 0));
            //controlPoint.Vertices.Add(new VertexTraits(1, 0, 0));
            //controlPoint.Vertices.Add(new VertexTraits(0, 0, 0));
            #endregion

            return controlPoint;
        }

        public TriMesh CreateNURBS(TriMesh mesh)
        {
            TriMesh nurbs = new TriMesh();
            double x = 0;
            double y = 0;
            double z = 0;
            double m1 = 0, m2 = 0;
            double h = 0;
            double[] w = { 0.9, 0.3 };//权值
            double[] ut = new double[4000];
            for (int i = 0; i < ConfigCurve.Instance.CurveNUM; i++)
            {
                if (i >= 3999)
                    break;
                ut[i] = (double)((double)i / (double)2000) * 7;
            }
            for (int t = 0; t < ConfigCurve.Instance.CurveNUM; t++)
            {
                double u = (double)t / (double)2000;
                for (int i = 0; i < ConfigCurve.Instance.ControlPointNum + 1; i++)
                {
                    m1 = m1 + mesh.Vertices[i].Traits.Position.x * CN(i, 3, u, ut) * w[i % 2];
                    m2 = m2 + mesh.Vertices[i].Traits.Position.y * CN(i, 3, u, ut) * w[i % 2];
                }
                for (int i = 0; i < ConfigCurve.Instance.ControlPointNum + 1; i++)
                {
                    h = h + CN(i, 3, u, ut) * w[i % 2];
                }
                x = m1 / h;
                y = m2 / h;
                nurbs.Vertices.Add(new VertexTraits(x, y, z));
                m1 = 0; m2 = 0; h = 0;
            }
            return nurbs;
        }

        public TriMesh CreateNURBSCicleControlPoint()
        {
            TriMesh controlPoint = new TriMesh();

            #region 半圆
            //controlPoint.Vertices.Add(new VertexTraits(0.1, 0, 0));
            //controlPoint.Vertices.Add(new VertexTraits(0.1, 0.1, 0));
            //controlPoint.Vertices.Add(new VertexTraits(0, 0.1, 0));
            //controlPoint.Vertices.Add(new VertexTraits(-0.1, 0.1, 0));
            //controlPoint.Vertices.Add(new VertexTraits(-0.1, 0, 0));
            #endregion

            #region 圆1
            controlPoint.Vertices.Add(new VertexTraits(0.1, 0, 0));
            controlPoint.Vertices.Add(new VertexTraits(0.1, 0.1, 0));
            controlPoint.Vertices.Add(new VertexTraits(0, 0.1, 0));
            controlPoint.Vertices.Add(new VertexTraits(-0.1, 0.1, 0));
            controlPoint.Vertices.Add(new VertexTraits(-0.1, 0, 0));
            controlPoint.Vertices.Add(new VertexTraits(-0.1, -0.1, 0));
            controlPoint.Vertices.Add(new VertexTraits(0, -0.1, 0));
            controlPoint.Vertices.Add(new VertexTraits(0.1, -0.1, 0));
            controlPoint.Vertices.Add(new VertexTraits(0.1, 0, 0));
            #endregion

            #region 圆2
            //controlPoint.Vertices.Add(new VertexTraits(0, 0, 0));
            //controlPoint.Vertices.Add(new VertexTraits(-1, 0, 0));
            //controlPoint.Vertices.Add(new VertexTraits(-0.5, 0.865, 0));
            //controlPoint.Vertices.Add(new VertexTraits(0, 1.73, 0));
            //controlPoint.Vertices.Add(new VertexTraits(0.5, 0.865, 0));
            //controlPoint.Vertices.Add(new VertexTraits(1, 0, 0));
            //controlPoint.Vertices.Add(new VertexTraits(0, 0, 0));
            #endregion

            return controlPoint;
        }

        public TriMesh CreateNURBSCicle(TriMesh mesh)
        {
            TriMesh nurbs = new TriMesh();
            double x = 0;
            double y = 0;
            double z = 0;
            double m1 = 0, m2 = 0;
            double h = 0;
            double[] w = { 0.9, 0.3 };//权值
            double[] ut = new double[4000];
            for (int i = 0; i < ConfigCurve.Instance.CurveNUM; i++)
            {
                if (i >= 3999)
                    break;
                ut[i] = (double)((double)i / (double)2000) * 7;
            }
            for (int t = 0; t < ConfigCurve.Instance.CurveNUM; t++)
            {
                double u = (double)t / (double)2000;
                for (int i = 0; i < circleAll + 1; i++)
                {
                    m1 = m1 + mesh.Vertices[i].Traits.Position.x * CN(i, 3, u, ut) * w[i % 2];
                    m2 = m2 + mesh.Vertices[i].Traits.Position.y * CN(i, 3, u, ut) * w[i % 2];
                }
                for (int i = 0; i < circleAll + 1; i++)
                {
                    h = h + CN(i, 3, u, ut) * w[i % 2];
                }
                x = m1 / h;
                y = m2 / h;
                nurbs.Vertices.Add(new VertexTraits(x, y, z));
                m1 = 0; m2 = 0; h = 0;
            }
            return nurbs;
        }

        public TriMesh CreateNURBSEllipseControlPoint()
        {
            TriMesh controlPoint = new TriMesh();
            #region 椭圆
            controlPoint.Vertices.Add(new VertexTraits(0.1, 0, 0));
            controlPoint.Vertices.Add(new VertexTraits(0.1, 0.1, 0)); 
            controlPoint.Vertices.Add(new VertexTraits(-0.05, 0.1, 0));
            controlPoint.Vertices.Add(new VertexTraits(-0.2, 0.1, 0));
            controlPoint.Vertices.Add(new VertexTraits(-0.2, 0, 0));
            controlPoint.Vertices.Add(new VertexTraits(-0.2, -0.1, 0)); 
            controlPoint.Vertices.Add(new VertexTraits(-0.05, -0.1, 0));
            controlPoint.Vertices.Add(new VertexTraits(0.1, -0.1, 0));
            controlPoint.Vertices.Add(new VertexTraits(0.1, 0, 0));
            #endregion
            return controlPoint;
        }

        public TriMesh CreateNURBSEllipse(TriMesh mesh)
        {
            TriMesh nurbs = new TriMesh();
            double x = 0;
            double y = 0;
            double z = 0;
            double m1 = 0, m2 = 0;
            double h = 0;
            double[] w = { 0.9, 0.5 };//权值
            double[] ut = new double[4000];
            for (int i = 0; i < ConfigCurve.Instance.CurveNUM; i++)
            {
                if (i >= 3999)
                    break;
                ut[i] = (double)((double)i / (double)2000) * 7;
            }
            for (int t = 0; t < ConfigCurve.Instance.CurveNUM; t++)
            {
                double u = (double)t / (double)2000;
                for (int i = 0; i < ellipseAll + 1; i++)
                {
                    m1 = m1 + mesh.Vertices[i].Traits.Position.x * CN(i, 3, u, ut) * w[i % 2];
                    m2 = m2 + mesh.Vertices[i].Traits.Position.y * CN(i, 3, u, ut) * w[i % 2];
                }
                for (int i = 0; i < ellipseAll + 1; i++)
                {
                    h = h + CN(i, 3, u, ut) * w[i % 2];
                }
                x = m1 / h;
                y = m2 / h;
                nurbs.Vertices.Add(new VertexTraits(x, y, z));
                m1 = 0; m2 = 0; h = 0;
            }
            return nurbs;
        }





        private double CN(int i, int k, double u, double[] ut)
        {
            if (k == 0)
            {
                if (u >= ut[i] && u < ut[i + 1])
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            if (ut[i + k] - ut[i] == 0)
            {
                return 0;
            }
            if (ut[i + k + 1] - ut[i + 1] == 0)
            {
                return 0;
            }
            return (u - ut[i]) / (ut[i + k] - ut[i]) * CN(i, k - 1, u, ut)
                + (ut[i + k + 1] - u) / (ut[i + k + 1] - ut[i + 1]) * CN(i + 1, k - 1, u, ut);
        }


    }
}
