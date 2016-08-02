using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public enum TriangleType
    {
        Acute, Right, Obtuse
    
    }
    public class TriangleTest 
    {
        public Vector3D A = Vector3D.Zero;
        public Vector3D B = Vector3D.Zero;
        public Vector3D C = Vector3D.Zero;

        public Vector2D sA = Vector2D.Zero;
        public Vector2D sB = Vector2D.Zero;
        public Vector2D sC = Vector2D.Zero;

        public double AB, AC, BC;

        public List<double> lengthOfTheEdge = new List<double>();
        public List<Vector3D> triVertices = new List<Vector3D>();

        private static TriangleTest instance = null;
        public static TriangleTest Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TriangleTest();
                }
                return instance;
            }
        }

        private TriangleTest()
        {

        }


        public TriangleTest(Vector3D a, Vector3D b, Vector3D c)
        {
            this.A = a;
            this.B = b;
            this.C = c;

            Vector2D sa;
            Vector2D sb;
            Vector2D sc;

            sa = new Vector2D(1, 5);
            sb = new Vector2D(2, 3);
            sc = new Vector2D(4, 2);

            this.sA = sa;
            this.sB = sb;
            this.sC = sc;

            triVertices.Add(A);
            triVertices.Add(B);
            triVertices.Add(C);

            AB = (A - B).Length();
            BC = (B - C).Length();
            AC = (C - A).Length();

            lengthOfTheEdge.Add(AB);
            lengthOfTheEdge.Add(BC);
            lengthOfTheEdge.Add(AC);
            lengthOfTheEdge.Sort();

        }

        #region

        //public Vector3D BaryCenter(Vector3D a, Vector3D b, Vector3D c)
        //{
        //    return (a + b + c) / 3;
        //}

        //public double ComputeArea(Vector3D a, Vector3D b, Vector3D c)
        //{
        //    double area = 0;
        //    area = ((b - a).Cross(c - a)).Length() / 2.0;

        //    return area;
        //}


        //public Vector3D ComputeEdge(Vector3D v1, Vector3D v2)
        //{
        //    return v2 - v1;
        //}

        //public Vector3D ComputeCircumCenter()
        //{
        //   return this.ComputeCircumCenter(A, B, C);
        //}

        //public Vector3D ComputeCircumCenter(Vector3D vertex0, Vector3D vertex1, Vector3D vertex2)
        //{
        //    int TriType;
        //    Vector3D circumCentre = Vector3D.Zero;
        //    List<double> lengthOfTheEdge = new List<double>();
        //    List<Vector3D> triVertices = new List<Vector3D>();
            
        //    triVertices.Add(vertex0);
        //    triVertices.Add(vertex1);
        //    triVertices.Add(vertex2);  
        //    double AB = (vertex0  - vertex1 ).Length();
        //    double BC = (vertex1  - vertex2 ).Length();
        //    double AC = (vertex2  - vertex0 ).Length();
        //    lengthOfTheEdge.Add(AB);
        //    lengthOfTheEdge.Add(BC);
        //    lengthOfTheEdge.Add(AC);
        //    lengthOfTheEdge.Sort();
        //    int maxAngel = (int)JudgeTheTriangle(lengthOfTheEdge);
        //    if (maxAngel < 89)
        //    {
        //        TriType = 0;
        //    }
        //    else if (maxAngel == 89 || maxAngel == 90 || maxAngel == 91)
        //    {
        //        TriType = 1;
        //    }
        //    else
        //    {
        //        TriType = 2;
        //    }
        //    switch (TriType)
        //    {
        //        case 1:
        //        case 0: circumCentre = ComputeCircumCenterPosition(AB, BC, AC, triVertices);
        //            double circumRadius = (circumCentre - vertex0 ).Length();
        //            return circumCentre;
        //        case 2: return circumCentre = (vertex0  + vertex1  + vertex2 ) / 3;
        //        default: return new Vector3D(0, 0, 0);
        //    }
        //}


        //public double JudgeTheTriangle(List<double> lengthOfTheEdge)
        //{
        //    double angle;
        //    angle = ComputeAngles(lengthOfTheEdge.ToArray());
        //    return angle;
        //}

        //public double ComputeAngles(double[] lengthOftheEdgeArr)
        //{
        //    double a = lengthOftheEdgeArr[0];
        //    double b = lengthOftheEdgeArr[1];
        //    double c = lengthOftheEdgeArr[2];
        //    double angles = (Math.Acos((Math.Pow(a, 2) + Math.Pow(b, 2)
        //                     - Math.Pow(c, 2)) / (2 * a * b))) * 360 / (2 * Math.PI);
        //    return angles;

        //}
        //public Vector3D ComputeCircumCenterPosition(double c, double a, double b, List<Vector3D> triVertices)
        //{
        //    double a2 = Math.Pow(a, 2);
        //    double b2 = Math.Pow(b, 2);
        //    double c2 = Math.Pow(c, 2);
        //    double a4 = Math.Pow(a, 4);
        //    double b4 = Math.Pow(b, 4);
        //    double c4 = Math.Pow(c, 4);
        //    double x = a2 * (b2 + c2 - a2);
        //    double y = b2 * (a2 + c2 - b2);
        //    double z = c2 * (a2 + b2 - c2);
        //    double v = a2 * b2 + a2 * c2 + b2 * c2;
        //    double w = a4 + b4 + c4;
        //    Vector3D circumCentre = (triVertices[0] * x
        //                           + triVertices[1]  * y
        //                           + triVertices[2]  * z)
        //                           / ((2 * v) - w);
        //    return circumCentre;


        //}

        #endregion



        public double ComputeAreaFirst()
        { // 海伦公式
            double area = 0;
            double P;
            double AB, BC, AC;
            AB = ComputeEdgeLength(1);
            BC = ComputeEdgeLength(2);
            AC = ComputeEdgeLength(3);

            P = (AB + BC + AC) / 2;

            area = Math.Sqrt(P * (P - AB) * (P - BC) * (P - AC));

            return area;
        }

        public double ComputeAreaSecond()
        { // 底乘以高除以二
            double area = 0;
            double edgeLength = 0, height = 0;
            double pedalX, pedalY;
            double dx, dy;

            edgeLength = ComputeEdgeLength(1);
            pedalX = ComputePedalPoint(1).x;
            pedalY = ComputePedalPoint(1).y;
            dx = pedalX - this.sC.x;
            dy = pedalY - this.sC.y;
            height = Math.Sqrt(dx * dx + dy * dy);

            area = (edgeLength * height) / 2;

            return area;
        }

        public double ComputeAreaThird()
        { // absinc / 2
            double area = 0;
            double a, b, c;
            double anglec;

            a = ComputeEdgeLength(2);
            b = ComputeEdgeLength(3);
            c = ComputeEdgeLength(1);

            anglec = (Math.Acos((Math.Pow(a, 2) + Math.Pow(b, 2) - Math.Pow(c, 2))
                    / (2 * a * b))) * 360 / (2 * Math.PI);

            area = (a * b * Math.Sin(anglec)) / 2;

            return area;
        }

        public double JudgeTheTriangle(List<double> lengthOfTheEdge)
        {
            double angle;
            angle = ComputeAngles(lengthOfTheEdge.ToArray());
            return angle;
        }

        public double ComputeAngles(double[] lengthOftheEdgeArr)
        {
            double a = lengthOftheEdgeArr[0];
            double b = lengthOftheEdgeArr[1];
            double c = lengthOftheEdgeArr[2];
            double angles = (Math.Acos((Math.Pow(a, 2) + Math.Pow(b, 2)
                             - Math.Pow(c, 2)) / (2 * a * b))) * 360 / (2 * Math.PI);
            return angles;
        }

        public Vector3D ComputeCircumCenterPosition(double c, double a, double b, List<Vector3D> triVertices)
        {
            double a2 = Math.Pow(a, 2);
            double b2 = Math.Pow(b, 2);
            double c2 = Math.Pow(c, 2);
            double a4 = Math.Pow(a, 4);
            double b4 = Math.Pow(b, 4);
            double c4 = Math.Pow(c, 4);
            double x = a2 * (b2 + c2 - a2);
            double y = b2 * (a2 + c2 - b2);
            double z = c2 * (a2 + b2 - c2);
            double v = a2 * b2 + a2 * c2 + b2 * c2;
            double w = a4 + b4 + c4;
            Vector3D circumCentre = (triVertices[0] * x
                                     + triVertices[1] * y
                                   + triVertices[2] * z)
                                   / ((2 * v) - w);
            return circumCentre;
        }

        public double ComputeCircumCenterRadius(Vector3D vertex0, Vector3D vertex1, Vector3D vertex2)
        {

            Vector3D circumCentre = Vector3D.Zero;

            double circumRadius = 0;

            circumCentre = ComputeCircumCenterPosition(AB, BC, AC, triVertices);
            circumRadius = (circumCentre - vertex0).Length();
            return circumRadius;
        }

        public Vector3D ComputeIncircleCenterPosition()
        {
            Vector3D incircleCenter = Vector3D.Zero;
            Vector3D zeroAxis = Vector3D.Zero;

            if(B.z - C.z == 0)
                zeroAxis = new Vector3D(1, 1, 0);
            else
                zeroAxis = new Vector3D(B.z * (C.x - B.x) / (B.z - C.z) + B.x,
                                                B.z * (C.y - B.y) / (B.z - C.z) + B.y,
                                                0);

            double edgeAxis = (zeroAxis - incircleCenter).Length();
            double edgePoint = (B - zeroAxis).Length();

            double angAxis = (Math.Acos((Math.Pow(AB, 2) + Math.Pow(edgeAxis, 2)
                             - Math.Pow(edgePoint, 2)) / (2 * AB * edgeAxis))) * 360 / (2 * Math.PI);

            double angA = (Math.Acos((Math.Pow(AB, 2) + Math.Pow(AC, 2)
                             - Math.Pow(BC, 2)) / (2 * AB * AC))) * 360 / (2 * Math.PI);
            double angB = (Math.Acos((Math.Pow(AB, 2) + Math.Pow(BC, 2)
                             - Math.Pow(AC, 2)) / (2 * AB * BC))) * 360 / (2 * Math.PI);

            double tanA = Math.Tan(angA / 2);
            double tanB = Math.Tan(angB / 2);

            double radius = AB / (tanA + tanB);

            double keyAngle = angAxis + (angA / 2);
            double keyEdge = radius/(angA / 2);

            incircleCenter = new Vector3D(Math.Abs(keyEdge * Math.Cos(keyAngle) * Math.Sin(keyAngle)),
                                        keyEdge * Math.Cos(keyAngle) * Math.Cos(keyAngle),
                                        keyEdge * Math.Sin(keyAngle) * Math.Sin(keyAngle));

            return incircleCenter;
        }
        public double ComputeIncircleCenterRadius()
        {
            double radius = 0;
            
            double angA = (Math.Acos((Math.Pow(AB, 2) + Math.Pow(AC, 2)
                             - Math.Pow(BC, 2)) / (2 * AB * AC))) * 360 / (2 * Math.PI);
            double angB = (Math.Acos((Math.Pow(AB, 2) + Math.Pow(BC, 2)
                             - Math.Pow(AC, 2)) / (2 * AB * BC))) * 360 / (2 * Math.PI);

            double tanA = Math.Tan(angA / 2);
            double tanB = Math.Tan(angB / 2);

            radius = AB / (tanA + tanB);

            return radius;
        }

        // 重心
        public Vector3D ComputeBaryCenter()
        { 
            Vector3D center = Vector3D.Zero;

            center = (A + B + C) / 3;

            return center;
        }

        // 内心
        public void ComputeIncircleCenter(out Vector3D center, out double radius)
        {
            center = Vector3D.Zero;
            radius = 0;
            double a, b, c;

            c = ComputeEdgeLength(1);
            b = ComputeEdgeLength(3);
            a = ComputeEdgeLength(2);

            center.x = (a * this.A.x + b * this.B.x + c * this.C.x) / (a + b + c);
            center.y = (a * this.A.y + b * this.B.y + c * this.C.y) / (a + b + c);
            center.z = (a * this.A.z + b * this.B.z + c * this.C.z) / (a + b + c);

            radius = center.Cross(C - A).Length();

        }
        //public void ComputeIncircleCenter(out Vector3D center, out double radius)
        //{
        //    center = Vector3D.Zero;
        //    radius = 0;

        //    center = ComputeIncircleCenterPosition();
        //    radius = ComputeIncircleCenterRadius();


        //}

        // 外心
        public void ComputeCircumCenter(out Vector3D center, out double radius)
        {
            radius = ComputeCircumCenterRadius(A, B, C);

            Vector3D circumCentre = Vector3D.Zero;

            center = ComputeCircumCenterPosition(AB, BC, AC, triVertices);
        }

        // 垂足
        public Vector2D ComputePedalPoint(int index)
        { // 纯无聊的数学没什么可解释的
            Vector2D pedal = Vector2D.Zero;
            double px = 0, py = 0;

            if (index == 1)
            {
                double kab;
                double b1;
                kab = (this.sA.y - this.sB.y) / (this.sA.x - this.sB.x);
                b1 = this.sA.y - kab * this.sA.x;
                px = (kab * this.sC.y + this.sC.x - kab * b1) / (kab * kab + 1);
                py = kab * px + b1;
            }
            if (index == 2)
            {
                double kbc;
                double b2;
                kbc = (this.sC.y - this.sB.y) / (this.sC.x - this.sB.x);
                b2 = this.sB.y - kbc * this.sB.x;
                px = (kbc * this.sA.y + this.sA.x - kbc * b2) / (kbc * kbc + 1);
                py = kbc * px + b2;
            }
            if (index == 3)
            {
                double kac;
                double b3;
                kac = (this.sA.y - this.sC.y) / (this.sA.x - this.sC.x);
                b3 = this.sC.y - kac * this.sC.x;
                px = (kac * this.sB.y + this.sB.x - kac * b3) / (kac * kac + 1);
                py = kac * px + b3;
            }
            pedal.x = px;
            pedal.y = py;

            return pedal;
        }

        // 内角角度
        public double ComputeAngle(int index)
        {
            double angle = 0;

            switch (index)
            {
                case 1:
                    angle = (Math.Acos((Math.Pow(AB, 2) + Math.Pow(BC, 2)
                             - Math.Pow(AC, 2)) / (2 * AB * BC))) * 360 / (2 * Math.PI);
                    return angle;
                case 2:
                    angle = (Math.Acos((Math.Pow(AB, 2) + Math.Pow(AC, 2)
                             - Math.Pow(BC, 2)) / (2 * AB * AC))) * 360 / (2 * Math.PI);
                    return angle;
                case 3:
                    angle = (Math.Acos((Math.Pow(AC, 2) + Math.Pow(BC, 2)
                             - Math.Pow(AB, 2)) / (2 * AC * BC))) * 360 / (2 * Math.PI);
                    return angle;
                default:
                    return 0;
            }
        }

        // 边长
        public double ComputeEdgeLength(int index)
        {
            double length = 0;
            double plengthx = 0, plengthy = 0, plengthz = 0;

            if (index == 1)
            {
                plengthx = this.A.x - this.B.x;
                plengthy = this.A.y - this.B.y;
                plengthz = this.A.z - this.B.z;
                length = Math.Sqrt(plengthx * plengthx + plengthy * plengthy + plengthz * plengthz);
            }
            if (index == 2)
            {
                plengthx = this.C.x - this.B.x;
                plengthy = this.C.y - this.B.y;
                plengthz = this.C.z - this.B.z;
                length = Math.Sqrt(plengthx * plengthx + plengthy * plengthy + plengthz * plengthz);
            }
            if (index == 3)
            {
                plengthx = this.A.x - this.C.x;
                plengthy = this.A.y - this.C.y;
                plengthz = this.A.z - this.C.z;
                length = Math.Sqrt(plengthx * plengthx + plengthy * plengthy + plengthz * plengthz);
            }

            return length;
        }

        public List<Vector3D> ComputeNinePointCircle()
        {
            List<Vector3D> points = new List<Vector3D>(9);
            Vector3D barypoint;
            for (int i = 0; i < 9; i++)
            {
                points.Add(Vector3D.Zero);
            }

            barypoint = (A + B + C) / 3;
            points[0] = barypoint.Cross(B - A);
            points[1] = barypoint.Cross(C - A);
            points[2] = barypoint.Cross(B - C);
            points[3] = (A - B) / 2;
            points[4] = (A - C) / 2;
            points[5] = (B - C) / 2;
            //搞不懂，然哥我果然是个坑

            return points;
        }

        public List<Vector3D> ComputeMorleyTriangle()
        {
            List<Vector3D> points = new List<Vector3D>();
            for (int i = 0; i < 3; i++)
            {
                points.Add(Vector3D.Zero);
            }



            return points;
        }

        public List<Vector3D> ComputeSteinerInellipse()
        {
            List<Vector3D> points = new List<Vector3D>();
            points.Add(Vector3D.Zero);
            points.Add(Vector3D.Zero);


            return points;
        }

        public List<Vector3D> ComputeSteinercircumellipse()
        {
            List<Vector3D> points = new List<Vector3D>();
            points.Add(Vector3D.Zero);
            points.Add(Vector3D.Zero);


            return points;
        }

        public List<Vector3D> ComputeNobbsPoint()
        {
            List<Vector3D> points = new List<Vector3D>();

            double RA = 0, RB = 0, RC = 0;

            RA = (AB + AC - BC) / 2;
            RB = (AB + BC - AC) / 2;
            RC = (AC + BC - AB) / 2;

            Vector3D Pab = new Vector3D(A.x + (B.x - A.x) * RA / (RA + RB),
                                A.y + (B.y - A.y) * RA / (RA + RB),
                                A.z + (B.z - A.z) * RA / (RA + RB));

            Vector3D Pac = new Vector3D(A.x + (C.x - A.x) * RA / (RA + RC),
                                A.y + (C.y - A.y) * RA / (RA + RC),
                                A.z + (C.z - A.z) * RA / (RA + RC));

            Vector3D Pbc = new Vector3D(B.x + (C.x - B.x) * RB / (RB + RC),
                                B.y + (C.y - B.y) * RB / (RB + RC),
                                B.z + (C.z - B.z) * RB / (RB + RC));

            points.Add(Pab);
            points.Add(Pac);
            points.Add(Pbc);

            return points;
        }

        public List<double> ComputeTangentCircleRadius()
        {
            List<double> radius = new List<double>();

            double RA = 0, RB = 0, RC = 0;

            RA = (AB + AC - BC) / 2;
            RB = (AB + BC - AC) / 2;
            RC = (AC + BC - AB) / 2;

            radius.Add(RA);
            radius.Add(RB);
            radius.Add(RC);

            return radius;
        }

        public double ComputeSoddyCircleRadius()
        {
            double radius = 0;

            double RA = 0, RB = 0, RC = 0;

            RA = (AB + AC - BC) / 2;
            RB = (AB + BC - AC) / 2;
            RC = (AC + BC - AB) / 2;

            double t1 = RA + RB + RC;
            double t2 = Math.Pow(RA, 2) + Math.Pow(RB, 2) + Math.Pow(RC, 2);
            double c = 2 * t2 - Math.Pow(t1, 2);
            double b = (-2) * t1;

            double d = b * b - 4 * c;

            if (d < 0)
                return 0;
            else if (d == 0)
            {
                radius = (-b) / 2;
                if (radius >= RA + RB || radius >= RC + RB || radius >= RA + RC)
                    return radius;
                else
                    return 0;
            }
            else
            {
                radius = (-b - Math.Sqrt(d))/2;
                double temp = (-b + Math.Sqrt(d)) / 2;
                if (radius >= RA + RB || radius >= RC + RB || radius >= RA + RC)
                    return radius;
                else if (temp >= RA + RB || temp >= RC + RB || temp >= RA + RC)
                    return temp;
                else
                    return 0;
            }
        }

        public void ComputeSoddyCircle(out Vector3D center, out double radius)
        {
            center = Vector3D.Zero;
            radius = ComputeSoddyCircleRadius();
        
        
        }


        public TriangleType Type()
        {
            TriangleType type = TriangleType.Acute;

            int maxAngel = (int)JudgeTheTriangle(lengthOfTheEdge);
            if (maxAngel < 89)
            {
                type = TriangleType.Acute;
            }
            else if (maxAngel == 89 || maxAngel == 90 || maxAngel == 91)
            {
                type = TriangleType.Right;
            }
            else
            {
                type = TriangleType.Obtuse;
            }
            return type;
        }

        public Vector3D InscribedCirclePosition(int index)
        {
            Vector3D position = Vector3D.Zero;


            return position;
        }

        public bool IsTriangle()
        {
            bool istriangle = true;

            if (AB + AC > BC)
            {
                if (AB + BC > AC)
                {
                    if (AC + BC > AB)
                        istriangle = true;
                    else
                        istriangle = false;
                }
                else
                    istriangle = false;
            }
            else
                istriangle = false;

            return istriangle;
        }

        public TriMesh CreateMesh()
        {
            TriMesh mesh = new TriMesh();

            mesh.Vertices.Add(new VertexTraits(this.A));
            mesh.Vertices.Add(new VertexTraits(this.B));
            mesh.Vertices.Add(new VertexTraits(this.C));


            TriMesh.Vertex[] faceVertices = new TriMesh.Vertex[3];
            faceVertices[0] = mesh.Vertices[0];
            faceVertices[1] = mesh.Vertices[1];
            faceVertices[2] = mesh.Vertices[2];
            mesh.Faces.AddTriangles(faceVertices);

            return mesh;


        }







    }
}
