using System;
using System.Collections.Generic;





namespace GraphicResearchHuiZhao
{
    public class ToolMoveSinglePoint : ToolBaseTriMesh
    {
        private Vector3D projectedCenter;
        protected int selectedPoint = -1;
        private ArcBall movingBall;
        private Vector4D handleCenter;

        private List<int> handleIndex = new List<int>();
        private List<Vector3D> oldHandlePos = new List<Vector3D>();


        public ToolMoveSinglePoint(double width, double height, TriMesh mesh)
            : base(width, height, mesh)
        {
            movingBall = new ArcBall(200, 200);
        }



        public override void MouseDown(Vector2D mouseDownPos, EnumMouseButton button)
        {
            if (mesh == null)
                return;
            base.MouseDown(mouseDownPos, button);

            bool haveSelected = false;
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                if (mesh.Vertices[i].Traits.SelectedFlag == (byte)1) haveSelected = true;
            }


            if (button == EnumMouseButton.Left && !haveSelected)
            {
                selectedPoint = SelectByPoint();
            }


            if (StartMoving() == false) return;
            Vector2D p = mouseDownPos - new Vector2D(projectedCenter.x, projectedCenter.y);
            p.x += 100;
            p.y += 100;

            switch (button)
            {
                case EnumMouseButton.Middle: movingBall.Click(p, ArcBall.MotionType.Rotation); break;
                case EnumMouseButton.Left: movingBall.Click(p / this.ScaleRatio, ArcBall.MotionType.Pan); break;
                case EnumMouseButton.Right: movingBall.Click(p, ArcBall.MotionType.Scale); break;
            }

            TriMeshUtil.GroupVertice(mesh);
            OnChanged(EventArgs.Empty);

        }

        public override void MouseMove(Vector2D mouseMovePos, EnumMouseButton button)
        {


            base.MouseMove(mouseMovePos, button);

            if (handleIndex == null) return;
            Vector2D p = mouseCurrPos - new Vector2D(projectedCenter.x, projectedCenter.y);
            p.x += 100;
            p.y += 100;
            switch (button)
            {
                case EnumMouseButton.Middle: movingBall.Drag(p); break;
                case EnumMouseButton.Left: movingBall.Drag(p / this.ScaleRatio); break;
                case EnumMouseButton.Right: movingBall.Drag(p); break;
            }
            Matrix4D tran = TransformController.Instance.TransformInverse * movingBall.CreateMatrix().Transpose() * TransformController.Instance.ModelMatrix;

            for (int i = 0; i < handleIndex.Count; i++)
            {
                int j = handleIndex[i];
                Vector4D q = new Vector4D((Vector3D)oldHandlePos[i], 1);
                q = tran * (q - handleCenter) + handleCenter;
                mesh.Vertices[j].Traits.Position.x = q.x;
                mesh.Vertices[j].Traits.Position.y = q.y;
                mesh.Vertices[j].Traits.Position.z = q.z;
            }

            //TriMeshUtil.SetUpNormalVertex(mesh);
            OnChanged(EventArgs.Empty);
        }

        public override void MouseUp(Vector2D mouseUpPos, EnumMouseButton button)
        {
            if (mesh == null)
                return;

            base.MouseUp(mouseUpPos, button);

            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                mesh.Vertices[i].Traits.SelectedFlag = (byte)0;
            }

            if (handleIndex == null) return;
            movingBall.End();

            OnChanged(EventArgs.Empty);
        }



        private void MovePoint()
        {

        }

        protected virtual int SelectByPoint()
        {
            return SelectVertexByPoint(false);
        }



        public bool StartMoving()
        {
            // find closest selected vertex
            OpenGLProjector projector = new OpenGLProjector();
            TriMesh m = this.mesh;

            CRectangle viewport = new CRectangle(0, 0, Width, Height);
            double eps = ToolSetting.ToolsSetting.DepthTolerance;
            double minDis = double.MaxValue;
            int minIndex = -1;

            for (int i = 0; i < m.Vertices.Count; i++)
            {
                if (m.Vertices[i].Traits.SelectedFlag == 0) continue;

                Vector3D v3d = projector.Project(m.Vertices[i].Traits.Position);
                Vector2D v = new Vector2D(v3d.x, v3d.y);

                if (!viewport.Contains((int)v.x, (int)v.y)) continue;
                if (projector.GetDepthValue((int)v.x, (int)v.y) - v3d.z < eps) continue;

                double dis = (v - mouseDownPos).Length();
                if (dis < minDis)
                {
                    minDis = dis;
                    minIndex = i;
                }
            }

            if (minIndex == -1)
            {
                handleIndex = null;
                oldHandlePos.Clear();
                return false;
            }

            // find boundary box
            int flag = m.Vertices[minIndex].Traits.SelectedFlag;
            if (handleIndex == null)
                return false;
            handleIndex.Clear();
            oldHandlePos.Clear();

            Vector3D c = new Vector3D(0, 0, 0);
            for (int i = 0; i < m.Vertices.Count; i++)
                if (m.Vertices[i].Traits.SelectedFlag == flag)
                {
                    Vector3D p = new Vector3D(m.Vertices[i].Traits.Position.x, m.Vertices[i].Traits.Position.y, m.Vertices[i].Traits.Position.z);
                    handleIndex.Add(i);
                    oldHandlePos.Add(p);
                    c += p;
                }

            c /= (double)handleIndex.Count;
            handleCenter = new Vector4D(c, 0);
            projectedCenter = projector.Project(handleCenter.x, handleCenter.y, handleCenter.z);
            return true;

        }

        public override void Draw()
        {

            OpenGLTriMesh.Instance.DrawSelectedVerticeBySphere(mesh);

        }

    }
}
