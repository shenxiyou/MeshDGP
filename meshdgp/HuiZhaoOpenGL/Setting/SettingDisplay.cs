using System;
using System.Drawing;

 

namespace GraphicResearchHuiZhao
{
    
    public class  DisplaySetting  
    {   

        private string name = "";
        private EnumDisplayMode meshDisplayMode = EnumDisplayMode.Basic;
   
        private float pointSize = 4.0f;
        private float lineWidth = 1.0f;
        private float selectionLineWidth = 0.001f;
        private bool antialiasing = false;
 
        private bool renderBoundary = true;
        private Color meshColor = Color.CornflowerBlue;
        private Color backgroundColor = Color.Transparent;
        private float boundaryLineWidth = 4.0f;
        private Color boundaryColor = Color.Red ;

        private Color selectedVertexColor = Color.Red;
        private Color selectedEdgeColor = Color.Green;
        private Color selectedFaceColor = Color.Yellow;



        private Color wireFrameColor = Color.Gray;
        
        private Color firstPricipalColor = Color.Red;
        private Color secondPricipalColor = Color.Blue;

        private Color normalColor = Color.Red;

        private Color dualColor = Color.Yellow;


        private double scaleSphere = 0.01;

        public double ScaleSphere
        {
            get
            {
                return scaleSphere;
            }
            set
            {
                scaleSphere = value;
            }
        }



        private double normalLength = 0.5;

        public double NormalLength
        {
            get
            {
                return normalLength;
            }
            set
            {
                normalLength = value;
            }
        }

        public EnumDisplayMode DisplayMode
        {
            get { return meshDisplayMode; }
            set { meshDisplayMode = value; }
        }

        private EnumOpenGLDemo openGLDemo;
        public EnumOpenGLDemo OpenGLDemo
        {
            get { return openGLDemo; }
            set { openGLDemo = value; }
        }

        

        public float PointSize
        {
            get { return pointSize; }
            set { pointSize = value; }
        }

        public float BoundaryLineWidth
        {
            get { return boundaryLineWidth; }
            set { boundaryLineWidth = value; }
        }
        public float LineWidth
        {
            get { return lineWidth; }
            set { lineWidth = value; }
        }

        public float SelectionLineWidth
        {
            get { return selectionLineWidth; }
            set { selectionLineWidth = value; }
        }

        public bool Antialiasing
        {
            get { return antialiasing; }
            set
            {
                antialiasing = value; 
            }
        }
       
        public bool RenderBoundary
        {
            get { return renderBoundary; }
            set { renderBoundary = value; }
        }
        public Color MeshColor
        {
            get { return meshColor; }
            set { meshColor = value; }
        }

        public Color BoundaryColor
        {
            get { return boundaryColor; }
            set { boundaryColor = value; }
        }

        public Color SelectedEdgeColor
        {
            get { return selectedEdgeColor; }
            set { selectedEdgeColor = value; }
        }

        public Color SelectedVertexColor
        {
            get { return selectedVertexColor; }
            set { selectedVertexColor = value; }
        }

        public Color SelectedFaceColor
        {
            get { return selectedFaceColor; }
            set { selectedFaceColor = value; }
        }

        public Color WifeFrameColor
        {
            get { return wireFrameColor; }
            set { wireFrameColor = value; }
        }


        public Color FirstPricipalColor
        {
            get { return firstPricipalColor; }
            set { firstPricipalColor = value; }
        }

        public Color SecondPricipalColor
        {
            get { return secondPricipalColor; }
            set { secondPricipalColor = value; }
        }

        public Color NormalColor
        {
            get { return normalColor; }
            set { normalColor = value; }
        }

        public Color DualColor
        {
            get { return dualColor; }
            set { dualColor = value; }
        }


        public Color BackGroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }

        public Color TetrahedronColor
        {
            get { return tetrahedronColor; }
            set { tetrahedronColor = value; }
        }

        
        public int Alpha
        {
            get { return alpha; }
            set
            {
                alpha = value;
                ChangeAlpha();
                GlobalData.Instance.OnChanged(EventArgs.Empty);
            }
        }

        public DisplaySetting(string name)
        {
            this.name = name;
            ChangeAlpha();
        }
        public override string ToString()
        {
            return name;
        }

        public void ChangeAlpha()
        {
            meshColor = ChangeAlpha(meshColor, alpha);
            wireFrameColor = ChangeAlpha(wireFrameColor, alpha);
        }

        public static Color ChangeAlpha(Color color, int alpha)
        {
            int argb = color.ToArgb();
            return Color.FromArgb(argb & 0x00FFFFFF | (alpha << 24));
        }

        private int alpha = 100;
        private Color tetrahedronColor = Color.Red;
    }
  
}
