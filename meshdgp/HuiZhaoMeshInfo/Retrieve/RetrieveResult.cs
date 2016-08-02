using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public class RetrieveResult
    {
        private static RetrieveResult singleton = new RetrieveResult();

        public static RetrieveResult Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new RetrieveResult();
                return singleton;
            }
        }

        public  Color4 VertexResult = new Color4(0, 0, 255);
        public Color4 EdgeResult = new Color4(255, 140, 105);
        public Color4 FaceResult = Color4.Purple;
    }
}
