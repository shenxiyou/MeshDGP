using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing; 

namespace GraphicResearchHuiZhao
{
 

    public class ToolCurveComplex : ToolMoveSinglePoint
    {  

       private TriMesh curve = null;
       public ToolCurveComplex(double width, double height, TriMesh mesh)
            : base(width,height, mesh)
        {
             
        }

       public override void MouseMove(Vector2D mouseMovePos, EnumMouseButton button)
       {
           base.MouseMove(mouseMovePos,button);

           switch (ParameterCurve.Instance.currentCurve)
           {
               case EnumCurveComplex.FourPointBezier :
                   curve = ParameterCurve.Instance.CreateFourBezierCurve(mesh);
                   break;
               case EnumCurveComplex.ThreePointBezier:
                   curve = ParameterCurve.Instance.CreateThreeBezierCurve(mesh);
                   break;
               case EnumCurveComplex.NPointBezier:
                   curve = ParameterCurve.Instance.CreateNBezierCurve(mesh);
                   break;
               case EnumCurveComplex.FourPointBSpline:
                   curve = ParameterCurve.Instance.CreateFourPointBSplineCurve(mesh);
                   break;
               case EnumCurveComplex.NPointBSpline:
                   curve = ParameterCurve.Instance.CreateNBsplineCurve(mesh);
                   break;
               case EnumCurveComplex.NURBS:
                   curve = ParameterCurve.Instance.CreateNURBS(mesh);
                   break;
               case EnumCurveComplex.NURBSCicle:
                   curve = ParameterCurve.Instance.CreateNURBSCicle(mesh);
                   break;
               case EnumCurveComplex.NURBSEllipse:
                   curve = ParameterCurve.Instance.CreateNURBSEllipse(mesh);
                   break;
           }
          
       }

       public override void Draw()
       {
           base.Draw();
           if (curve != null)
           {

               OpenGLTriMesh.Instance.DrawCurve(curve, Color.Green);
           }
           

       }

    }
}
