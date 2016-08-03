using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicResearchHuiZhao 
{
    public partial class OpenGLManager     
    {
        public void SetClipPlane()
        {
            if (GlobalSetting.ClipPlaneSetting.Enable0)
            {
                GL.Enable(EnableCap.ClipPlane0);
                GL.ClipPlane(ClipPlaneName.ClipPlane0, GlobalSetting.ClipPlaneSetting.Plane0.ToArray());
            }
            else
            {
                GL.Disable(EnableCap.ClipPlane0);
            }
            if (GlobalSetting.ClipPlaneSetting.Enable1)
            {
                GL.Enable(EnableCap.ClipPlane1);
                GL.ClipPlane(ClipPlaneName.ClipPlane1, GlobalSetting.ClipPlaneSetting.Plane1.ToArray());
            }
            else
            {
                GL.Disable(EnableCap.ClipPlane1);
            }
            if (GlobalSetting.ClipPlaneSetting.Enable2)
            {
                GL.Enable(EnableCap.ClipPlane2);
                GL.ClipPlane(ClipPlaneName.ClipPlane2, GlobalSetting.ClipPlaneSetting.Plane2.ToArray());
            }
            else
            {
                GL.Disable(EnableCap.ClipPlane2);
            }
            if (GlobalSetting.ClipPlaneSetting.Enable3)
            {
                GL.Enable(EnableCap.ClipPlane3);
                GL.ClipPlane(ClipPlaneName.ClipPlane3, GlobalSetting.ClipPlaneSetting.Plane3.ToArray());
            }
            else
            {
                GL.Disable(EnableCap.ClipPlane3);
            }
            if (GlobalSetting.ClipPlaneSetting.Enable4)
            {
                GL.Enable(EnableCap.ClipPlane4);
                GL.ClipPlane(ClipPlaneName.ClipPlane4, GlobalSetting.ClipPlaneSetting.Plane4.ToArray());
            }
            else
            {
                GL.Disable(EnableCap.ClipPlane4);
            }
            if (GlobalSetting.ClipPlaneSetting.Enable5)
            {
                GL.Enable(EnableCap.ClipPlane5);
                GL.ClipPlane(ClipPlaneName.ClipPlane5, GlobalSetting.ClipPlaneSetting.Plane5.ToArray());
            }
            else
            {
                GL.Disable(EnableCap.ClipPlane5);
            }
        }
	}
}