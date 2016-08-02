


using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;


namespace GraphicResearchHuiZhao
{
    public partial class TriMeshIO
    {

        private static TriMeshIO singleton = new TriMeshIO();

        public static string PointsetsFile;

        public static TriMeshIO Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new TriMeshIO();
                return singleton;
            }
        }

        /// <summary>
        /// Creates a new, identical mesh.
        /// </summary>
        /// <returns>A deep copy of the mesh.</returns>
        public static TriMesh Copy(TriMesh mesh)
        {
            // Use serialization to create a deep copy
            using (MemoryStream ms = new MemoryStream(mesh.Vertices.Count * 300))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, mesh);
                ms.Seek(0, SeekOrigin.Begin);
                return (TriMesh)bf.Deserialize(ms);
            }
        }


        public static TriMesh Clone(TriMesh mesh)
        {
            TriMesh newMesh = new TriMesh();
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                VertexTraits traits = new VertexTraits(mesh.Vertices[i].Traits.Position.x,
                                                       mesh.Vertices[i].Traits.Position.y,
                                                       mesh.Vertices[i].Traits.Position.z);
                newMesh.Vertices.Add(traits);
            }

            TriMesh.Vertex[] faceVetices = new TriMesh.Vertex[3];
            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                int faceVertexIndex1 = mesh.Faces[i].GetVertex(0).Index;
                int faceVertexIndex2 = mesh.Faces[i].GetVertex(1).Index;
                int faceVertexIndex3 = mesh.Faces[i].GetVertex(2).Index;

                faceVetices[0] = newMesh.Vertices[faceVertexIndex1];
                faceVetices[1] = newMesh.Vertices[faceVertexIndex2];
                faceVetices[2] = newMesh.Vertices[faceVertexIndex3];
                newMesh.Faces.AddTriangles(faceVetices);
            }
            newMesh.TrimExcess();
            return newMesh;
        }
        public static TriMesh ReadFile(string fileName)
        {
            TriMesh mesh = null;
            if (fileName.EndsWith("obj"))
            {
                mesh = TriMeshIO.FromObjFile(fileName);

            }
            if (fileName.EndsWith("off"))
            {
                mesh = TriMeshIO.FromOffFile(fileName);
            }
            if (fileName.EndsWith("ply"))
            {
                mesh = TriMeshIO.FromPlyFile(fileName);
            }
            if (fileName.EndsWith("npts"))
            {
                PointsetsFile = fileName;
            }
            if (mesh != null)
            {
                TriMeshUtil.ScaleToUnit(mesh,1.0);
                TriMeshUtil.MoveToCenter(mesh); 
              
                TriMeshUtil.SetUpNormalVertex(mesh);
            }
            return mesh;
        }


      

    





    }
}
