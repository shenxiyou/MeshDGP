using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class TriMeshSimplifyController
    {


        public static int Simplify(TriMesh mesh, EnumSimplicationType type, int preservedFace)
        {
            int i = 0;
            switch (type)
            {
                case EnumSimplicationType.CornerCutting:
                    CornerCuttingSimplication corner = new CornerCuttingSimplication(mesh);
                    i = corner.Run(preservedFace);
                    break;
                case EnumSimplicationType.LengthAngle:
                    LengthAndAngleSimplication lenghtAngle = new LengthAndAngleSimplication(mesh);
                    i = lenghtAngle.Run(preservedFace);
                    break;
                case EnumSimplicationType.MinCurvature:
                    MinCurvatureSimplication minCurvature = new MinCurvatureSimplication(mesh);
                    i = minCurvature.Run(preservedFace);
                    break;
                case EnumSimplicationType.QEM:
                    QEMSimplication qem = new QEMSimplication(mesh);
                    i = qem.Run(preservedFace);
                    break;
                case EnumSimplicationType.ShortEdge:
                    ShortEdgeSimplication shortedge = new ShortEdgeSimplication(mesh);
                    i = shortedge.Run(preservedFace);
                    break;
                case EnumSimplicationType.SmallFace:
                    SmallFaceSimplication smallFace = new SmallFaceSimplication(mesh);
                    i = smallFace.Run(preservedFace);
                    break;
                case EnumSimplicationType.SquareVolume:
                    SquareVolumeSimplication squareVolume = new SquareVolumeSimplication(mesh);
                    i = squareVolume.Run(preservedFace);
                    break;
                case EnumSimplicationType.SurfaceFitting:
                    SurfaceFittingSimplication surfaceFitting = new SurfaceFittingSimplication(mesh);
                    i = surfaceFitting.Run(preservedFace);
                    break;


            }
            TriMeshUtil.FixIndex(mesh);
            TriMeshUtil.SetUpNormalVertex(mesh);
            return i;
        }


    }
}
