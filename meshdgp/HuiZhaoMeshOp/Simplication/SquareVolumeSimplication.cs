using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public class SquareVolumeSimplication : MergeTriangleSimplicationBase
    {
        ErrorPair[] faceError;

        public SquareVolumeSimplication(TriMesh mesh)
            : base(mesh)
        {

        }

        protected override void Init()
        {
            this.heap = new MinHeap<TriMesh.Face>(this.Mesh.Faces.Count);
            this.handle = new HeapNode<TriMesh.Face>[this.Mesh.Faces.Count];
            this.faceError = new ErrorPair[this.Mesh.Faces.Count];
            this.traits = new TriMeshTraits(this.Mesh);
            this.traits.Init();

            foreach (TriMesh.Face face in this.Mesh.Faces)
            {
                ErrorPair pair = this.GetErrorPair(face);
                this.faceError[face.Index] = pair;
                this.handle[face.Index] = heap.Add(pair.Error, face);
            }
        }

        protected override Vector3D GetPos(TriMesh.Face target)
        {
            return TriMeshUtil.GetMidPoint(target);
            //return this.faceError[target.Index].Pos;
        }

        protected override double GetValue(TriMesh.Face target)
        {
            throw new NotImplementedException();
        }

        protected override void AfterMerge(HalfEdgeMesh.Vertex v)
        {
            foreach (var face in this.removed)
            {
                this.heap.Del(handle[face.Index]);
            }

            this.traits.MergeUpdate(v);
            foreach (var face in v.Faces)
            {
                ErrorPair pair = this.GetErrorPair(face);
                this.faceError[face.Index] = pair;
                this.heap.Update(handle[face.Index], pair.Error);
            }
        }

        struct ErrorPair
        {
            public Vector3D Pos;
            public double Error;
        }

        private ErrorPair GetErrorPair(TriMesh.Face face)
        {
            Matrix4D m1 = this.GetVolumeMatrix(face);
            if (m1[1] != m1[4] || m1[2] != m1[8] || m1[6] != m1[9] ||
               m1[3] != m1[12] || m1[7] != m1[13] || m1[11] != m1[14]
                ) throw new Exception("Matrix m1 is not symmetric");
            Matrix4D m2 = Matrix4D.ZeroMatrix;
            for (int i = 0; i <= 11; i++)
            {
                m2[i] = m1[i];
            }
            m2[12] = 0;
            m2[13] = 0;
            m2[14] = 0;
            m2[15] = 1;

            Vector3D newPos = Vector3D.Zero;

            double det = Util.Solve(ref m2, ref newPos);
            if (det == 0)
            {
                newPos = TriMeshUtil.GetMidPoint(face);
            }
            double error = this.GetError(m1, newPos);
            return new ErrorPair() { Pos = newPos, Error = error };
        }

        private Matrix4D GetVolumeMatrix(TriMesh.Face face)
        {
            Vector3D normal = this.traits.FaceNormal[face.Index];
            Plane p = new Plane(face.HalfEdge.ToVertex.Traits.Position, normal);
            Matrix4D m = this.Square(p);

            double area = this.traits.FaceArea[face.Index];
            foreach (var item in TriMeshUtil.RetrieveOneRingFaceOfFace(face))
            {
                area += this.traits.FaceArea[item.Index];
            }

            m *= area * area;
            return m;
        }

        private Matrix4D Square(Plane plane)
        {
            Matrix4D k = Matrix4D.ZeroMatrix;
            double a = plane.Normal.x;
            double b = plane.Normal.y;
            double c = plane.Normal.z;
            double d = plane.D;

            k[0] = a * a; k[1] = a * b; k[2] = a * c; k[3] = a * d;
            k[4] = a * b; k[5] = b * b; k[6] = b * c; k[7] = b * d;
            k[8] = a * c; k[9] = b * c; k[10] = c * c; k[11] = c * d;
            k[12] = a * d; k[13] = b * d; k[14] = c * d; k[15] = d * d;

            return k;
        }

        private double GetError(Matrix4D Q, Vector3D v)
        {
            double x = v.x;
            double y = v.y;
            double z = v.z;
            return Q[0] * x * x + 2 * Q[1] * x * y + 2 * Q[2] * x * z + 2 * Q[3] * x +
                Q[5] * y * y + 2 * Q[6] * y * z + 2 * Q[7] * y +
                Q[10] * z * z + 2 * Q[11] * z + Q[15];
        }
    }
}
