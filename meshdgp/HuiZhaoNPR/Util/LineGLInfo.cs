using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class LineGLInfo
    {
        public float[] Alpha;
        public Vector3D[] Vertex;
        public int Front;

        public LineGLInfo()
        {
            Alpha = new float[0];
            Vertex = new Vector3D[0];
        }

        public LineGLInfo(float[] alpha, Vector3D[] vertex, int front)
        {
            Alpha = (float[])alpha.Clone();
            Vertex = (Vector3D[])vertex.Clone();
            Front = front;
        }
    }
}
