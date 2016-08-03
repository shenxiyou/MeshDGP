using System;

using OpenTK;

namespace GraphicResearchHuiZhao
{
    public struct VertexT2dN3dV3d
    {
        public OpenTK.Vector2d TexCoord;
        public OpenTK.Vector3d Normal;
        public OpenTK.Vector3d Position;

        public VertexT2dN3dV3d(OpenTK.Vector2d texcoord, Vector3d normal, Vector3d position)
        {
            TexCoord = texcoord;
            Normal = normal;
            Position = position;
        }
    }

    public struct VertexT2fN3fV3f
    {
        public OpenTK.Vector2 TexCoord;
        public OpenTK.Vector3 Normal;
        public OpenTK.Vector3 Position;
    }

    public struct VertexT2hN3hV3h
    {
        public OpenTK.Vector2h TexCoord;
        public OpenTK.Vector3h Normal;
        public OpenTK.Vector3h Position;
    }

   
}
