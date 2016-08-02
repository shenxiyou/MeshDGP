using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public class Triangle 
    {
        public Vector3D A = Vector3D.Zero;
        public Vector3D B = Vector3D.Zero;
        public Vector3D C = Vector3D.Zero;


        private static Triangle instance = null;
        public static Triangle Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Triangle();
                }
                return instance;
            }
        }

        private Triangle()
        {

        }


        public Triangle(Vector3D a, Vector3D b, Vector3D c)
        {
            this.A = a;
            this.B = b;
            this.C = c;
        }

        public Vector3D BaryCenter(Vector3D a, Vector3D b, Vector3D c)
        {
            return (a + b + c) / 3;
        }

        public double ComputeArea(Vector3D a, Vector3D b, Vector3D c)
        {
            double area = 0;
            area = ((b - a).Cross(c - a)).Length() / 2.0;

            return area;
        }


        public Vector3D ComputeEdge(Vector3D v1, Vector3D v2)
        {
            return v2 - v1;
        }

        public Vector3D ComputeCircumCenter()
        {
           return this.ComputeCircumCenter(A, B, C);
        }

        public Vector3D ComputeCircumCenter(Vector3D vertex0, Vector3D vertex1, Vector3D vertex2)
        {
            int TriType;
            Vector3D circumCentre = Vector3D.Zero;
            List<double> lengthOfTheEdge = new List<double>();
            List<Vector3D> triVertices = new List<Vector3D>();
            
            triVertices.Add(vertex0);
            triVertices.Add(vertex1);
            triVertices.Add(vertex2);  
            double AB = (vertex0  - vertex1 ).Length();
            double BC = (vertex1  - vertex2 ).Length();
            double AC = (vertex2  - vertex0 ).Length();
            lengthOfTheEdge.Add(AB);
            lengthOfTheEdge.Add(BC);
            lengthOfTheEdge.Add(AC);
            lengthOfTheEdge.Sort();
            int maxAngel = (int)JudgeTheTriangle(lengthOfTheEdge);
            if (maxAngel < 89)
            {
                TriType = 0;
            }
            else if (maxAngel == 89 || maxAngel == 90 || maxAngel == 91)
            {
                TriType = 1;
            }
            else
            {
                TriType = 2;
            }
            switch (TriType)
            {
                case 1:
                case 0: circumCentre = ComputeCircumCenterPosition(AB, BC, AC, triVertices);
                    double circumRadius = (circumCentre - vertex0 ).Length();
                    return circumCentre;
                case 2: return circumCentre = (vertex0  + vertex1  + vertex2 ) / 3;
                default: return new Vector3D(0, 0, 0);
            }
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
        private Vector3D ComputeCircumCenterPosition(double c, double a, double b, List<Vector3D> triVertices)
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
                                   + triVertices[1]  * y
                                   + triVertices[2]  * z)
                                   / ((2 * v) - w);
            return circumCentre;


        }
    }
}
