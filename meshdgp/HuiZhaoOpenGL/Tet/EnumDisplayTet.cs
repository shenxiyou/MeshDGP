using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    [Flags]
    public enum TetDisplayFlag
    {
        None = 0x0000,
        Vertex = 0x0001,
        Edge = 0x0002,
        Face = 0x0004,
        SelectedVertex = 0x0008,
        SelectedEdge = 0x0010,
        SelectedFace = 0x0020,
        SelectedTetrahedron = 0x0040,
        HasInner = 0x0080,
        Transparent = 0x0100,
        VertexNormal = 0x0200,
        FaceNormal = 0x0400,
        SelectedVertexNormal = 0x0800,
        SelectedFaceNormal = 0x1000
    }
}
