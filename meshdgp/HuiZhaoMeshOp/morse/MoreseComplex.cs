using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public class MorseComplex
    {
       
        private TriMesh mesh;

        public MorseVertice morseVertice = null; 

        public MorseComplex(TriMesh mesh)
        {
           
            this.mesh = mesh;

            morseVertice = RetrieveMorseVertice(mesh);
        }

        public MorseVertice RetrieveMorseVertice(TriMesh mesh)
        {
            MorseVertice morse = new MorseVertice();
            foreach (TriMesh.Vertex v in mesh.Vertices)
            {
                if (v.Traits.SelectedFlag == 1)
                {
                    morse.MinList.Add(v);
                }

                if (v.Traits.SelectedFlag == 2)
                {
                    morse.MaxList .Add(v);
                }

                if (v.Traits.SelectedFlag >=3)
                {
                    morse.SaddleList.Add(v);
                }

            }
            return morse;
        } 


        public Dictionary<TriMesh.Vertex, int> GetVertices(TriMesh mesh)
        {
            Dictionary<TriMesh.Vertex, int> vertexDict = new Dictionary<HalfEdgeMesh.Vertex, int>();

            foreach (var v in  mesh.Vertices)
            {
                int count = 0;
                foreach (var edge in v.Edges)
                {
                    if (edge.Traits.SelectedFlag != 0)
                    {
                        count++;
                    }
                }
                if (count > 2)
                {
                    vertexDict[v] = 2;
                }
            }

            foreach (var v in  morseVertice.MinList)
            {
                vertexDict[v] = 0;
            }

            foreach (var v in  morseVertice.SaddleList)
            {
                if (v.Traits.SelectedFlag > 3)
                {
                    vertexDict[v] = 1;
                }
            }
            return vertexDict;
        }

        public Region[] GetRegion(Dictionary<TriMesh.Vertex, int> vertexDict)
        { 
            List<List<TriMesh.Face>> partition= TriMeshUtil.RetrieveFacePatchBySelectedEdge(mesh);  
            List<Region> list = new List<Region>(); 
            foreach (var part in partition)
            {
                bool[] regionFlags = new bool[this.mesh.Faces.Count];
                foreach (var face in part)
                {
                    regionFlags[face.Index] = true;
                }

                TriMesh.Vertex center = null;
                foreach (var max in  morseVertice.MaxList)
                {
                    foreach (var face in max.Faces)
                    {
                        if (regionFlags[face.Index])
                        {
                            center = max;
                        }
                    }
                }

                TriMesh.HalfEdge[] boundary = TriMeshUtil.RetrieveRegionBoundaryHalfEdge(this.mesh, regionFlags);
                List<TriMesh.Vertex> round = new List<HalfEdgeMesh.Vertex>();

                foreach (var hf in boundary)
                {
                    if (vertexDict.ContainsKey(hf.ToVertex))
                    {
                        round.Add(hf.ToVertex);
                    }
                    hf.Edge.Traits.SelectedFlag = 1;
                    hf.Edge.Traits.Color = Color4.Red;
                }
                list.Add(new Region { Center = center, Round = round.ToArray() });
            }
            return list.ToArray();
        }

        
        

        public   TriMesh CreateTriangle(Dictionary<TriMesh.Vertex, int> vertexDict, Region[] regions)
        {

            TriMesh mesh = new TriMesh();

            foreach (var v in vertexDict.Keys)
            {
                v.HalfEdge = null;
                this.mesh.AppendToVertexList(v);
            }

            foreach (var v in morseVertice.MaxList)
            {
                v.HalfEdge = null;
                this.mesh.AppendToVertexList(v);
            } 

            foreach (var region in regions)
            {
                if (region.Round.Length > 3)
                {
                    for (int i = 0; i < region.Round.Length; i++)
                    {
                        int next = (i + 1) % region.Round.Length;
                        try
                        {
                             mesh.Faces.AddTriangles(region.Center, region.Round[i], region.Round[next]);
                        }
                        catch (Exception e)
                        {
                            //this.mesh.Faces.AddTriangles(region.Center, region.Round[next], region.Round[i]);
                            //Console.WriteLine(e.ToString());
                        }
                    }
                }
                else if (region.Round.Length == 3)
                {
                     mesh.Faces.AddTriangles(region.Round);
                }
            }

            return mesh ;
        }

        public TriMesh  BuildWithMax()
        { 
            Dictionary<TriMesh.Vertex, int> vertexDict = this.GetVertices(mesh);
            Region[] regions = this.GetRegion(vertexDict);
         
            return CreateTriangle(vertexDict, regions);
        }

        public void SetColor()
        {
            int i=0;
            foreach (var v in this.morseVertice.MaxList)
            {
                foreach (var face in v.Faces)
                {
                    face.Traits.SelectedFlag = (byte)i;
                    face.Traits.Color = TriMeshUtil.SetRandomColor(i);

                }
                i++;
            }
        }

        

        public class Region
        {
            public TriMesh.Vertex Center;
            public TriMesh.Vertex[] Round;
        }
    }
}
