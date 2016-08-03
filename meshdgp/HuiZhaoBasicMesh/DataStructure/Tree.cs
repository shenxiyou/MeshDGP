using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class DynamicTree<T>
    {
        public TreeNode<T> Root;

    }

    public class TreeNode<T>
    {
        public T Attribute;

        public TreeNode<T> Parent;

        public TreeNode<T> LeftMostChild;

        public TreeNode<T> RightSibling;

        public int ChildCount;

        public TreeNode(T value)
        {
            this.Parent = null;
            this.LeftMostChild = null;
            this.RightSibling = null;
            this.ChildCount = 0;
            this.Attribute = value;
        }

        public void AddChild(TreeNode<T> child)
        {
            ChildCount++;


            child.Parent = this;

            if (LeftMostChild == null)
            {
                LeftMostChild = child;
                return;
            }

            TreeNode<T> currentNode = LeftMostChild;

            while (currentNode.RightSibling != null)
            {
                currentNode = currentNode.RightSibling;
            }

            currentNode.RightSibling = child;
        }

        public int TrivalChildrenCount()
        {
            Queue<TreeNode<T>> queue = new Queue<TreeNode<T>>();
            queue.Enqueue(this);

            int sum = 0;
            do
            {
                TreeNode<T> currentNode = queue.Dequeue();

                //Add child node to queue 
                TreeNode<T> currentChildNode = currentNode.LeftMostChild;
                do
                {
                    queue.Enqueue(currentChildNode);
                    sum++;
                    currentChildNode = currentChildNode.RightSibling;
                } while (currentChildNode != null);


            } while (queue.Count != 0);


            return sum;
        }

    }


}
