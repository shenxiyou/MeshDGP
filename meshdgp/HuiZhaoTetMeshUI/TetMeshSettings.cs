using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Design;

namespace GraphicResearchHuiZhao
{
    public class TetMeshSettings
    {
        Plane plane = new Plane(1, 0, 0, 0);

        [Editor(typeof(PlaneEditor), typeof(UITypeEditor))]
        [Range(-1, 1, 0.01)]
        public Plane ClipPlane
        {
            get { return plane; }
            set
            {
                plane = value;
                this.ChangePlane();
            }
        }

        bool intersecting = true;
        public bool Intersecting
        {
            get { return intersecting; }
            set { intersecting = value; }
        }

        void ChangePlane()
        {
            TetMesh mesh = GlobalData.Instance.TetMesh;
            foreach (var tet in mesh.Tetras)
            {
                PlaneIntersectionType intersect = TetMeshUtil.Intersect(this.plane, tet);
                if (intersect == PlaneIntersectionType.Front ||
                    (intersecting && intersect == PlaneIntersectionType.Intersecting))
                {
                    tet.Flag = 1;
                }
                else
                {
                    tet.Flag = 0;
                }
            }
            mesh.ComputeSelectedNormal();
            GlobalData.Instance.OnChanged(EventArgs.Empty);
        }
    }
}
