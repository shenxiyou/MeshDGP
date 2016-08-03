using System; 
 
using OpenTK.Graphics.OpenGL;

namespace GraphicResearchHuiZhao
{
	public class OpenGLProjector 
	{
		#region Private Instance Fields
		private double[] modelView = new double[16];
		private double[] projection = new double[16];
		private int[] viewport = new int[4];
		private float[] depthBuffer = null;
		#endregion

		#region Public Properties
		public double[] ModelViewMatrix
		{
			get { return modelView; }
		}
		public double[] ProjectionMatrix 
		{
			get { return projection; }
		}
		public int[] Viewport 
		{
			get { return viewport; }
		}
		public float[] DepthBuffer 
		{
			get { return depthBuffer; }
		}

		#endregion

		public  OpenGLProjector() 
		{
			GL.GetDouble(GetPName.ModelviewMatrix,  modelView);
			GL.GetDouble(GetPName.ProjectionMatrix,  projection);
			GL.GetInteger(GetPName.Viewport, viewport);
			
			depthBuffer = new float[viewport[2] * viewport[3]];
			 
		    GL.ReadPixels(viewport[0], viewport[1], viewport[2], viewport[3], PixelFormat.DepthComponent, PixelType.Float, depthBuffer);
			 
		}

        public GraphicResearchHuiZhao.Vector3D UnProject(double inX, double inY, double inZ) 
		{  
		 
            OpenTK.Vector3 result;
            OpenTK.Graphics.Glu.UnProject(new OpenTK.Vector3((float)inX, (float)inY, (float)inZ), modelView, projection, viewport, out result);
            return new GraphicResearchHuiZhao.Vector3D(result.X,result.Y,result.Z);
		}
        public GraphicResearchHuiZhao.Vector3D UnProject(GraphicResearchHuiZhao.Vector3D p) 
		{
		 
            OpenTK.Vector3 result;
            OpenTK.Graphics.Glu.UnProject(new OpenTK.Vector3((float)p.x, (float)p.y, (float)p.z), modelView, projection, viewport, out  result);
            return new GraphicResearchHuiZhao.Vector3D(result.X, result.Y, result.Z);
		}
        public GraphicResearchHuiZhao.Vector3D UnProject(double[] arr, int index) 
		{
			 
            OpenTK.Vector3 result;
            OpenTK.Graphics.Glu.UnProject(new OpenTK.Vector3((float)arr[index], (float)arr[index + 1], (float)arr[index + 2]), modelView, projection, viewport, out result);
            return new GraphicResearchHuiZhao.Vector3D(result.X, result.Y, result.Z);
		}
        public GraphicResearchHuiZhao.Vector3D Project(double inX, double inY, double inZ) 
		{
			 
            OpenTK.Vector3 result;
            OpenTK.Graphics.Glu.Project(new OpenTK.Vector3((float)inX, (float)inY, (float)inZ), modelView, projection, viewport, out  result);
            return new GraphicResearchHuiZhao.Vector3D(result.X, result.Y, result.Z);
		}
        public GraphicResearchHuiZhao.Vector3D Project(GraphicResearchHuiZhao.Vector3D p) 
		{
			 
            OpenTK.Vector3 result;
            OpenTK.Graphics.Glu.Project(new OpenTK.Vector3((float)p.x, (float)p.y, (float)p.z), modelView, projection, viewport, out result);
            return new GraphicResearchHuiZhao.Vector3D(result.X, result.Y, result.Z);
		}
        public GraphicResearchHuiZhao.Vector3D Project(double[] arr, int index) 
		{		    
            OpenTK.Vector3 result;
            OpenTK.Graphics.Glu.Project(new OpenTK.Vector3((float)arr[index], (float)arr[index + 1], (float)arr[index + 2]), modelView, projection, viewport, out  result);
            return new GraphicResearchHuiZhao.Vector3D(result.X, result.Y, result.Z);
		}
		public double GetDepthValue(int x, int y) 
		{
			return depthBuffer[(y-viewport[1])*viewport[2] + (x-viewport[0])];
		}

       

	}
}
