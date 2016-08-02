using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public partial class OpenGLManager
    {
        public void RenderData()
        {
            if (GlobalData.Instance.TriMesh != null)
            {
                OpenGLTriMesh.Instance.DrawTriMesh(GlobalData.Instance.TriMesh);

            }

            if (GlobalData.Instance.QuadMesh != null)
            {
                OpenGLTriMesh.Instance.DrawHalfEdgeQuadMesh(GlobalData.Instance.QuadMesh);
            }

            MeshRecord currMeshRecord = GlobalData.Instance.SimpleMesh;
            if (currMeshRecord != null)
            {

                OpenGLNonManifold.Instance.DrawMesh(currMeshRecord.Mesh);

            }

            if (GlobalData.Instance.TetMesh != null)
            {
                OpenGLTetMesh.Instance.DrawFlag(GlobalData.Instance.TetMesh);
            }
            if (GlobalData.Instance.ske != null)
            {
                OpenGLTriMesh.Instance.DrawSkeleton(GlobalData.Instance.ske);
            }

            if (GlobalData.Instance.boxes != null)
            {
                OpenGLOctree.Instance.DrawDynamicOctree(GlobalData.Instance.boxes);
            }
        }
    }
}
