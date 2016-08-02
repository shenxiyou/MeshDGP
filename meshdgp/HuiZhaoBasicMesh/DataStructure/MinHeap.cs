using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class Node<T>
    {
        private double nodeValue;
        public double NodeValue
        {
            set { nodeValue = value; }
            get { return nodeValue; }
        }

        private T attachItem;
        public T AttachItem
        {
            set { attachItem = value; }
            get { return attachItem; }
        }

        public Node(double value, T item)
        {
            this.nodeValue = value;
            this.attachItem = item;
        }
        public Node() { }
    }

    public class MinHeap<T>
    {
        private int currentSize;
        public int Size
        {
            get { return currentSize - 1; }
        }

        private int maxSize;
        private int MaxSize
        {
            get { return maxSize - 1; }
        }
        private Node<T>[] nodes;

        public MinHeap(int capability)
        {
            nodes = new Node<T>[capability + 1];
            maxSize = capability + 1;
            currentSize = 0;
            Node<T> defaultNode = new Node<T>(double.MinValue, default(T));
            nodes[0] = defaultNode;
        }

        public void Add(double itemValue, T item)
        {
            if (currentSize >= maxSize - 1)
            {
                return;
            }
            currentSize++;
            nodes[currentSize] = new Node<T>(itemValue, item);
            int size = currentSize;

            while (nodes[size].NodeValue < nodes[parent(size)].NodeValue)
            {
                swap(size, parent(size));
                size = parent(size);
            }

        }

        public void FilterDown(int position)
        {
            int smallestChild;
            while (!isLeaf(position))
            {
                smallestChild = leftChild(position);
                if ((smallestChild < currentSize) && (nodes[smallestChild].NodeValue > nodes[smallestChild + 1].NodeValue))
                {
                    smallestChild = smallestChild + 1;
                }
                if (nodes[position].NodeValue <= nodes[smallestChild].NodeValue)
                {
                    return;
                }
                swap(position, smallestChild);
                position = smallestChild;
            }

        }


        public T Pull()
        {
            if (currentSize < 0)
            {
                return default(T);
            }
            swap(1, currentSize);
            currentSize--;

            if (currentSize != 0)
            {
                FilterDown(1);
            }

            T attachItem = nodes[currentSize + 1].AttachItem;
            nodes[currentSize + 1].NodeValue = double.MaxValue;
            nodes[currentSize + 1] = null;
            return attachItem;
        }

        private void swap(int i, int j)
        {
            Node<T> temp;

            temp = nodes[i];
            nodes[i] = nodes[j];
            nodes[j] = temp;
        }

        private int parent(int pos)
        {
            return pos / 2;
        }
        private int leftChild(int pos)
        {
            return 2 * pos;
        }
        private int rightChild(int pos)
        {
            return 2 * pos + 1;
        }

        private bool isLeaf(int pos)
        {
            return ((pos > currentSize / 2) && (pos <= currentSize));
        }
    }
}
