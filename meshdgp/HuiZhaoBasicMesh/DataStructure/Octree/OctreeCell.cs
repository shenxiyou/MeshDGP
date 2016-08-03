using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class Cell<T>
    {
        public Cell<T> Parent;
        public Cell<T>[] Childrens;

        //For marching cube
        public double[] CornerValues;

        public int IN;

        public BindBox Box;

        public Vector2D XRange;
        public Vector2D YRange;
        public Vector2D ZRange;

        public T Object;

        public int Depth = 0;

        public Vector3D FindCornerPosition(int i)
        {

            if (i > 7)
            {
                return Vector3D.Zero;
            }

            short xFlag = 0;
            short yFlag = 0;
            short zFlag = 0;

            if (i == 1 || i == 2 || i == 5 || i == 6)
            {
                xFlag = 1;
            }

            if (i == 2 || i == 3 || i == 6 || i == 7)
            {
                yFlag = 1;
            }

            if ((i & 4) == 4) //X upper
            {
                zFlag = 1;
            }

            return new Vector3D(Box.x + xFlag * Box.xSize, Box.y + yFlag * Box.ySize, Box.z + zFlag * Box.zSize);
        }

        public void AddChild()
        {
            Childrens = new Cell<T>[8];

            for (int i = 0; i < Childrens.Length; i++)
            {
                Childrens[i] = new Cell<T>();
                Childrens[i].Depth = Depth + 1;
            }

            double xMidel = Box.x + Box.xSize / 2;
            double yMidel = Box.y + Box.ySize / 2;
            double zMidel = Box.z + Box.zSize / 2;

            for (int i = 0; i < 8; i += 4)
            {
                Childrens[i].Box.x = Box.x;
                Childrens[i].Box.xSize = Box.xSize / 2;
                Childrens[i].Box.y = Box.y;
                Childrens[i].Box.ySize = Box.ySize / 2;

                Childrens[i + 1].Box.x = xMidel;
                Childrens[i + 1].Box.xSize = Box.xSize / 2;
                Childrens[i + 1].Box.y = Box.y;
                Childrens[i + 1].Box.ySize = Box.ySize / 2;

                Childrens[i + 2].Box.x = Box.x;
                Childrens[i + 2].Box.xSize = Box.xSize / 2;
                Childrens[i + 2].Box.y = yMidel;
                Childrens[i + 2].Box.ySize = Box.ySize / 2;

                Childrens[i + 3].Box.x = xMidel;
                Childrens[i + 3].Box.xSize = Box.xSize / 2;
                Childrens[i + 3].Box.y = yMidel;
                Childrens[i + 3].Box.ySize = Box.ySize / 2;

            }

            for (int i = 0; i < 4; i++)
            {
                Childrens[i].Box.z = Box.z;
                Childrens[i].Box.zSize = Box.zSize / 2;

                Childrens[i + 4].Box.z = zMidel;
                Childrens[i + 4].Box.zSize = Box.zSize / 2;

            }

            for (int i = 0; i < 8; i++)
            {
                Childrens[i].Parent = this;
            }

        }

        public int FindIndex(Vector3D position)
        {
            short xflag = 0;
            short yflag = 0;
            short zflag = 0;

            double xMidel = Box.x + Box.xSize / 2;
            double yMidel = Box.y + Box.ySize / 2;
            double zMidel = Box.z + Box.zSize / 2;

            if (xMidel < position.x)
            {
                xflag = 1;  //Upper
            }
            if (yMidel < position.y)
            {
                yflag = 1;  //Upper
            }
            if (zMidel < position.z)
            {
                zflag = 1;  //Upper
            }

            int index = 0;
            if (xflag == 1)
            {
                index |= 1;
            }
            if (yflag == 1)
            {
                index |= 2;
            }
            if (zflag == 1)
            {
                index |= 4;
            }

            return index;
        }

        public Cell<T> AddChild(Vector3D position, out bool flag)
        {
            int index = FindIndex(position);

            //Console.WriteLine(index);
            if (Childrens == null)
            {
                Childrens = new Cell<T>[8];
            }

            if (Childrens[index] != null)
            {
                flag = false;
                return Childrens[index];
            }

            flag = true;
            short xflag = 0;
            short yflag = 0;
            short zflag = 0;

            if ((index & 1) == 1)
            {
                xflag = 1;
            }
            if ((index & 2) == 2)
            {
                yflag = 1;
            }
            if ((index & 4) == 4)
            {
                zflag = 1;
            }

            double Xstart = Box.x + xflag * Box.xSize / 2;
            double Ystart = Box.y + yflag * Box.ySize / 2;
            double Zstart = Box.z + zflag * Box.zSize / 2;

            Cell<T> cell = new Cell<T>();
            cell.Box = new BindBox();
            cell.Box.x = Xstart;
            cell.Box.y = Ystart;
            cell.Box.z = Zstart;
            cell.Box.xSize = Box.xSize / 2;
            cell.Box.ySize = Box.ySize / 2;
            cell.Box.zSize = Box.zSize / 2;
            cell.Parent = this;
            cell.Depth = Depth + 1;

            cell.AddChild();

            Childrens[index] = cell;

            return cell;
        }

        public Cell<T> AddChild(Vector3D position)
        {
            int index = FindIndex(position);

            //Console.WriteLine(index);
            if (Childrens == null)
            {
                Childrens = new Cell<T>[8];
                Childrens[index].AddChild();
            }

            if (Childrens[index] != null)
            {
                return Childrens[index];
            }

            short xflag = 0;
            short yflag = 0;
            short zflag = 0;

            if ((index & 1) == 1)
            {
                xflag = 1;
            }
            if ((index & 2) == 2)
            {
                yflag = 1;
            }
            if ((index & 4) == 4)
            {
                zflag = 1;
            }

            double Xstart = Box.x + xflag * Box.xSize / 2;
            double Ystart = Box.y + yflag * Box.ySize / 2;
            double Zstart = Box.z + zflag * Box.zSize / 2;

            Cell<T> cell = new Cell<T>();
            cell.Box = new BindBox();
            cell.Box.x = Xstart;
            cell.Box.y = Ystart;
            cell.Box.z = Zstart;
            cell.Box.xSize = Box.xSize / 2;
            cell.Box.ySize = Box.ySize / 2;
            cell.Box.zSize = Box.zSize / 2;
            cell.Parent = this;
            cell.Depth = Depth + 1;

            Childrens[index] = cell;

            return cell;

        }

        public Cell<T> FindCellWithPosition(double x, double y, double z)
        {
            Cell<T> target = null;

            foreach (Cell<T> item in Childrens)
            {
                if ((item.Box.x <= x && x < item.Box.x + item.Box.xSize) &&
                    (item.Box.y <= y && y < item.Box.y + item.Box.ySize) &&
                    (item.Box.z <= z && z < item.Box.z + item.Box.zSize)
                    )
                {
                    target = item;
                    break;
                }
            }

            return target;
        }

        public bool Contain(double x, double y, double z)
        {
            return (this.Box.x <= x && x < this.Box.x + this.Box.xSize) &&
                    (this.Box.y <= y && y < this.Box.y + this.Box.ySize) &&
                    (this.Box.z <= z && z < this.Box.z + this.Box.zSize);
        }

        public bool AddItemToChild(Vector3D position, T Object)
        {
            Cell<T> target = null;
            if ((target = FindCellWithPosition(position.x, position.y, position.z)) != null)
            {
                target.Object = Object;
                return true;
            }
            return false;
        }
    }
}
