using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class HeapNode<T>
    {
        public T Item;
        public double Value;
        internal int Pos;

        public HeapNode(double value, T item)
        {
            this.Value = value;
            this.Item = item;
        }
    }

    public class MinHeap<T>
    {
        public int size;
        public int capability;
        private HeapNode<T>[] nodes;

        public MinHeap(int capability)
        {
            this.nodes = new HeapNode<T>[capability + 1];
            this.capability = capability;
            this.nodes[0] = new HeapNode<T>(double.MinValue, default(T));
            this.size = 0;
        }

        public bool Check()
        {
            bool result = true;
            for (int i = 1; i <= size; i++)
            {
                if (!isLeaf(i))
                {
                    int smallChild = 2 * i;
                    if ((smallChild < size) && (nodes[smallChild].Value > nodes[smallChild + 1].Value))
                    {
                        smallChild = smallChild + 1;
                    }
                    if (nodes[i].Value > nodes[smallChild].Value)
                    {
                        Console.WriteLine(i + ": " + nodes[i].Value + ", child: " + nodes[smallChild].Value);
                        result = false;
                    }
                }
            }
            return result;
        }

        public HeapNode<T> Add(HeapNode<T> item)
        {
            if (size >= capability)
            {
                return null;
            }
            size++;
            nodes[size] = item;
            nodes[size].Pos = size;

            int pos = FilterUp(size);
            return nodes[pos];
        }

        public HeapNode<T> Add(double value, T item)
        {
            HeapNode<T> node = new HeapNode<T>(value, item);
            return this.Add(node);
        }

        public bool Del(HeapNode<T> item)
        {
            int pos = item.Pos;
            if (pos != 0)
            {
                nodes[pos] = nodes[size];
                nodes[pos].Pos = pos;
                if (nodes[pos].Value > item.Value)
                {
                    FilterDown(pos);
                }
                else
                {
                    FilterUp(pos);
                }
                size--;
                item.Pos = 0;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Update(HeapNode<T> item, double value)
        {
            if (item.Pos != 0)
            {
                double old = item.Value;
                item.Value = value;
                if (value > old)
                {
                    FilterDown(item.Pos);
                }
                else
                {
                    FilterUp(item.Pos);
                }
            }
        }

        public void Clear()
        {
            size = 0;
        }

        public HeapNode<T> Pull()
        {
            if (size == 0)
            {
                return null;
            }
            HeapNode<T> node = nodes[1];
            nodes[1] = nodes[size];
            nodes[size] = null;
            size--;

            if (size > 1)
            {
                FilterDown(1);
            }

            node.Pos = 0;
            return node;
        }

        private int FilterUp(int pos)
        {
            HeapNode<T> temp = nodes[pos];
            while (temp.Value < nodes[pos / 2].Value)
            {
                nodes[pos] = nodes[pos / 2];
                nodes[pos].Pos = pos;
                pos /= 2;
            }
            nodes[pos] = temp;
            nodes[pos].Pos = pos;
            return pos;
        }

        private int FilterDown(int pos)
        {
            HeapNode<T> temp = nodes[pos];
            while (!isLeaf(pos))
            {
                int smallChild = 2 * pos;
                if ((smallChild < size) && (nodes[smallChild].Value > nodes[smallChild + 1].Value))
                {
                    smallChild = smallChild + 1;
                }

                if (temp.Value > nodes[smallChild].Value)
                {
                    nodes[pos] = nodes[smallChild];
                    nodes[pos].Pos = pos;
                    pos = smallChild;
                }
                else
                {
                    break;
                }
            }
            nodes[pos] = temp;
            nodes[pos].Pos = pos;
            return pos;
        }

        private bool isLeaf(int pos)
        {
            return ((pos > size / 2) && (pos <= size));
        }
    }
}
