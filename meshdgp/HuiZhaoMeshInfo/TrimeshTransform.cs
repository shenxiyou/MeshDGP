using System;
using System.Collections.Generic;
using System.Text; 
namespace GraphicResearchHuiZhao 
{
    public partial class TriMeshUtil 
    {

        public static Matrix4D ComputeMatrixScale(double scale)
        {
            Matrix4D m = Matrix4D.Identity(); 
            m[0, 0] =scale;
            m[1, 1] = scale;
            m[2, 2] = scale;
            m[3, 3] = -scale;
          
            return m;
        }

        public static Matrix4D ComputeMatrixScale(double scaleX,double scaleY, double scaleZ)
        {
            Matrix4D m = Matrix4D.Identity();
            m[0, 0] = scaleX;
            m[1, 1] = scaleY;
            m[2, 2] = scaleZ;
            m[3, 3] = 1;

            return m;
        }

        public static Matrix4D ComputeMatrixRotation(Vector3D rotation)
        {
            Matrix4D rotationX = ComputeMatrixRotationX(rotation.x);
            Matrix4D rotationY = ComputeMatrixRotationY(rotation.y);
            Matrix4D rotationZ = ComputeMatrixRotationZ(rotation.z); 
            Matrix4D m = rotationX * rotationY * rotationZ;
            return m;
        }

        public static Matrix4D ComputeMatrixRotationX(double angle)
        {
            Matrix4D m = Matrix4D.Identity();
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);
            m[0, 0] = 1;
            m[1, 1] = cos;
            m[1, 2] = sin;
            m[2, 1] = -sin;
            m[2, 2] = cos;
            m[3, 3] = 1;
            return m;
        }

        public static Matrix4D ComputeMatrixRotationY(double angle)
        {
            Matrix4D m = Matrix4D.Identity();
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);
            m[0, 0] = cos;
            m[0, 2] = -sin;
            m[1, 1] = 1;
            m[2, 0] = sin;
            m[2, 2] = cos;
            m[3, 3] = 1;
            return m;
        }

        public static Matrix4D ComputeMatrixRotationZ(double angle)
        {
            Matrix4D m = Matrix4D.Identity();
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);
            m[0, 0] = cos;
            m[0, 1] = sin;
            m[1, 0] = -sin;
            m[1, 1] = cos;
            m[2, 2] = 1;
            m[3, 3] = 1;
            return m;
        }

        public static Matrix4D ComputeMatrixRotationX(double sin, double cos)
        {
            Matrix4D m = Matrix4D.Identity();
            m[0, 0] = 1;
            m[1, 1] = cos;
            m[1, 2] = sin;
            m[2, 1] = -sin;
            m[2, 2] = cos;
            m[3, 3] = 1;
            return m;
        }

        public static Matrix4D ComputeMatrixRotationY(double sin, double cos)
        {
            Matrix4D m = Matrix4D.Identity();
            m[0, 0] = cos;
            m[0, 2] = -sin;
            m[1, 1] = 1;
            m[2, 0] = sin;
            m[2, 2] = cos;
            m[3, 3] = 1;
            return m;
        }

        public static Matrix4D ComputeMatrixRotationZ(double sin, double cos)
        {
            Matrix4D m = Matrix4D.Identity();
            m[0, 0] = cos;
            m[0, 1] = sin;
            m[1, 0] = -sin;
            m[1, 1] = cos;
            m[2, 2] = 1;
            m[3, 3] = 1;
            return m;
        }


        public static Matrix4D ComputeMatrixMove(ref Vector3D move)
        {
            Matrix4D m = Matrix4D.Identity();
            m[3, 0] = move.x;
            m[3, 1] = move.y;
            m[3, 2] = move.z;
            return m;
        }

        //对一个点或方向向量进行旋转

        public static Vector3D TransformationVector3D(Vector3D v, Matrix4D m)
        {
            Vector4D v1 = new Vector4D();
            v1.x = v.x;
            v1.y = v.y;
            v1.z = v.z;
            v1.w = 1;
            v1 *= m;
            v.x = v1.x;
            v.y = v1.y;
            v.z = v1.z;
            return v;
        }
        public static void TransformRotationX(TriMesh mesh, double angle)//根据角度绕X轴旋转
        {
            Matrix4D m = ComputeMatrixRotationX(angle);
            TransformationTriMesh(mesh,m);

        }

       
        public static void TransformRotationX(TriMesh mesh, double sin, double cos)//直接给出参数绕X轴旋转
        {
            Matrix4D m = ComputeMatrixRotationX(sin, cos);
            TransformationTriMesh(mesh, m);
        }

      
        public static void TransformRotationY(TriMesh mesh, double angle)//根据角度绕Y轴旋转
        {
            Matrix4D m = ComputeMatrixRotationY(angle);
            TransformationTriMesh(mesh, m);

        }

       
        public static void TransformRotationY(TriMesh mesh, double sin, double cos)//直接给出参数绕Y轴旋转
        {
            Matrix4D m = ComputeMatrixRotationY(sin, cos);
            TransformationTriMesh(mesh, m);
        }

       
        public static void TransformRotationZ(TriMesh mesh, double angle)//根据角度绕Z轴旋转
        {
            Matrix4D m = ComputeMatrixRotationZ(angle);
            TransformationTriMesh(mesh, m);

        }

       
        public static void TransformRotationZ(TriMesh mesh, double sin, double cos)//直接给出参数绕Z轴旋转
        {
            Matrix4D m = ComputeMatrixRotationZ(sin, cos);
            TransformationTriMesh(mesh, m);
        }

       
        public static void TransformationMove(TriMesh mesh, double x, double y, double z) 
        {
            Vector3D move = new Vector3D(x, y, z);
            Matrix4D m = ComputeMatrixMove(ref move);
            TransformationTriMesh(mesh, m);
        }

        public static void TransformationMove(TriMesh mesh, Vector3D move) 
        {
            Matrix4D m = ComputeMatrixMove(ref move);
            TransformationTriMesh(mesh, m);
        }

        public static void TransformationMove(List<TriMesh.Vertex> vertice, Vector3D move) 
        {
            Matrix4D m = ComputeMatrixMove(ref move);
            TransformationVertex(vertice, m);
        }


       
        public static void TransformationRotation(TriMesh mesh, Vector3D rotation) 
        { 
            Matrix4D m = ComputeMatrixRotation(rotation);
            TransformationTriMesh(mesh, m);
        }

        public static void TransformationScale(TriMesh mesh, double scale) 
        {
            Matrix4D m = ComputeMatrixScale(scale);
            TransformationTriMesh(mesh, m);
        }

        public static void TransformationRotation(List<TriMesh.Vertex> vertice, Vector3D rotation,bool local) 
        {
            Vector3D center = Vector3D.Zero;
            Matrix4D m = Matrix4D.Identity();
            if (local)
            {
                center = TriMeshUtil.GetCenter(vertice);

                Matrix4D moveC = ComputeMatrixMove(ref center);
                center.Negate();
                Matrix4D moveCReverse = ComputeMatrixMove(ref  center);
                Matrix4D r  = ComputeMatrixRotation(rotation);
                m = moveCReverse * r * moveC;

            }
            else
            {

                 m = ComputeMatrixRotation(rotation);
            }
            TransformationVertex(vertice, m);
        }

        public static void TransformationWorldPosition(TriMesh mesh, Vector3D worldX, Vector3D worldY, Vector3D worldZ)//按照新世界坐标轴进行变换
        {
            Matrix4D m = Matrix4D.Identity();//清空
            m[0, 0] = worldX.x;
            m[0, 1] = worldX.y;
            m[0, 2] = worldX.z;
            m[1, 0] = worldY.x;
            m[1, 1] = worldY.y;
            m[1, 2] = worldY.z;
            m[2, 0] = worldZ.x;
            m[2, 1] = worldZ.y;
            m[2, 2] = worldZ.z;
            TransformationTriMesh(mesh, m);
        }


        public static void TransformationScale(TriMesh mesh, Matrix4D m)//对一个Trimesh中的所有点进行旋转
        {
            Vector4D v = new Vector4D();
            for (int i = 0; i < mesh.Vertices.Count; i++)//对每个点进行变换
            {
                v.x = mesh.Vertices[i].Traits.Position.x;
                v.y = mesh.Vertices[i].Traits.Position.y;
                v.z = mesh.Vertices[i].Traits.Position.z;
                v.w = 1;
                v *= m;
                mesh.Vertices[i].Traits.Position.x = v.x;
                mesh.Vertices[i].Traits.Position.y = v.y;
                mesh.Vertices[i].Traits.Position.z = v.z;
            }
        }
        


        public static void TransformationTriMesh(TriMesh mesh, Matrix4D m)//对一个Trimesh中的所有点进行旋转
        {
            Vector4D v = new Vector4D();
            for (int i = 0; i < mesh.Vertices.Count; i++)//对每个点进行变换
            {
                v.x = mesh.Vertices[i].Traits.Position.x;
                v.y = mesh.Vertices[i].Traits.Position.y;
                v.z = mesh.Vertices[i].Traits.Position.z;
                v.w = 1;
                v *= m;
                mesh.Vertices[i].Traits.Position.x = v.x;
                mesh.Vertices[i].Traits.Position.y = v.y;
                mesh.Vertices[i].Traits.Position.z = v.z;
            }
        }

        public static void TransformationVertex(List<TriMesh.Vertex> vertice, Matrix4D m)//对一个Trimesh中的所有点进行旋转
        {
            Vector4D v = new Vector4D();
            for (int i = 0; i < vertice.Count; i++) 
            {
                v.x = vertice[i].Traits.Position.x;
                v.y = vertice[i].Traits.Position.y;
                v.z = vertice[i].Traits.Position.z;
                v.w = 1;
                v *= m;
                vertice[i].Traits.Position.x = v.x;
                vertice[i].Traits.Position.y = v.y;
                vertice[i].Traits.Position.z = v.z;
            }
        }

        public static void TransformationVertex(TriMesh.Vertex vertex, Vector3D vec) 
        {

            Matrix4D m = ComputeMatrixMove(ref vec);
            TransformationVertex(vertex, m);
        }

        public static void TransformationVertex( TriMesh.Vertex vertex, Matrix4D m) 
        {
            Vector4D v = new Vector4D();

            v.x = vertex.Traits.Position.x;
            v.y = vertex.Traits.Position.y;
            v.z = vertex.Traits.Position.z;
            v.w = 1;
            v *= m;
            vertex.Traits.Position.x = v.x;
            vertex.Traits.Position.y = v.y;
            vertex.Traits.Position.z = v.z;
             
        }
        public static Vector3D TransformOrigin(Matrix4D m)
        {
            Vector4D v1 = Vector4D.Zero;
            v1.w = 1;
            v1 = v1 * m;
            Vector3D v = Vector3D.Zero;
            v.x = v1.x;
            v.y = v1.y;
            v.z = v1.z;
            return v;
        }

        public static void  TransformRoatationV(TriMesh mesh, Vector3D first,Vector3D second)
        {
            Matrix3D rot=ComputeRotationRodrigues(first,second,1);
            for(int i=0;i<mesh.Vertices.Count;i++)
            {
                mesh.Vertices[i].Traits.Position =rot *  mesh.Vertices[i].Traits.Position; 
                
            }
        }

        

        public static Matrix3D ComputeRotationRodrigues(Vector3D first, Vector3D second, int step)
        {

            Vector3D cross = first.Cross(second);

            double sin = cross.Length();
            double cos = first.Dot(second);

            //if (cos > 0.99)
            //{
            //    return Matrix3D.IdentityMatrix();
            //}


            double angle = Math.Acos(cos);

            cos = Math.Cos(angle / step);
            sin = Math.Sin(angle / step);

            cross = cross.Normalize();
            double x = cross.x;
            double y = cross.y;
            double z = cross.z;

            Matrix3D result = Matrix3D.IdentityMatrix();


            //result[0, 0] = cos+(1-cos)*x*x;
            //result[0, 1] = x * y*(1 - cos) - z * sin;
            //result[0, 2] = y* sin+x*z*(1-cos);
            //result[1, 0] = z * sin + x * y * (1 - cos);
            //result[1, 1] = cos + y * y * (1 - cos);
            //result[1, 2] = -x * sin + y * z * (1 - cos);
            //result[2, 0] = -y * sin + x * z * (1 - cos);
            //result[2, 1] = x * sin + y * z * (1 - cos);
            //result[2, 2] =cos + z* z * (1 - cos);



            //result[0, 0] = cos*(y*y+z*z) +   x * x;
            //result[0, 1] = x * y * (1 - cos) - z * sin;
            //result[0, 2] = y * sin + x * z * (1 - cos);
            //result[1, 0] = z * sin + x * y * (1 - cos);
            //result[1, 1] = cos*(x*x+z*z) + y * y ;
            //result[1, 2] = -x * sin + y * z * (1 - cos);
            //result[2, 0] = -y * sin + x * z * (1 - cos);
            //result[2, 1] = x * sin + y * z * (1 - cos);
            //result[2, 2] = cos*(x*x+y*y) + z * z  ;

            result[0, 0] = cos * (1 - x * x) + x * x;
            result[0, 1] = x * y * (1 - cos) - z * sin;
            result[0, 2] = y * sin + x * z * (1 - cos);
            result[1, 0] = z * sin + x * y * (1 - cos);
            result[1, 1] = cos * (1 - y * y) + y * y;
            result[1, 2] = -x * sin + y * z * (1 - cos);
            result[2, 0] = -y * sin + x * z * (1 - cos);
            result[2, 1] = x * sin + y * z * (1 - cos);
            result[2, 2] = cos * (1 - z * z) + z * z;

            // result = result.Transpose();
            return result;
        }


    }
}
