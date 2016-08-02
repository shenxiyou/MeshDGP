using System;
using System.Collections.Generic;
using System.Text;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicResearchHuiZhao 
{
    public partial class OpenGLTriMesh
    {
        public void DrawNPR(TriMesh mesh)
        {
            DrawSelectedEdgeOnly(mesh);
           // DrawSmoothHiddenLine(mesh);
        }

        // Draw exterior silhouette of the mesh: this just draws
// thick contours, which are partially hidden by the mesh.
// Note: this needs to happen *before* draw_base_mesh...
        public void draw_silhouette( List<float> ndotv)
        {
            GL.DepthMask(false);
           // Vector3 currcolor = Vector3(0.0, 0.0, 0.0);

            GL.LineWidth(6);
            GL.Begin(BeginMode.Lines);


            GL.End();

            GL.Enable(EnableCap.PointSmooth);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.PointSize(6);
            GL.Begin(BeginMode.Points);

            GL.End();
            GL.Disable(EnableCap.PointSmooth);
            GL.Disable(EnableCap.Blend);
            GL.DepthMask(true);
      

            

          //  currcolor = vec(0.0, 0.0, 0.0);
          //  glLineWidth(6);
          //  glBegin(GL_LINES);
          ////  draw_isolines(ndotv, vector<float>(), vector<float>(), ndotv,
          //            false, false, false, 0.0f);
          //  glEnd();

          //  // Wide lines are gappy, so fill them in
          //  glEnable(GL_POINT_SMOOTH);
          //  glEnable(GL_BLEND);
          //  glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
          //  glPointSize(6);
          //  glBegin(GL_POINTS);
          //  draw_isolines(ndotv, vector<float>(), vector<float>(), ndotv,
          //            false, false, false, 0.0f);
          //  glEnd();

          //  glDisable(GL_POINT_SMOOTH);
          //  glDisable(GL_BLEND);
          //  glDepthMask(GL_TRUE);
        }


 
    }
}
