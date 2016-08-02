using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public abstract class MergeSimplicationBase<TTarget> : IErrorGoalSimplication
    {
        public TriMesh Mesh;

        public MergeSimplicationBase(TriMesh mesh)
        {
            this.Mesh = mesh;
            this.Init();
        }

        public int Run(int preserveFace)
        {
            int count = 0;
            while (this.Mesh.Faces.Count > preserveFace)
            {
                MergeArgs args = this.GetMin();
                if (args == null)
                {
                    break;
                }

                this.BeforeMerge(args.Target);

                TriMesh.Vertex newVertex = this.Merge(args);

                this.AfterMerge(newVertex);

                count++;
            }

            return count;
        }

        protected abstract void Init();

        protected abstract MergeArgs GetMin();

        protected abstract TriMesh.Vertex Merge(MergeArgs args);

        protected abstract void BeforeMerge(TTarget target);

        protected abstract void AfterMerge(TriMesh.Vertex v);

        public class MergeArgs
        {
            public TTarget Target;
            public Vector3D Pos;
        }
    }
}
