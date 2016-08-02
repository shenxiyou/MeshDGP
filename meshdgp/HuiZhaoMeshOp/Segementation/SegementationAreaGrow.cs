using System;
using System.Collections.Generic;
using System.Text;
 


namespace GraphicResearchHuiZhao
{
    public class SegementationAreaGrow:SegmentationBase
    {
        

        public SegementationAreaGrow(TriMesh mesh):base(mesh) 
        {
            
        }


        private List<double> area = new List<double>();//与种子点相关的三角形的面积的均值
        private double vthreshold = 0;//阈值
        private double k = 0.896;//0<k<1

        public void ComputeAverageArea(List<TriMesh.Vertex> selectedVertices)//计算种子点相关三角形的面积的均值
        {
            foreach (TriMesh.Vertex v in selectedVertices)
            {
                double a = 0, b = 0, c = 0;
                double sum = 0; 
                sum = TriMeshUtil.ComputeAreaOneRing(v)/ v.FaceCount ;//计算相关三角形的面积的均值 
                area.Add(sum);
            }
        }

        public double ComputeSArea(TriMesh.Vertex vertex)//计算相关三角形的面积
        {
            

            double s = TriMeshUtil.ComputeAreaOneRing(vertex) / vertex.FaceCount;
         
            return s;
        }

        public void AreaGrowByVertex()//以点进行扩散
        {
            List<TriMesh.Vertex> selectedVertices = new List<TriMesh.Vertex>();//获得初始选取的点

            for (int i = 0; i < this.mesh.Vertices.Count; i++)
            {
                if (this.mesh.Vertices[i].Traits.SelectedFlag > 0)
                {
                    selectedVertices.Add(this.mesh.Vertices[i]);
                }
            }

            ComputeAverageArea(selectedVertices);

            for (int i = 0; i < selectedVertices.Count; i++)//分割涂色
            {
                foreach (TriMesh.Vertex vertex in selectedVertices[i].Vertices)
                {
                    if (vertex.Traits.SelectedFlag == 0)
                    {
                        double temp = 0, temp1 = 0, temp2 = 0;
                        temp1 = ComputeSArea(vertex);
                        temp2 = ComputeSArea(selectedVertices[i]);
                        temp = Math.Abs(temp1 - temp2);
                        vthreshold = k * area[i];//阈值的最终结果
                        if (temp < vthreshold)
                        {
                            vertex.Traits.SelectedFlag = selectedVertices[i].Traits.SelectedFlag;
                        }
                    }
                }
            }
        }
        
        public void AreaGrowByFace()
        {
            List<TriMesh.Face> selectedFaces = new List<TriMesh.Face>();

            for (int i = 0; i < this.mesh.Faces.Count; i++)
            {
                if (this.mesh.Faces[i].Traits.SelectedFlag > 0)
                {
                    selectedFaces.Add(this.mesh.Faces[i]);
                }
            }

            for (int i = 0; i < selectedFaces.Count; i++)
            {
                foreach (TriMesh.Face  face in selectedFaces[i].Faces)
                {
                    if (face.Traits.SelectedFlag == 0)
                    {
                        double temp1 = 0, temp2 = 0, temp = 0,area=0;
                        temp1 =TriMeshUtil.ComputeAreaFace(face);
                        temp2 = TriMeshUtil.ComputeAreaFace(selectedFaces[i]);
                        temp = Math.Abs(temp1 - temp2);
                        for (int j = 0; j < selectedFaces[i].FaceCount; j++)
                        {
                            area += TriMeshUtil.ComputeAreaFace(selectedFaces[i].GetFace(j));
                        }
                        area = area / selectedFaces[i].FaceCount;
                        if (temp < 0.916 * area)
                        {
                            face.Traits.SelectedFlag = selectedFaces[i].Traits.SelectedFlag;
                        }
                    }
                }
            }
           
        }
       
        public double ComputeFaceAngle(TriMesh.Face face1, TriMesh.Face face2)//计算两个面的cos角度值
        {
            double angel = 0;
            double a = 0, b = 0, c = 0;
            c = face1.Traits.Normal.x * face2.Traits.Normal.x + face1.Traits.Normal.y * face2.Traits.Normal.y + face1.Traits.Normal.z * face2.Traits.Normal.z;
            a = Math.Pow(Math.Pow(face1.Traits.Normal.x, 2) + Math.Pow(face1.Traits.Normal.y, 2) + Math.Pow(face1.Traits.Normal.z, 2), 0.5);
            b = Math.Pow(Math.Pow(face2.Traits.Normal.x, 2) + Math.Pow(face2.Traits.Normal.y, 2) + Math.Pow(face2.Traits.Normal.z, 2), 0.5);
            angel = c / (a + b);
            //angel = Math.Acos(angel);
            return angel;
        }



        public void FaceInit()
        {
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                if (mesh.Vertices[i].Traits.SelectedFlag > 0)
                {
                    mesh.Vertices[i].GetFace(0).Traits.SelectedFlag = mesh.Vertices[i].Traits.SelectedFlag;
                }
            }
        }
        public void AreaGrowByFaceAuto()
        {
            List<TriMesh.Face> selectFace = new List<TriMesh.Face>();//获取被选中的面
            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                if (mesh.Faces[i].Traits.SelectedFlag > 0)
                {
                    selectFace.Add(mesh.Faces[i]);
                }
            }

            for (int i = 0; i < selectFace.Count; i++)
            {
                
                foreach (TriMesh.Face f in selectFace[i].Faces)
                {
                    if (f.Traits.SelectedFlag == 0)
                    {
                       
                        if ( ComputeFaceAngle(f, selectFace[i]) >0.449)
                        {
                            f.Traits.SelectedFlag = selectFace[i].Traits.SelectedFlag;
                        }
                    }
                }
            }
        }

        public void ByFacesNotSelect()
        {
            while (true)
            {
                List<TriMesh.Face> face = new List<TriMesh.Face>();
                for (int i = 0; i < mesh.Faces.Count; i++)
                {
                    if (mesh.Faces[i].Traits.SelectedFlag == 0)
                    {
                        face.Add(mesh.Faces[i]);
                    }
                }
                byte count = 1;
                for (int i = 0; i < face.Count; i++)
                {
                    face[i].Traits.SelectedFlag = count;
                    foreach (TriMesh.Face f in face[i].Faces)
                    {
                        if (f.Traits.SelectedFlag == 0)
                        {
                            if (ComputeFaceAngle(f, face[i]) > 0.449)
                            {
                                f.Traits.SelectedFlag = face[i].Traits.SelectedFlag;
                            }
                        }

                    }
                    count++;
                }

                if (face.Count == 0)
                    break;
            }


        }
    }
}
