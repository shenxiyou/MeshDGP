////using System;
////using System.Collections.Generic;
////using System.Text;

////namespace GraphicResearchHuiZhao
////{
////    public class LineBase
////    {
////        public TriMesh mesh = null;
////        public  Vector3D viewPoint;
////        public LineBase(TriMesh mesh)
////        {
////            this.mesh = mesh;
////        }

////        public virtual TriMesh Extract()
////        {
////            TriMesh mesh = new TriMesh();
////            return mesh;
////        }

////        public Vector3D ComputeViewPoint()
////        { 
////            return ToolPool.Instance.Tool.ComputeViewPoint();
////        }
////    }
////}
