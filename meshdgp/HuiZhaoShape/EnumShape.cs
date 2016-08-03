using System;
using System.Collections.Generic; 
using System.Text;

namespace GraphicResearchHuiZhao
{
    public enum EnumShapeDeform
    {
        Cylinder, CylinderV2, Plane2D, PlaneSpoke
    }

    public enum EnumShape
    {
        Triangle, Circle, Grid, Sqaure, Pentagon, Hexagon, Octagon,
        ConeDemo,Cone, Tet, Pyramid,
        Cube, HexagonalPrism, TriangularPrism, Cylinder,
        Sphere, CylinderUV, CylinderUVPlane,
        CylinderSquare,CylinderPlaneSquare,
        PlaneTest1,PlaneFolded,
       
    }

    public enum EnumShapeSel
    {
        SelectionVertex, SelectionEdge, SelectionFace,
        SelectionVertexSave, SelectionEdgeSave, SelectionFaceSave,
        SelectionALl,SelectionAllSave
    }
}

