using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{

    public class OctTree<T>
    {

        public int TreeDepth
        {
            get
            {
                return 0;
            }
        }

        public int CellCount
        {
            get
            {
                return 0;
            }
        }




        public Cell<T> Root;

        public BindBox Box;

        public OctTree(Vector3D position, double initSize)
        {
            Box.x = position.x;
            Box.y = position.y;
            Box.z = position.z;

            Box.xSize = Box.ySize = Box.zSize = initSize;
        }

        //public Cell FindCellWithPosition(double x,double y,double z);



        public void BuildStaticOctree(int treeDepth, List<Vector3D> points)
        {

        }



        public void BuildDynamicOctree(int treeDepth, List<Vector3D> points)
        {
        }

        public void BuildDynamicOctree(List<Vector3D> points)
        {
        }


        public Cell<T> FindCell(Vector3D position)
        {
            Cell<T> cell = new Cell<T>();

            return cell;
        }

        public List<Cell<T>> FindNeighbors(Cell<T> cell)
        {
            List<Cell<T>> cells = new List<Cell<T>>();
            return cells;
        }

    }

}
