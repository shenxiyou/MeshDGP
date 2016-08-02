using System;
using System.Collections.Generic;
using System.Text;
 
namespace GraphicResearchHuiZhao 
{
    public partial class TriMesh  
    {
        
        public static TriMesh BulidSphere(string data)//通过文件建立Trimesh
        {
            data = "D:\\atet.obj";
            TriMesh sphere= FromObjFile(data);
            Matrix4D m = new Matrix4D();
            m[0, 0] = 0.01; m[1, 1] = 0.01; m[2, 2] = 0.01;
            Vector4D v = new Vector4D();
            for (int i = 0; i < sphere.Vertices.Count; i++)//对整个球进行缩放
            {
                v.x = sphere.Vertices[i].Traits.Position.x;
                v.y = sphere.Vertices[i].Traits.Position.y;
                v.z = sphere.Vertices[i].Traits.Position.z;
                v.w = 1;
                v *= m;
                sphere.Vertices[i].Traits.Position.x = v.x;
                sphere.Vertices[i].Traits.Position.y = v.y;
                sphere.Vertices[i].Traits.Position.z = v.z;
             }

            return sphere;
        }
        public static TriMesh BulidSphere()//建立球星Trimesh
        {
            TriMesh sphere = new TriMesh();

            sphere.Vertices.Add(new VertexTraits(-0.9972, 19.7347, -0.3015));
            sphere.Vertices.Add(new VertexTraits(-0.9972, 13.8535, -15.6699));
            sphere.Vertices.Add(new VertexTraits(-14.3067, 13.8535, -7.98572));
            sphere.Vertices.Add(new VertexTraits(-14.3067, 13.8535, 7.3828));
            sphere.Vertices.Add(new VertexTraits(-0.9972, 13.8535, 15.0670));
            sphere.Vertices.Add(new VertexTraits(12.3122, 13.8535, 7.3828));
            sphere.Vertices.Add(new VertexTraits(12.3122, 13.8535, -7.9857));
            sphere.Vertices.Add(new VertexTraits(-0.9972, -3.8925, -15.6699));
            sphere.Vertices.Add(new VertexTraits(-14.3067, -3.8925, -7.9857));
            sphere.Vertices.Add(new VertexTraits(-14.3067, -3.8925, 7.3828));
            sphere.Vertices.Add(new VertexTraits(-0.9972, -3.8925, 15.0670));
            sphere.Vertices.Add(new VertexTraits(12.3122, -3.8925, 7.3828));
            sphere.Vertices.Add(new VertexTraits(12.3122, -3.8925, -7.9857));
            sphere.Vertices.Add(new VertexTraits(-0.9972, -9.7738, -0.3015));


            BulidSphereFace(0, 1, 2,sphere);
            BulidSphereFace(0, 2, 3,sphere);
            BulidSphereFace(0, 3, 4, sphere);
            BulidSphereFace(0, 4, 5, sphere);
            BulidSphereFace(0, 5, 6, sphere);
            BulidSphereFace(0, 6, 1, sphere);
            BulidSphereFace(1, 7, 8, sphere);
            BulidSphereFace(1, 8, 2, sphere);
            BulidSphereFace(2, 8, 9, sphere);
            BulidSphereFace(2, 9, 3, sphere);
            BulidSphereFace(3, 9, 10, sphere);
            BulidSphereFace(3, 10, 4, sphere);
            BulidSphereFace(4, 10, 11, sphere);
            BulidSphereFace(4, 11, 5, sphere);
            BulidSphereFace(5, 11, 12, sphere);
            BulidSphereFace(5, 12, 6, sphere);
            BulidSphereFace(6, 12, 7, sphere);
            BulidSphereFace(6, 7, 1, sphere);
            BulidSphereFace(13, 8, 7, sphere);
            BulidSphereFace(13, 9, 8, sphere);
            BulidSphereFace(13, 10, 9, sphere);
            BulidSphereFace(13, 11, 10, sphere);
            BulidSphereFace(13, 12, 11, sphere);
            BulidSphereFace(13, 7, 12, sphere);

            sphere.TrimExcess();

            Matrix4D m = new Matrix4D();
            m[0, 0] = 0.003; m[1, 1] = 0.003; m[2, 2] = 0.003;
            Vector4D v = new Vector4D();
            for (int i = 0; i < sphere.Vertices.Count; i++)//对整个球进行缩放
            {
                v.x = sphere.Vertices[i].Traits.Position.x;
                v.y = sphere.Vertices[i].Traits.Position.y;
                v.z = sphere.Vertices[i].Traits.Position.z;
                v.w = 1;
                v *= m;
                sphere.Vertices[i].Traits.Position.x = v.x;
                sphere.Vertices[i].Traits.Position.y = v.y;
                sphere.Vertices[i].Traits.Position.z = v.z;
             }
            return sphere;

        }

        private static void BulidSphereFace(int v1, int v2, int v3,TriMesh mesh)//添加面
        {
            TriMesh.Vertex[] faceVertices = new TriMesh.Vertex[3];
            faceVertices[0] = mesh.Vertices[v1];
            faceVertices[1] = mesh.Vertices[v2];
            faceVertices[2] = mesh.Vertices[v3];
            TriMesh.Face[] addedFaces = mesh.Faces.AddTriangles(faceVertices);
        }

        public void BulidBone(double Leng)//建立8点的骨骼
        {

            this.Traits.HasFaceVertexNormals = true;
            this.Traits.HasTextureCoordinates = true;

            this.Vertices.Add(new VertexTraits(-0.01, -0.01, Leng));//创建长方体的时候直接使用两点间距离作为骨节长度
            this.Vertices.Add(new VertexTraits(0.01, -0.01, Leng));
            this.Vertices.Add(new VertexTraits(-0.01, 0.01, Leng));
            this.Vertices.Add(new VertexTraits(0.01, 0.01, Leng));
            this.Vertices.Add(new VertexTraits(-0.01, 0.01, 0));
            this.Vertices.Add(new VertexTraits(0.01, 0.01, 0));
            this.Vertices.Add(new VertexTraits(-0.01, -0.01, 0));
            this.Vertices.Add(new VertexTraits(0.01, -0.01, 0));

            BulidBoneFace(0, 1, 2);
            BulidBoneFace(2, 1, 3);
            BulidBoneFace(2, 3, 4);
            BulidBoneFace(4, 3, 5);
            BulidBoneFace(4, 5, 6);
            BulidBoneFace(6, 5, 7);
            BulidBoneFace(6, 7, 0);
            BulidBoneFace(0, 7, 1);
            BulidBoneFace(1, 7, 3);
            BulidBoneFace(3, 7, 5);
            BulidBoneFace(6, 0, 4);
            BulidBoneFace(4, 0, 2);

            this.TrimExcess();
        }

        private void BulidBoneFace(int v1, int v2, int v3)//为正方形添加面
        {
            TriMesh.Vertex[] faceVertices = new TriMesh.Vertex[3];
            faceVertices[0] = this.Vertices[v1];
            faceVertices[1] = this.Vertices[v2];
            faceVertices[2] = this.Vertices[v3];
            TriMesh.Face[] addedFaces = this.Faces.AddTriangles(faceVertices);
        }

    }
}

