using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public static partial class TriMeshUtil
    {

        public static List<double> ComputeAngle(TriMesh.Face face)
        {
            List<double> angles = new List<double>();

            double angle1 = ComputeAngle(face.GetVertex(0).HalfEdge);
            double angle2 = ComputeAngle(face.GetVertex(1).HalfEdge);
            double angle3 = ComputeAngle(face.GetVertex(2).HalfEdge);

            angles.Add(angle1);
            angles.Add(angle2);
            angles.Add(angle3);

            return angles;
        }



        public static List<double> ComputeAngleDegree(TriMesh.Face face)
        {
            List<double> angles = ComputeAngle(face);

            for (int i = 0; i < angles.Count; i++)
            {
                angles[i] = angles[i] * 180 / 3.14;
            }
           
            return angles;
        }


        public static double ComputeAngles2(double[] lengthOftheEdgeArr)
        {
            double a = lengthOftheEdgeArr[0];
            double b = lengthOftheEdgeArr[1];
            double c = lengthOftheEdgeArr[2];

            double angles = (Math.Acos((Math.Pow(a, 2) + Math.Pow(b, 2) - Math.Pow(c, 2)) / (2 * a * b))) * 360 / (2 * Math.PI);
            return angles;

        }

        public static double ComputeAngle(TriMesh.HalfEdge op)
        {
            double a = (op.FromVertex.Traits.Position - op.ToVertex.Traits.Position).Length();
            double b = (op.ToVertex.Traits.Position - op.Next.ToVertex.Traits.Position).Length();
            double c = (op.Next.ToVertex.Traits.Position - op.FromVertex.Traits.Position).Length();

            double angle = Math.Acos((b * b + c * c - a * a) / (2 * b * c));
            return angle;
        }
        

        public static double[] ComputeAngle(TriMesh mesh)
        {
            double[] angle = new double[mesh.HalfEdges.Count];
            for (int i = 0; i < mesh.HalfEdges.Count; i++)
            {
                angle[i]=ComputeAngle(mesh.HalfEdges[i]);
            }
            return angle;
        }

        public static double ComputeAngle(TriMesh.HalfEdge hf1, TriMesh.HalfEdge hf2)
        {
            Vector3D hf1v = hf1.ToVertex.Traits.Position - hf1.FromVertex.Traits.Position;
            Vector3D hf2v = hf2.ToVertex.Traits.Position - hf2.FromVertex.Traits.Position;

            double cosAngle = hf1v.Dot(hf2v) / (hf1v.Length() * hf2v.Length());

            if (cosAngle > 1)
            {
                cosAngle = 1.0;
            }
            else if (cosAngle < -1)
            {
                cosAngle = -1.0;
            }

            double angle = Math.Acos(cosAngle);

            return angle;
        }

        public static double[] ComputeDihedralAngle(TriMesh mesh)
        {
            double[] angles = new double[mesh.Edges.Count];

            for (int i = 0; i < mesh.Edges.Count; i++)
            {
                angles[i] = ComputeDihedralAngle(mesh.Edges[i]);
            }
            return angles;
        }

        public static double ComputeDihedralAngle(TriMesh.Edge edge)
        {
            TriMesh.Face f1 = edge.Face0;
            TriMesh.Face f2 = edge.Face1;
            if (f1 != null && f2 != null)
            {
                Vector3D f1Normal = ComputeNormalFace(f1).Normalize();
                Vector3D f2Normal = ComputeNormalFace(f2).Normalize();
                double cosa = f1Normal.Dot(f2Normal)
                           / (f1Normal.Length() * f2Normal.Length());

                return Math.Acos(cosa);
            }
            else
            {
                return 0;
            }
        }
    }
}
