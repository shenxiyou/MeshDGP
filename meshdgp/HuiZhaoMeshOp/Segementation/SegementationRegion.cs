using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class SegementationRegion:SegmentationBase
    {
        public SegementationRegion(TriMesh mesh)
            : base(mesh)
        {
        }


        public override void Run()
        {
            
            Vector3D[] Av = new Vector3D[this.mesh.Faces.Count];
            Vector3D[] E = new Vector3D[this.mesh.Faces.Count];
            double[] D = new double[this.mesh.Faces.Count];
            Vector3D[] AI = new Vector3D[this.mesh.Faces.Count];
            List<Region> region = new List<Region>();
            Vector3D max;
            Vector3D min;

            double[] s = new double[this.mesh.Faces.Count];


            int k = 0;
            int[] facenum = new int[this.mesh.Faces.Count];
            for (int i = 0; i < this.mesh.Faces.Count; i++)
            {
                facenum[i] = -1;
                foreach (TriMesh.Face face in mesh.Faces[i].Faces)
                {
                    double a =TriMeshUtil.ComputeEdgeLength(face.GetVertex(0), face.GetVertex(1));

                    double b = TriMeshUtil.ComputeEdgeLength(face.GetVertex(0), face.GetVertex(2));
                    double c = TriMeshUtil.ComputeEdgeLength(face.GetVertex(2), face.GetVertex(1));
                    double p = (a + b + c) / 2;
                    s[i] = Math.Pow(p * (p - a) * (p - b) * (p - c), 0.5);

                    Av[k] = (face.Traits.Normal - mesh.Faces[i].Traits.Normal) * s[i];
                    k++;
                    E[i] = E[i] + Av[i] / (s[i] * k);


                }

                max = Av[0];
                min = Av[0];
                for (int j = 1; j < k; j++)
                {

                    if (max.Length() < Av[j].Length())
                    {
                        max = Av[j];
                    }
                    if (min.Length() > Av[j].Length())
                    {
                        min = Av[j];
                    }

                }
                AI[i] = max - min;
                for (int j = 1; j <= k; j++)
                {
                    E[i] = E[i] + Av[i] / (s[i] * j);
                    D[i] = (Av[i] - E[i]).Length() / j;
                }
                k = 0;
            }


            for (int i = 0; i < this.mesh.Faces.Count; i++)
            {


               
                if ((Av[i].Length() < 0.86) && (D[i] < 0.098))
                {


                    facenum[k] = i;

                    k++;
                }
            }

            for (int i = 0; i < k - 1; i++)
            {
                int j;
                int index = i;
                for (int m = i + 1; m < k; m++)
                {
                    if (E[facenum[m]].Length() < E[facenum[index]].Length())
                    {
                        j = facenum[m];
                        facenum[m] = facenum[index];
                        facenum[index] = j;
                    }
                }
            }
            // int j = 0;
            byte color = 1;

            for (int m = 0; m < k; m++)
            {

                int test = 0;
                Vector3D Normal;
               
                Region mutiface = new Region(this.mesh);
                double sum;
                if (mesh.Faces[facenum[m]].Traits.SelectedFlag == 0)
                {
                    mesh.Faces[facenum[m]].Traits.SelectedFlag = color;
                    mutiface.SetSeedFaces(facenum[m]);
                    mutiface.AddBoundaryFaces(mesh.Faces[facenum[m]]);
                    mutiface.SetColor(color);
                    Normal = mesh.Faces[facenum[m]].Traits.Normal;
                    sum = s[facenum[m]];
                }
                else
                {
                    continue;
                }
                Boolean n = true;

                for (; n; )
                {
                    // n = true;
                    test = mutiface.Getallfacenumber();
                    List<TriMesh.Face> selectedFaces = new List<TriMesh.Face>();
                 
                    for (int i = 0; i < mutiface.getBoundaryFaces().Count; i++)
                    {
                        selectedFaces.Add(mutiface.getBoundaryFaces()[i]);
                    }

                 

                    for (int i = 0; i < selectedFaces.Count; i++)
                    {
                        int test2 = 0;
                    
                        foreach (TriMesh.Face face in selectedFaces[i].Faces)
                        {

                            if (face.Traits.SelectedFlag == 0 && (Normal - selectedFaces[i].Traits.Normal).Length() * s[facenum[0]] < 0.481 && (face.Traits.Normal - Normal).Length() * sum < 0.72)
                            {

                                mutiface.AddFaces(selectedFaces[i], i);
                          

                                mutiface.AddBoundaryFaces(face);
                 
                                face.Traits.SelectedFlag = selectedFaces[i].Traits.SelectedFlag;
                                Normal = Normal + face.Traits.Normal;
                                sum = sum + s[face.Index];

                                test2++;
                            }
                        }


                        Normal = Normal / (test + 1);
                        sum = sum / (test + 1);
                        if (test2 == 0)
                        {
                            mutiface.AddFinalBoundaryFaces(selectedFaces[i]);

                            

                        }
                      

                    }
                    if (test == mutiface.Getallfacenumber())
                    {
                      
                        n = false;
                    }
                    else if (mutiface.Getallfacenumber() >= 2200)
                    {
                    
                        n = false;
                    }


                }


                region.Add(mutiface);
             
                color++;
            }  

        }
    }
}
