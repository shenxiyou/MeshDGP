using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class SegementationKMean : SegmentationBase
    {


 
        public static int k=7;
        public static int j = 0,a=0,b=0,d=0,e=0,f=0,g=0,h=0;
        public static double threshold=0.000025;
        Vector3D new1, new2, new3, new4, new5, new6, new7;
        public SegementationKMean(TriMesh mesh):base(mesh) 
        {
             
        }

       

        public void Init()
        {
            List<TriMesh.Face> selectedFaces = new List<TriMesh.Face>();

            for (int i = 0; i < this.mesh.Vertices.Count; i++)
            {
                if (this.mesh.Vertices[i].Traits.SelectedFlag > 0)
                {
                    foreach (TriMesh.Face face in mesh.Vertices[i].Faces)
                    {
                        face.Traits.SelectedFlag = mesh.Vertices[i].Traits.SelectedFlag;
                        selectedFaces.Add(face);
                    }
                }
            } 

            
        }

        public void ByFacetwo()
        {


            for (int i = 0; i < this.mesh.Faces.Count; i++)
            {
                if (this.mesh.Faces[i].Traits.SelectedFlag > 0)
                {
                    foreach (TriMesh.Face face in mesh.Faces[i].Faces)
                    {
                        face.Traits.SelectedFlag = mesh.Faces[i].Traits.SelectedFlag;
                    }
                }
            }
        }



        public void SubFunction(TriMesh mesh, List<TriMesh.Vertex> Center)
        {
            int c = 0;
            List<TriMesh.Vertex> selectedVertices = new List<TriMesh.Vertex>();
            double[] x = new double[k];
            for (int i = 0; i < this.mesh.Vertices.Count; i++)
            {
                for (int t = 0; t < k; t++)
                {
                    x[t] = Distance(Center[t], this.mesh.Vertices[i]);
                }
                c = FindMin(x);
                switch (c)
                {
                    case 0:
                        {
                            new1.x += this.mesh.Vertices[i].Traits.Position.x;
                            new1.y += this.mesh.Vertices[i].Traits.Position.y;
                            new1.z += this.mesh.Vertices[i].Traits.Position.z;
                            a++;
                            this.mesh.Vertices[i].Traits.SelectedFlag = Center[c].Traits.SelectedFlag;
                            break;
                        }
                    case 1:
                        {
                            new2.x += this.mesh.Vertices[i].Traits.Position.x;
                            new2.y += this.mesh.Vertices[i].Traits.Position.y;
                            new2.z += this.mesh.Vertices[i].Traits.Position.z;
                            b++;
                            this.mesh.Vertices[i].Traits.SelectedFlag = Center[c].Traits.SelectedFlag;
                            break;
                        }
                    case 2:
                        {
                            new3.x += this.mesh.Vertices[i].Traits.Position.x;
                            new3.y += this.mesh.Vertices[i].Traits.Position.y;
                            new3.z += this.mesh.Vertices[i].Traits.Position.z;
                            d++;
                            this.mesh.Vertices[i].Traits.SelectedFlag = Center[c].Traits.SelectedFlag;
                            break;
                        }
                    case 3:
                        {
                            new4.x += this.mesh.Vertices[i].Traits.Position.x;
                            new4.y += this.mesh.Vertices[i].Traits.Position.y;
                            new4.z += this.mesh.Vertices[i].Traits.Position.z;
                            e++;
                            this.mesh.Vertices[i].Traits.SelectedFlag = Center[c].Traits.SelectedFlag;
                            break;
                        }
                    case 4:
                        {
                            new5.x += this.mesh.Vertices[i].Traits.Position.x;
                            new5.y += this.mesh.Vertices[i].Traits.Position.y;
                            new5.z += this.mesh.Vertices[i].Traits.Position.z;
                            f++;
                            this.mesh.Vertices[i].Traits.SelectedFlag = Center[c].Traits.SelectedFlag;
                            break;
                        }
                    case 5:
                        {
                            new6.x += this.mesh.Vertices[i].Traits.Position.x;
                            new6.y += this.mesh.Vertices[i].Traits.Position.y;
                            new6.z += this.mesh.Vertices[i].Traits.Position.z;
                            g++;
                            this.mesh.Vertices[i].Traits.SelectedFlag = Center[c].Traits.SelectedFlag;
                            break;
                        }
                    case 6:
                        {
                            new7.x += this.mesh.Vertices[i].Traits.Position.x;
                            new7.y += this.mesh.Vertices[i].Traits.Position.y;
                            new7.z += this.mesh.Vertices[i].Traits.Position.z;

                            h++;
                            this.mesh.Vertices[i].Traits.SelectedFlag = Center[c].Traits.SelectedFlag;
                            break;
                        }
                }
            }
        }
        public List<TriMesh.Vertex> ComputeNewCenter(List<TriMesh.Vertex> Center, int a, int b, int d, int e, int f, int g, int h)  //计算出新的聚类集
        {
            List<TriMesh.Vertex> selectedVertices = new List<TriMesh.Vertex>();
            selectedVertices = Center;
            selectedVertices[0].Traits.Position.x = new1.x / a;
            selectedVertices[0].Traits.Position.y = new1.y / a;
            selectedVertices[0].Traits.Position.z = new1.z / a;

            selectedVertices[1].Traits.Position.x = new2.x / b;
            selectedVertices[1].Traits.Position.y = new2.y / b;
            selectedVertices[1].Traits.Position.z = new2.z / b;

            selectedVertices[2].Traits.Position.x = new3.x / d;
            selectedVertices[2].Traits.Position.y = new3.y / d;
            selectedVertices[2].Traits.Position.z = new3.z / d;

            selectedVertices[3].Traits.Position.x = new4.x / e;
            selectedVertices[3].Traits.Position.y = new4.y / e;
            selectedVertices[3].Traits.Position.z = new4.z / e;

            selectedVertices[4].Traits.Position.x = new5.x / f;
            selectedVertices[4].Traits.Position.y = new5.y / f;
            selectedVertices[4].Traits.Position.z = new5.z / f;

            selectedVertices[5].Traits.Position.x = new6.x / g;
            selectedVertices[5].Traits.Position.y = new6.y / g;
            selectedVertices[5].Traits.Position.z = new6.z / g;

            selectedVertices[6].Traits.Position.x = new7.x / h;
            selectedVertices[6].Traits.Position.y = new7.y / h;
            selectedVertices[6].Traits.Position.z = new7.z / h;
            a = b = d = e = f = g = h = 0;
            return selectedVertices;
        }
        public Boolean Compare(List<TriMesh.Vertex> c1, List<TriMesh.Vertex> c2)   //将新旧两个聚类集进行比较，若大于阀值就为真，则要继续进行迭代，反之推出
        {
            for (int i = 0; i < k; i++)
            {
                if (((c1[i].Traits.Position.x - c2[i].Traits.Position.x) >= threshold) || ((c1[i].Traits.Position.y - c2[i].Traits.Position.y) >= threshold) || ((c1[i].Traits.Position.z - c2[i].Traits.Position.z) >= threshold))
                {
                    return true;
                }
                else return false;
            }
            return false;
        }
       
        public void KMeans()
        {
            List<TriMesh.Vertex> Center = new List<TriMesh.Vertex>();
            List<TriMesh.Vertex> selectedVertices = new List<TriMesh.Vertex>();
            for (int i = 0; i < this.mesh.Vertices.Count; i++)
            {
                if (this.mesh.Vertices[i].Traits.SelectedFlag > 0)
                {
                    Center.Add(this.mesh.Vertices[i]);
                    j++;
                }
            }
            SubFunction(mesh, Center);
            List<TriMesh.Vertex> newCenter = new List<TriMesh.Vertex>();
            selectedVertices = ComputeNewCenter(Center, a, b, d, e, f, g, h);
            bool right = Compare(Center, selectedVertices);
            while (right == true)
            {
                newCenter = selectedVertices;
                selectedVertices = null;
                SubFunction(mesh, newCenter);
                selectedVertices = ComputeNewCenter(newCenter, a, b, d, e, f, g, h);
                right = Compare(newCenter, selectedVertices);
            }


        }
        public int FindMin(double[] a)                //一个图形中的点距离中心集最短距离的点，将点归类
        {
            int n = 0;
            for (int t = 0; t < k; t++)
            {
                if (a[n] > a[t]) n = t;
            }
            return n;
        }



        public double Distance(TriMesh.Vertex c, TriMesh.Vertex d)   //利用距离公式计算点到中心集的点的距离
        {
            double average;
            average = Math.Sqrt(Math.Pow((c.Traits.Position.x - d.Traits.Position.x), 2) + Math.Pow((c.Traits.Position.y - d.Traits.Position.y), 2) + Math.Pow((c.Traits.Position.z - d.Traits.Position.z), 2));
            return average;
        }

         







    }
}
