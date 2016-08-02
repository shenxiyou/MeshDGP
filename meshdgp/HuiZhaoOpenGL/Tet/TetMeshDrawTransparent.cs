using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicResearchHuiZhao
{
    public partial class OpenGLTetMesh
    {
        public void BeginTransparent(TetMesh mesh)
        {
            //GL.Enable(EnableCap.CullFace);
            //GL.CullFace(CullFaceMode.Back);
            GL.DepthMask(false);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(GlobalSetting.BlendSetting.BlendingFactorSrc, GlobalSetting.BlendSetting.BlendingFactorDest);
            GL.BlendEquation(GlobalSetting.BlendSetting.BlendEquationMode);
            GL.BlendColor(GlobalSetting.BlendSetting.BlendColor);
        }

        public void EndTransparent(TetMesh mesh)
        {
            GL.Disable(EnableCap.Blend);
            GL.DepthMask(true);
        }

        public void DrawTransparent(TetMesh mesh)
        {
            GlobalSetting.SettingLight.DoubleSideLight = true;
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Disable(EnableCap.ColorMaterial);

            float alpha = GlobalSetting.MaterialSetting.MaterialDiffuse.A / 256f;
            float f = 1 - alpha;

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Front);
            GL.DepthFunc(DepthFunction.Always);
            this.DrawTransparentMesh(mesh, f * alpha);
            GL.DepthFunc(DepthFunction.Lequal);
            this.DrawTransparentMesh(mesh, (alpha - f * alpha) / (1f - f * alpha));

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.DepthFunc(DepthFunction.Always);
            this.DrawTransparentMesh(mesh, f * alpha);
            GL.DepthFunc(DepthFunction.Lequal);
            this.DrawTransparentMesh(mesh, (alpha - f * alpha) / (1f - f * alpha));

            GL.Disable(EnableCap.Blend);
            GL.DepthMask(true);
            GL.Disable(EnableCap.DepthTest);
        }

        void DrawTransparentMesh(TetMesh mesh, float alpha)
        {
            OpenTK.Graphics.Color4 front = GlobalSetting.MaterialSetting.MaterialDiffuse;
            front.A = alpha;
            OpenTK.Graphics.Color4 back = GlobalSetting.MaterialSetting.BackMaterialDiffuse;
            back.A = alpha;
            GL.Material(MaterialFace.Front, MaterialParameter.Diffuse, front);
            GL.Material(MaterialFace.Back, MaterialParameter.Diffuse, back);
        }
    }
}
