using System;
using System.Collections.Generic;
using System.Text;
 

namespace GraphicResearchHuiZhao
{

    public   class TriMeshCurvature
    {


        private static TriMeshCurvature singleton = new TriMeshCurvature();


        public static TriMeshCurvature Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new TriMeshCurvature();
                return singleton;
            }
        }

        private TriMeshCurvature()
        {
        }
       
        /// <summary>
        /// Computes an approximate bounding sphere and axis-aligned bounding box.
        /// </summary>
        public void ComputeBoundingSphereAndBox(TriMesh mesh)
        {
            const float boundingEpsilon = 1e-5f;

            if (mesh.Vertices.Count == 0)
            {
                mesh.Traits.BoundingSphere.Center = new Vector3D(0, 0, 0);
                mesh.Traits.BoundingSphere.Radius = 0.0f;
            }
            else
            {
                Vector3D max = new Vector3D(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
                Vector3D min = new Vector3D(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
                foreach (TriMesh.Vertex v in mesh.Vertices)
                {
                    for (int i = 0; i < 3; ++i)
                    {
                        if (v.Traits.Position[i] > max[i])
                        {
                            max[i] = v.Traits.Position[i];
                        }
                        if (v.Traits.Position[i] < min[i])
                        {
                            min[i] = v.Traits.Position[i];
                        }
                    }
                }

                Vector3D mean = (max + min) * 0.5f;
                mesh.Traits.BoundingSphere.Center = mean;

                float distanceSquared, maxDistanceSquared = 0.0f;
                foreach (TriMesh.Vertex v in mesh.Vertices)
                {
                    distanceSquared = (float)Math.Sqrt((v.Traits.Position - mean).Length());
                    if (distanceSquared > maxDistanceSquared)
                    {
                        maxDistanceSquared = distanceSquared;
                    }
                }

                // Increase size of bounding entities slightly to prevent numerical error
                max.x += boundingEpsilon;
                max.y += boundingEpsilon;
                max.z += boundingEpsilon;
                min.x -= boundingEpsilon;
                min.y -= boundingEpsilon;
                min.z -= boundingEpsilon;
                mesh.Traits.BoundingBox.Max = max;
                mesh.Traits.BoundingBox.Min = min;
                mesh.Traits.BoundingSphere.Radius = (float)Math.Sqrt(maxDistanceSquared) + boundingEpsilon;
            }
        }

        /// <summary>
        /// Computes the angular area for each face vertex and the point area for each vertex.
        /// </summary>
        /// <param name="cornerArea">A halfedge dynamic trait to store face vertex angular areas.</param>
        /// <param name="pointArea">A vertex dynamic trait to store vertex angular areas.</param>
        public void ComputePointCornerArea(TriMesh mesh,TriMesh.HalfedgeDynamicTrait<float> cornerArea, TriMesh.VertexDynamicTrait<float> pointArea)
        {
            TriMesh.HalfEdge fh0, fh1, fh2;
            TriMesh.Vertex fv0, fv1, fv2;
            Vector3D e0, e1, e2;
            float length0, length1, length2, oneOverTotalLength;

            foreach (TriMesh.Face f in mesh.Faces)
            {
                // Get halfedges for this face
                fh0 = f.HalfEdge;
                fh1 = fh0.Next;
                fh2 = fh1.Next;

                // Get vertices for this face
                fv0 = fh0.ToVertex;
                fv1 = fh1.ToVertex;
                fv2 = fh2.ToVertex;

                // Edge vectors
                e0 = fv2.Traits.Position - fv1.Traits.Position;
                e1 = fv0.Traits.Position - fv2.Traits.Position;
                e2 = fv1.Traits.Position - fv0.Traits.Position;

                // Triangle area
                float area = (float)(0.5f * e0.Cross(e1).Length());

                // Edge lengths
                length0 = (float)e0.Length();
                length1 = (float)e1.Length();
                length2 = (float)e2.Length();

                // Approximate corner area by fraction of triangle area given by opposite edge length
                oneOverTotalLength = 1.0f / (length0 + length1 + length2);
                cornerArea[fh0] = length2 * oneOverTotalLength;
                cornerArea[fh1] = length0 * oneOverTotalLength;
                cornerArea[fh2] = length1 * oneOverTotalLength;

                // Add corner areas to areas for vertices
                pointArea[fv0] += cornerArea[fh0];
                pointArea[fv1] += cornerArea[fh1];
                pointArea[fv2] += cornerArea[fh2];
            }
        }

        //#if CURVATURE
        /// <summary>
        /// Computes principle curvatures on the vertices.
        /// </summary>
        public void ComputePrincipleCurvatures(TriMesh mesh)
        {
            TriMesh.HalfedgeDynamicTrait<float> cornerArea = new TriMesh.HalfedgeDynamicTrait<float>(mesh);
            TriMesh.VertexDynamicTrait<float> pointArea = new TriMesh.VertexDynamicTrait<float>(mesh);

            ComputePointCornerArea(mesh,cornerArea, pointArea);
            ComputePrincipleCurvatures(mesh,cornerArea, pointArea);
        }

        /// <summary>
        /// Computes principle curvatures on the vertices.
        /// </summary>
        /// <param name="cornerArea">A halfedge dynamic trait with face vertex angular areas.</param>
        /// <param name="pointArea">A vertex dynamic trait with vertex angular areas.</param>
        /// <remarks>
        /// Portions of this method are based on code from the C++ trimesh2 library
        /// (from TriMesh_curvature.cc).
        /// </remarks>
        public void ComputePrincipleCurvatures(TriMesh mesh,TriMesh.HalfedgeDynamicTrait<float> cornerArea, TriMesh.VertexDynamicTrait<float> pointArea)
        {
            // Add dynamic trait for principle curvature computation
            TriMesh.VertexDynamicTrait<float> curv = new TriMesh.VertexDynamicTrait<float>(mesh);

            // Initialize principle curvatures to zero
            foreach (TriMesh.Vertex v in mesh.Vertices)
            {
                v.Traits.MaxCurvature = 0.0f;
                v.Traits.MinCurvature = 0.0f;
            }

            // Initialize a coordinate system for each vertex
            foreach (TriMesh.Vertex v in mesh.Vertices)
            {
                if (Math.Sqrt(v.Traits.Normal.Length()) > 0.0f)    // Ignore isolated points
                {
                    // Vector that points from this vertex to an adjacent one
                    v.Traits.MaxCurvatureDirection = v.HalfEdge.ToVertex.Traits.Position - v.Traits.Position;
                    v.Traits.MaxCurvatureDirection.Normalize();

                    // Get a vector orthogonal to this vector and the vertex normal
                    v.Traits.MinCurvatureDirection = v.Traits.Normal.Cross(v.Traits.MaxCurvatureDirection);
                }
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
                float[] m = new float[3];
                float[,] w = new float[3, 3];

                for (int i = 0; i < 3; ++i)
                {
                    float u = (float)e[i].Dot(t);
                    float v = (float)e[i].Dot(b);

                    w[0, 0] += u * u;
                    w[0, 1] += u * v;
                    w[2, 2] += v * v;

                    dn = fv[(i + 2) % 3].Traits.Normal - fv[(i + 1) % 3].Traits.Normal;

                    float dnu = (float)dn.Dot(t);
                    float dnv = (float)dn.Dot(b);

                    m[0] += dnu * u;
                    m[1] += dnu * v + dnv * u;
                    m[2] += dnv * v;
                }

                w[1, 1] = w[0, 0] + w[2, 2];
                w[1, 2] = w[0, 1];

                // Least squares solution
                float[] diag = new float[3];
                if (Curvature.LdlTransposeDecomp(w, diag))
                {
                    Curvature.LdlTransposeSolveInPlace(w, diag, m);

                    // Adjust curvature for vertices of this face
                    for (int i = 0; i < 3; ++i)
                    {
                        float c1, c12, c2;
                        Curvature.ProjectCurvature(t, b, m[0], m[1], m[2], fv[i].Traits.MaxCurvatureDirection, fv[i].Traits.MinCurvatureDirection, out c1, out c12, out c2);

                        float weight = cornerArea[fh[i]] / pointArea[fv[i]];
                        fv[i].Traits.MaxCurvature += weight * c1;
                        curv[fv[i]] += weight * c12;
                        fv[i].Traits.MinCurvature += weight * c2;
                    }
                }
            }

            // Compute curvature for each vertex
            foreach (TriMesh.Vertex v in mesh.Vertices)
            {
                if (Math.Sqrt(v.Traits.Normal.Length()) > 0.0f)    // Ignore isolated points
                {
                    Curvature.DiagonalizeCurvature(v.Traits.MaxCurvatureDirection, v.Traits.MinCurvatureDirection, v.Traits.MaxCurvature, curv[v], v.Traits.MinCurvature, v.Traits.Normal,
                        out v.Traits.MaxCurvatureDirection, out v.Traits.MinCurvatureDirection, out v.Traits.MaxCurvature, out v.Traits.MinCurvature);
                }
            }
        }
        //#endif

        /// <summary>
        /// Computes vertex normals.
        /// </summary>
        public void ComputeNormals(TriMesh mesh)
        {
            TriMesh.HalfedgeDynamicTrait<float> cornerArea = new TriMesh.HalfedgeDynamicTrait<float>(mesh);
            TriMesh.VertexDynamicTrait<float> pointArea = new TriMesh.VertexDynamicTrait<float>(mesh);

            ComputePointCornerArea(mesh,cornerArea, pointArea);
            ComputeFaceVertexNormals(mesh,cornerArea, pointArea);
        }



        /// <summary>
        /// Computes vertex normals.
        /// </summary>
        /// <param name="cornerArea">A halfedge dynamic trait with face vertex angular areas.</param>
        /// <param name="pointArea">A vertex dynamic trait with vertex angular areas.</param>
        public void ComputeFaceVertexNormals(TriMesh mesh, TriMesh.HalfedgeDynamicTrait<float> cornerArea, TriMesh.VertexDynamicTrait<float> pointArea)
        {
            TriMesh.FaceDynamicTrait<Vector3D> normal = new TriMesh.FaceDynamicTrait<Vector3D>(mesh);

            TriMesh.Vertex[] fv = new TriMesh.Vertex[3];

            // Compute normal for each face
            foreach (TriMesh.Face f in mesh.Faces)
            {
                int i = 0;
                foreach (TriMesh.Vertex v in f.Vertices)
                {
                    fv[i] = v;
                    ++i;
                }

                // Compute normal for this face
                normal[f] = (fv[2].Traits.Position - fv[1].Traits.Position).Cross(fv[0].Traits.Position - fv[2].Traits.Position);
                normal[f] = normal[f].Normalize();
                f.Traits.Normal = normal[f];
            }

            // Compute normal for each vertex
            foreach (TriMesh.HalfEdge h in  mesh.HalfEdges)
            {
                if (!h.OnBoundary)  // Ignore halfedges that don't have a face
                {
                    float weight = cornerArea[h] / pointArea[h.ToVertex];
                    h.ToVertex.Traits.Normal += weight * normal[h.Face];
                }
            }

            // Normalize normals
            foreach (TriMesh.Vertex v in mesh.Vertices)
            {
                if (Math.Sqrt(v.Traits.Normal.Length()) > 0.0f)    // Ignore isolated points
                {
                    v.Traits.Normal.Normalize();
                }
            }
        }


      

      

        

    }
}
