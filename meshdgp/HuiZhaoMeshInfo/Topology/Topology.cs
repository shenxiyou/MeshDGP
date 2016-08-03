using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public static partial class TriMeshUtil
    {
        public static TriMesh Combine(List<TriMesh> meshes)
        {
            TriMesh combine = new TriMesh();
            foreach (TriMesh mesh in meshes)
            {
                if (mesh != null)
                {
                    TriMesh.Vertex[] arr = new TriMesh.Vertex[mesh.Vertices.Count];
                    foreach (TriMesh.Vertex v in mesh.Vertices)
                    { 
                        arr[v.Index] = combine.Vertices.Add(new VertexTraits(v.Traits.Position));
                        arr[v.Index].Traits = v.Traits;
                    }
                    foreach (TriMesh.Face face in mesh.Faces)
                    {
                        TriMesh.Face faceNew=combine.Faces.Add(arr[face.GetVertex(0).Index],
                                          arr[face.GetVertex(1).Index],
                                          arr[face.GetVertex(2).Index]);
                        faceNew.Traits = face.Traits;
                    }
                }
            }
            return combine;
        }

        public static TriMesh Combine(TriMesh mesh1, TriMesh mesh2)
        {
            TriMesh combine = new TriMesh();
            List<TriMesh> meshes = new List<TriMesh>();
            meshes.Add(mesh1);
            meshes.Add(mesh2);

            return Combine(meshes);
           
        }

        public static int CountComponents(TriMesh mesh,bool color)
        {
            int count = 0;
            bool[] visited = new bool[mesh.Faces.Count];
            Queue<TriMesh.Face> queue = new Queue<HalfEdgeMesh.Face>(); 
            queue.Enqueue(mesh.Faces[0]);
            visited[0] = true;
            while (queue.Count != 0)
            {
                TriMesh.Face face = queue.Dequeue();
                if (color)
                {
                    face.Traits.SelectedFlag = 1;
                    face.Traits.Color = TriMeshUtil.SetRandomColor(count);
                }
                foreach (var hf in face.Halfedges)
                {
                    if (hf.Opposite.Face != null && 
                       !visited[hf.Opposite.Face.Index])
                    {
                        queue.Enqueue(hf.Opposite.Face);
                        visited[hf.Opposite.Face.Index] = true;
                    }
                } 
                if (queue.Count == 0)
                {
                    count++;
                    for (int i = 0; i < visited.Length; i++)
                    {
                        if (!visited[i])
                        {
                            queue.Enqueue(mesh.Faces[i]);
                            visited[i] = true;
                            break;
                        }
                    }
                }
            }
            return count;
        }

        public static int CountBoundary(TriMesh mesh)
        {
            return TriMeshUtil.RetrieveBoundaryEdgeAll(mesh).Count;
        }

        public static int CountGenus(TriMesh mesh)
        {
            int euler = CountEulerCharacteristic(mesh);
            int b = CountBoundary(mesh);
            int c = CountComponents(mesh,false);
            return c - (euler + b) / 2;
        }

        public static int CountEulerCharacteristic(TriMesh mesh)
        {
            return mesh.Vertices.Count - mesh.Edges.Count + mesh.Faces.Count;
        }

        public static string BuildStatus(TriMesh mesh)
        {
            if (mesh == null)
                return null;
            string status = "Name: " + mesh.FileName;
            status += "    Vertice: " + mesh.Vertices.Count;
            status += "    Edges: " + mesh.Edges.Count;
            status += "    Faces: " + mesh.Faces.Count;
            status += "    Boundaries: " + TriMeshUtil.CountBoundary(mesh).ToString();
            status += "    Genus: " + TriMeshUtil.CountGenus(mesh).ToString();
            status += "    Component: " + TriMeshUtil.CountComponents(mesh,false).ToString();
            status += "    Euler: " + CountEulerCharacteristic(mesh).ToString();
            status += "    Area: " + ComputeAreaTotal(mesh).ToString();
            status += "    Volumen: " + ComputeVolume(mesh).ToString();
            status += "    Radius: " + ComputeBoundingSphere(mesh).Radius.ToString();

            return status;
        }


        public static Dictionary<string, string> BuildMeshInfo(TriMesh mesh)
        {
            Dictionary<string, string> meshinfo = new Dictionary<string, string>();

            meshinfo.Add("Verties", mesh.Vertices.Count.ToString());
            meshinfo.Add("Edges", mesh.Edges.Count.ToString());
            meshinfo.Add("Faces",mesh.Faces.Count.ToString());
            
            meshinfo.Add("Elur Formula For Genus=0", "V-E+F = 2");
            meshinfo.Add("General Elur Formula on Genus !=0",  "V-E+F=2(C-G)-B, C is Component Number,G is Genus Number,B is boundary Number");
            meshinfo.Add("Elur Characteristic is ",  "2(C-G)-B or V-E+F");
            meshinfo.Add("Elur Characteristic on Current Mesh",
              "V-E+F = " +
               mesh.Vertices.Count.ToString() + " - " +
               mesh.Edges.Count.ToString() + " + " +
               mesh.Faces.Count.ToString() + " = " +
               CountEulerCharacteristic(mesh).ToString());
            meshinfo.Add("Component", CountComponents(mesh,false).ToString());
            meshinfo.Add("Boundary Number",TriMeshUtil.CountBoundary(mesh).ToString());
            meshinfo.Add("Genus","G=-((V-E+F)+B)/2+C");

            meshinfo.Add("Genus on Current mesh", CountGenus(mesh).ToString());
            meshinfo.Add("Total Gaussian Curvature is ", TriMeshUtil.ComputeTotalGaussianCurvarture(mesh).ToString());
            meshinfo.Add("Gaussian-Bonnet Theroem", "2*PI*(V-E+F) = SUM(Gaussian Curvature)");
            meshinfo.Add("Current Gaussian-Bonnet Theroem",
                "2*PI*(V-E+F)=" +
                "2 x 3.1415 x " + CountEulerCharacteristic(mesh).ToString() + " = " +
                (Math.PI * 2 * CountEulerCharacteristic(mesh))
                + " ??=  " + TriMeshUtil.ComputeTotalGaussianCurvarture(mesh)
                );
            meshinfo.Add("Average Area", TriMeshUtil.ComputeAreaAverage(mesh).ToString());
            meshinfo.Add("Volume", TriMeshUtil.ComputeVolume(mesh).ToString());
            meshinfo.Add("Total Area", TriMeshUtil.ComputeAreaTotal(mesh).ToString());

            return meshinfo;

        }

    }
}
