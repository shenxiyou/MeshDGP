using System;
using System.Collections.Generic; 
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public partial class TriMeshShape
    {
        public int Type = 0;
        public double GridSizeX = 0.1d;
        public double GridSizeY = 0.1d;
        public double GridSizeZ = 0.1d;

        public int SquareLength = 10;
        public int SquareWidth = 10;

        public int CylinderLength = 8;
        public int CylinderWidth = 8;
        public int CylinderHeight = 40;








        public int CircleVertex = 50;

        public int PlaneGridX=30;
        public int PlaneGridY=30;
        public double PlaneGridSizeX=0.125;
        public double PlaneGridSizeY = 0.125;

        public int ConeNum = 50;
        public double ConeHeight = Math.Sqrt(2) / 2;


        public int CylinderUVm = 50;
        public int CylinderUVn =100;
        public double CylinderUVr = 0.5;
        public double CylinderUVl = 1;
        public double CylinderUVMaxU = 2;
        public double CylinderUVMaxV = 2;
        public int CylinderUVDiff = 0;

        public int SphereNum = 50; 
        }
}
