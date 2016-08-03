using System; 
using System.IO;
using System.ComponentModel;
 

 

namespace GraphicResearchHuiZhao
{
   
    public class MeshRecord : BaseRecord
    {
        private string filename;
        private NonManifoldMesh mesh = null;
        private Matrix4D modelViewMatrix = Matrix4D.IdentityMatrix;
 

        public string Filename
        {
            get { return filename; }
        }
        public int VertexCount
        {
            get { return mesh.VertexCount; }
        }
        public int FaceCount
        {
            get { return mesh.FaceCount; }
        }
        public int VertexCoordinateCount
        {
            get
            {
                return mesh.TextextCoordinate.Length;
            }
        }

        public string TextureFile
        {
            get
            {
                return mesh.TextureFile;
            }
             
        }

        [Browsable(false)]
        public NonManifoldMesh Mesh
        {
            get { return mesh; }
            set { mesh = value; }
        }
        [Browsable(false)]
        public Matrix4D ModelViewMatrix
        {
            get { return modelViewMatrix; }
            set { modelViewMatrix = value; }
        }
       

        public MeshRecord(string filename, NonManifoldMesh mesh)
        {
            this.filename = filename;
            this.mesh = mesh;
        }

        public override string ToString()
        {
            return Path.GetFileName(filename);
        }
    }
   
}
