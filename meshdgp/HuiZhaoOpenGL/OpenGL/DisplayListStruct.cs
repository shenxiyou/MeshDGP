using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public struct DisplayLists
    {
        #region Fields
        int mesh;
        int axes;
        int edges;
        int vertexNormals;
        int faceVertexNormals;
        int principleCurvatures;
        int boundingSphere;
        int boundingBox;
        #endregion

        #region Properties
        public int Axes
        {
            get
            {
                return axes;
            }
        }

        public int BoundingBox
        {
            get
            {
                return boundingBox;
            }
        }

        public int BoundingSphere
        {
            get
            {
                return boundingSphere;
            }
        }

        public int Count
        {
            get
            {
                return 8;
            }
        }

        public int Edges
        {
            get
            {
                return edges;
            }
        }

        public int FaceVertexNormals
        {
            get
            {
                return faceVertexNormals;
            }
        }

        public int Mesh
        {
            get
            {
                return mesh;
            }
        }

        public int PrincipleCurvatures
        {
            get
            {
                return principleCurvatures;
            }
        }

        public int VertexNormals
        {
            get
            {
                return vertexNormals;
            }
        }
        #endregion

        #region Methods
        public DisplayLists(int baseID)
        {
            mesh = baseID++;
            axes = baseID++;
            edges = baseID++;
            vertexNormals = baseID++;
            faceVertexNormals = baseID++;
            principleCurvatures = baseID++;
            boundingSphere = baseID++;
            boundingBox = baseID;
        }
        #endregion
    }
}
