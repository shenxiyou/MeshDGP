using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics;

namespace GraphicResearchHuiZhao
{
    public partial class OpenGLTriMesh
    {
         




        public void DrawTrivialConnection(TriMesh mesh, 
            Vector3D[] faceVectors,int N)
        {
            double eps = 1e-3;

            double perAngle = 2 * Math.PI / N;

            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                Vector3D vector =Vector3D.Normalize(faceVectors[i]);
                TriMesh.Face face = mesh.Faces[i];
                Vector3D center = TriMeshUtil.GetMidPoint(face);
                Vector3D normal = TriMeshUtil.ComputeNormalFace(face);
                double inradius = TriMeshUtil.ComputeInradius(face);

                Quaternion rotate = Quaternion.RotationAxis(normal, perAngle);
                Quaternion v = new Quaternion(vector, 0);

                for (int j = 0; j < N; j++)
                {
                    Vector3D a = center + inradius * v.ImagePart;
                    //Vector3D b = center - inradius * v.ImagePart;

                    v = rotate * v * rotate.Conjugate();
                    GL.Begin(BeginMode.Lines);
                    GL.Vertex3(a.x, a.y, a.z);
                    GL.Vertex3(center.x, center.y, center.z);
                    GL.End();

                } 
            } 
        }



    }
}
