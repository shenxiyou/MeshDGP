using System;
using System.Collections.Generic;

 
using System.Timers;
 
 

namespace GraphicResearchHuiZhao
{
    public class ToolPool
    {

       

        // An event that clients can use to be notified whenever the
        // elements of the list change.
        public event MeshChangedDelegate  ChangedTool;

        // Invoke the Changed event; called whenever list changes
        protected virtual void OnChanged(EventArgs e)
        {
            if (ChangedTool != null)
                ChangedTool(this, e);
        }

        private static ToolPool instance = null;
        public static ToolPool Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ToolPool();
                }
                return instance;
            }
        }

        private ToolPool()
        {
            
        }


        public TriMesh Mesh
        {
            get
            {
                return GlobalData.Instance.TriMesh;
            }

        }

        private AbstractTool tool ;
        public AbstractTool Tool
        {
            get { return tool; }
            set
            {
                if (tool != null)
                {
                    tool.Dispose();  
                }
                tool = value;
                this.tool.Changed += new AbstractTool.ChangedEventHandlerTool(tool_ChangedTool);
            }
        }

        public int Width;
        public int Height;


        public void SwitchTool(EnumTool targetToolType)
        {
            if (this.Mesh == null)
                return;

            AbstractTool targetTool = null;
            switch (targetToolType)
            {
                case EnumTool.View :
                    targetTool = new ToolView(Width, Height);
                    break;
                case EnumTool.VertexByPoint:
                    targetTool = new ToolVertexByPoint( Width,  Height, this.Mesh);
                    break;
                case EnumTool.VertexByCicle:
                    targetTool = new ToolVertexByCircle( Width,  Height, this.Mesh);
                    break;
                case EnumTool.VertexByRectangle:
                    targetTool = new ToolVertexByRectangle( Width,  Height, this.Mesh);
                    break;
                case EnumTool.VertexByCurve:
                    targetTool = new ToolPointByCurve( Width,  Height, this.Mesh);
                    break;
                case EnumTool.EdgeByCicle:
                    targetTool = new ToolEdgeByCircle( Width, Height, this.Mesh);
                    break;
                case EnumTool.EdgeByRectangle:
                    targetTool = new ToolEdgeByRectangle( Width,  Height, this.Mesh);
                    break;
                case EnumTool.EdgeByCurve:
                    targetTool = new ToolEdgeByCurve(Width, Height, this.Mesh);
                    break;
                case EnumTool.EdgeByPoint:
                    targetTool = new ToolEdgeByPoint( Width, Height, this.Mesh);
                    break;
                case EnumTool.FaceByCicle:
                    targetTool = new ToolFaceByCircle( Width,  Height, this.Mesh);
                    break;
                case EnumTool.FaceByRectangle:
                    targetTool = new ToolFaceByRectangle( Width, Height, this.Mesh);
                    break;
                case EnumTool.FaceByPoint:
                    targetTool = new ToolFaceByPoint( Width,  Height, this.Mesh);
                    break;
                case EnumTool.PointMove:
                    targetTool = new ToolMovePoint( Width,  Height, this.Mesh);
                    break; 

                case EnumTool.PointSingleMove:
                    targetTool = new ToolMoveSinglePoint(Width, Height, this.Mesh);
                    break;
                
            }

             
            this.Tool = targetTool;
            this.tool.Changed += new AbstractTool.ChangedEventHandlerTool(tool_ChangedTool);
        }

        void tool_ChangedTool(object sender, EventArgs e)
        {
            OnChanged(EventArgs.Empty);
        }

       

    }
}
