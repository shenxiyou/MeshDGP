using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public static partial class TriMeshUtil
    {
 

        public static PrincipalCurvature[] ComputePrincipleCurvatureTwo(TriMesh mesh)
        {
            double[] CornerArea;
            double[] PointArea;

            ComputeCornerArea(mesh,out CornerArea, out PointArea);
            PrincipalCurvature[] pc= ComputePrincipleCurvatures(mesh,CornerArea, PointArea);

            return pc;

        }



        public static void ComputeCornerArea(TriMesh mesh,out double[] CornerArea, out double[] PointArea)
        {
            CornerArea = new double[mesh.HalfEdges.Count];
            PointArea = new double[mesh.Vertices.Count];

            foreach (var face in  mesh.Faces)
            {
                double area = TriMeshUtil.ComputeAreaFace(face);

                TriMesh.HalfEdge[] hf = new HalfEdgeMesh.HalfEdge[]
                {
                    face.HalfEdge,
                    face.HalfEdge.Next,
                    face.HalfEdge.Previous
                };

                int[] index = new int[3];
                for (int i = 0; i < 3; i++)
                {
                    index[i] = hf[i].Index;
                }

                Vector3D[] e = new Vector3D[3];
                for (int i = 0; i < 3; i++)
                {
                    e[i] = TriMeshUtil.GetHalfEdgeVector(hf[i].Previous);
                }

                double[] l2 = new double[3];
                for (int i = 0; i < 3; i++)
                {
                    l2[i] = e[i].LengthSquared;
                }

                double[] ew = new double[3];
                for (int i = 0; i < 3; i++)
                {
                    ew[i] = l2[i] * (l2[(i + 1) % 3] + l2[(i + 2) % 3] - l2[i]);
                }

                bool flag = false;
                for (int i = 0; i < 3; i++)
                {
                    if (ew[i] < 0d)
                    {
                        int a = (i + 1) % 3;
                        int b = (i + 2) % 3;
                        CornerArea[index[a]] = -0.25d * l2[b] * area / e[i].Dot(e[b]);
                        CornerArea[index[b]] = -0.25d * l2[a] * area / e[i].Dot(e[a]);
                        CornerArea[index[i]] = area -  CornerArea[index[a]] -  CornerArea[index[b]];
                        flag = true;
                    }
                }
                if (!flag)
                {
                    double ewscale = 0.5d * area / (ew[0] + ew[1] + ew[2]);
                    for (int i = 0; i < 3; i++)
                    {
                        CornerArea[index[i]] = ewscale * (ew[(i + 1) % 3] + ew[(i + 2) % 3]);
                    }
                }

                for (int i = 0; i < 3; i++)
                {
                     PointArea[hf[i].ToVertex.Index] += CornerArea[index[i]];
                }
            }
        }

        public static PrincipalCurvature[] ComputePrincipleCurvatures(TriMesh mesh, double[] CornerArea, double[] PointArea)
        {

            Vector3D[] vertexNormal = TriMeshUtil.ComputeNormalVertex(mesh);

            // Add dynamic trait for principle curvature computation
            double[] curv = new double[mesh.Vertices.Count];
            PrincipalCurvature[] pc = new PrincipalCurvature[mesh.Vertices.Count];

            // Initialize a coordinate system for each vertex
            foreach (TriMesh.Vertex v in mesh.Vertices)
            {
                pc[v.Index] = new PrincipalCurvature();
                // Vector that points from this vertex to an adjacent one
                pc[v.Index].maxDir = (v.HalfEdge.ToVertex.Traits.Position - v.Traits.Position).Cross(vertexNormal[v.Index]).Normalize();
                // Get a vector orthogonal to this vector and the vertex normal
                pc[v.Index].minDir = vertexNormal[v.Index].Cross(pc[v.Index].maxDir);
            }

            TriMesh.HalfEdge[] fh = new TriMesh.HalfEdge[3];
            TriMesh.Vertex[] fv = new TriMesh.Vertex[3];
            Vector3D[] e = new Vector3D[3];
            Vector3D t, b, dn, faceNormal;

            // Compute curvature for each face
            foreach (TriMesh.Face f in mesh.Faces)
            {
                // Get halfedges for this face
                fh[0] = f.HalfEdge;
                fh[1] = fh[0].Next;
                fh[2] = fh[1].Next;

                // Get vertices for this face
                fv[0] = fh[0].ToVertex;
                fv[1] = fh[1].ToVertex;
                fv[2] = fh[2].ToVertex;

                // Edge vectors
                e[0] = fv[2].Traits.Position - fv[1].Traits.Position;
                e[1] = fv[0].Traits.Position - fv[2].Traits.Position;
                e[2] = fv[1].Traits.Position - fv[0].Traits.Position;

                t = e[0];
                t.Normalize();

                faceNormal = e[0].Cross(e[1]);
                faceNormal.Normalize();

                b = faceNormal.Cross(t);
                b.Normalize();

                // Estimate curvature by variation of normals along edges
                double[] m = new double[3];
                double[,] w = new double[3, 3];

                for (int i = 0; i < 3; ++i)
                {
                    double u = e[i].Dot(t);
                    double v = e[i].Dot(b);

                    w[0, 0] += u * u;
                    w[0, 1] += u * v;
                    w[2, 2] += v * v;

                    dn = vertexNormal[fv[(i + 2) % 3].Index] - vertexNormal[fv[(i + 1) % 3].Index];

                    double dnu = dn.Dot(t);
                    double dnv = dn.Dot(b);

                    m[0] += dnu * u;
                    m[1] += dnu * v + dnv * u;
                    m[2] += dnv * v;
                }

                w[1, 1] = w[0, 0] + w[2, 2];
                w[1, 2] = w[0, 1];

                // Least squares solution
                double[] diag = new double[3];
                if (Transform.LdlTransposeDecomp(w, diag))
                {
                    Transform.LdlTransposeSolveInPlace(w, diag, m);

                    // Adjust curvature for vertices of this face
                    for (int i = 0; i < 3; ++i)
                    {
                        UV tb = new UV { U = t, V = b };
                        KUV mk = new KUV { U = m[0], UV = m[1], V = m[2] };
                        UV dst = new UV { U = pc[fv[i].Index].maxDir, V = pc[fv[i].Index].minDir };
                        KUV c = Transform.ProjectCurvature(tb, mk, dst);

                        double weight =  CornerArea[fh[i].Index] /  PointArea[fv[i].Index];
                        pc[fv[i].Index].max += weight * c.U;
                        curv[fv[i].Index] += weight * c.UV;
                        pc[fv[i].Index].min += weight * c.V;
                    }
                }
            }

            // Compute curvature for each vertex
            foreach (TriMesh.Vertex v in mesh.Vertices)
            {
                UV src = new UV { U = pc[v.Index].maxDir, V = pc[v.Index].minDir };
                KUV srcK = new KUV { U = pc[v.Index].max, UV = curv[v.Index], V = pc[v.Index].min };
                pc[v.Index] = Transform.DiagonalizeCurvature(src, srcK, vertexNormal[v.Index]);
            }

            return pc;
        }

        public static Vector4D[] ComputeDCruv(TriMesh mesh)
        {
            double[] CornerArea;
            double[] PointArea;

            ComputeCornerArea(mesh,out CornerArea, out PointArea);
            Vector4D[] DC=ComputeDCruv(mesh,CornerArea, PointArea);
            return DC;
        }

        public static Vector4D[] ComputeDCruv(TriMesh mesh, double[] CornerArea, double[] PointArea)
        {
            PrincipalCurvature[] PrincipalCurv = ComputePrincipleCurvatures(mesh, CornerArea, PointArea);

            Vector4D[] dcurv = new Vector4D[mesh.Vertices.Count];

            TriMesh.HalfEdge[] fh = new TriMesh.HalfEdge[3];
            TriMesh.Vertex[] fv = new TriMesh.Vertex[3];
            Vector3D[] e = new Vector3D[3];
            Vector3D t, b, faceNormal;

            // Compute curvature for each face
            foreach (TriMesh.Face f in mesh.Faces)
            {
                // Get halfedges for this face
                fh[0] = f.HalfEdge;
                fh[1] = fh[0].Next;
                fh[2] = fh[1].Next;

                // Get vertices for this face
                fv[0] = fh[0].ToVertex;
                fv[1] = fh[1].ToVertex;
                fv[2] = fh[2].ToVertex;

                // Edge vectors
                e[0] = fv[2].Traits.Position - fv[1].Traits.Position;
                e[1] = fv[0].Traits.Position - fv[2].Traits.Position;
                e[2] = fv[1].Traits.Position - fv[0].Traits.Position;

                t = e[0];
                t.Normalize();

                faceNormal = e[0].Cross(e[1]);

                b = faceNormal.Cross(t);
                b.Normalize();

                KUV[] fcurv = new KUV[3];
                for (int i = 0; i < 3; i++)
                {
                    PrincipalCurvature pc =  PrincipalCurv[fv[i].Index];
                    UV src = new UV { U = pc.maxDir, V = pc.minDir };
                    KUV mk = new KUV { U = pc.max, UV = 0, V = pc.min };
                    UV tb = new UV { U = t, V = b };
                    fcurv[i] = Transform.ProjectCurvature(src, mk, tb);
                }

                double[] m = new double[4];
                double[,] w = new double[4, 4];
                for (int i = 0; i < 3; i++)
                {
                    KUV prev = fcurv[(i + 2) % 3];
                    KUV next = fcurv[(i + 1) % 3];
                    KUV dfcurv = new KUV { U = prev.U - next.U, UV = prev.UV - next.UV, V = prev.V - next.V };
                    double u = e[i].Dot(t);
                    double v = e[i].Dot(b);
                    w[0, 0] += u * u;
                    w[0, 1] += u * v;
                    w[3, 3] += v * v;
                    m[0] += u * dfcurv.U;
                    m[1] += v * dfcurv.U + 2d * u * dfcurv.UV;
                    m[2] += 2d * v * dfcurv.UV + u * dfcurv.V;
                    m[3] += v * dfcurv.V;
                }
                w[1, 1] = 2d * w[0, 0] + w[3, 3];
                w[1, 2] = 2d * w[0, 1];
                w[2, 2] = w[0, 0] + 2d * w[3, 3];
                w[2, 3] = w[0, 1];

                double[] diag = new double[4];
                if (Transform.LdlTransposeDecomp(w, diag))
                {
                    Transform.LdlTransposeSolveInPlace(w, diag, m);
                    Vector4D d = new Vector4D(m);
                    // Adjust curvature for vertices of this face
                    for (int i = 0; i < 3; ++i)
                    {
                        UV tb = new UV { U = t, V = b };
                        
                        PrincipalCurvature pc =  PrincipalCurv[fv[i].Index];
                        UV dst = new UV { U = pc.maxDir, V = pc.minDir };
                        Vector4D c = Transform.ProjectDCurvature(tb, d, dst);

                        double weight =  CornerArea[fh[i].Index] / PointArea[fv[i].Index];
                        dcurv[fv[i].Index] += weight * d;
                    }
                }
            }

            return dcurv;
        }

    }
}
