using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class Region
    {
        TriMesh.Face Seedface;
        byte color;
        List<TriMesh.Face> Faces = new List<TriMesh.Face>();
        List<TriMesh.Face> BoundaryFaces = new List<TriMesh.Face>();
        List<TriMesh.Face> FinalBoundaryFaces = new List<TriMesh.Face>();
        int Bnumber = 0;
        int Allnumber = 0;
        double normal = 0;
        double dnormal = 0;
        TriMesh mesh = null;

        public Region(TriMesh mesh)
        {
            this.mesh = mesh;
        }
        public int Getallfacenumber()
        {
            return Faces.Count + BoundaryFaces.Count + FinalBoundaryFaces.Count;
        }
        public double Calculatenormal()
        {
            Vector3D n = Faces[0].Traits.Normal;
            for (int i = 1; i < Faces.Count; i++)
            {
                n = n + Faces[i].Traits.Normal;
            }
            normal = n.Length() / Faces.Count;
            return normal;
        }

        public List<TriMesh.Face> getBoundaryFaces()
        {
            return BoundaryFaces;
        }

        public int getBnumber()
        {
            return Bnumber;
        }
        public double CDnormal()
        {
            Vector3D n = Faces[0].Traits.Normal - Seedface.Traits.Normal;
            for (int i = 1; i < Faces.Count; i++)
            {
                n = n + (Faces[i].Traits.Normal - Seedface.Traits.Normal);
            }
            dnormal = n.Length() / Faces.Count;
            return dnormal;
        }

        public void AddFaces(TriMesh.Face Face, int n)
        {

            for (int i = 0; i < Faces.Count; i++)
            {
                if (Faces[i].Equals(Face))
                {
                    return;
                }
            }

            for (int j = 0; j < BoundaryFaces.Count; j++)
            {
                //Console.WriteLine(j+"\tBF:" + BoundaryFaces.Count+"\tBI:"+BoundaryFaces[j].Index+"\tFI:"+Face.Index);
                if (BoundaryFaces[j].Equals(Face))
                {

                    Faces.Add(BoundaryFaces[j]);
                    BoundaryFaces.Remove(Face);
                    //Console.WriteLine("BA:" + BoundaryFaces.Count);
                    return;
                }
            }
        }
        public void RemoveFaces(TriMesh.Face Face)
        {
            for (int j = 0; j < BoundaryFaces.Count; j++)
            {

                if (BoundaryFaces[j].Equals(Face))
                {
                    // Console.WriteLine("BF:" + BoundaryFaces.Count);
                    Faces.Add(BoundaryFaces[j]);
                    BoundaryFaces.Remove(Face);
                    //Console.WriteLine("BA:" + BoundaryFaces.Count);
                }
            }
        }
        public void AddFinalBoundaryFaces(TriMesh.Face Face)
        {
            for (int i = 0; i < FinalBoundaryFaces.Count; i++)
            {
                if (FinalBoundaryFaces[i].Equals(Face))
                {
                    return;
                }
            }
            for (int j = 0; j < BoundaryFaces.Count; j++)
            {

                if (BoundaryFaces[j].Equals(Face))
                {
                    FinalBoundaryFaces.Add(Face);
                    BoundaryFaces.Remove(Face);
                    // Console.WriteLine("final");
                }
            }


        }

        public void AddBoundaryFaces(TriMesh.Face Face)
        {
            BoundaryFaces.Add(Face);

        }

        public void SetSeedFaces(int n)
        {
            Seedface = this.mesh.Faces[n];
        }
        public void SetColor(byte i)
        {
            color = i;
        }

    }
}
