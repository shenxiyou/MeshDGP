using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class TreeCoTree
    {
        public TriMesh mesh = null;
        public TreeCoTree(TriMesh mesh)
        {
            this.mesh = mesh;
        }

        public bool IsBoundaryGenerator(List<TriMesh.HalfEdge> cycle)
        {
            if (cycle.Count == 0)
            {
                return false;
            }
            else
                return (cycle[0].FromVertex.OnBoundary || cycle[0].ToVertex.OnBoundary);
        }

        private bool isDualBoundaryLoop(List<TriMesh.HalfEdge> collections)
        {
            if (collections.Count == 0)
            {
                return false;
            }
            else
            {
                return (collections[0].FromVertex.OnBoundary || collections[0].ToVertex.OnBoundary);
            }
        }
        public DynamicTree<TriMesh.Vertex> BuildPrimalSpanningTree(TriMesh mesh, out TreeNode<TriMesh.Vertex>[] primalFlags)
        {
            DynamicTree<TriMesh.Vertex> primalSpanningTree = new DynamicTree<HalfEdgeMesh.Vertex>();
            TreeNode<TriMesh.Vertex>[] flags = new TreeNode<HalfEdgeMesh.Vertex>[mesh.Vertices.Count];
            foreach (TriMesh.Vertex v in mesh.Vertices)
            {
                if (!v.OnBoundary)
                {
                    TreeNode<TriMesh.Vertex> root = new TreeNode<HalfEdgeMesh.Vertex>(v);
                    primalSpanningTree.Root = root;
                    flags[v.Index] = root;
                    break;
                }
            }
            //Iterate verties to get spanning tree
            Queue<TreeNode<TriMesh.Vertex>> queue = new Queue<TreeNode<TriMesh.Vertex>>();
            queue.Enqueue(primalSpanningTree.Root);
            do
            {
                TreeNode<TriMesh.Vertex> currentNode = queue.Dequeue();
                TriMesh.Vertex currentVertex = currentNode.Attribute;
                TriMesh.HalfEdge he = currentVertex.HalfEdge;
                do
                {
                    TriMesh.Vertex to = he.Opposite.FromVertex;
                    if (flags[to.Index] == null &&
                        !to.OnBoundary &&
                        to != primalSpanningTree.Root.Attribute)
                    {
                        TreeNode<TriMesh.Vertex> node = new TreeNode<HalfEdgeMesh.Vertex>(to);
                        flags[to.Index] = node;
                        currentNode.AddChild(node);
                        queue.Enqueue(node);
                    }
                    he = he.Opposite.Next;
                } while (he != currentVertex.HalfEdge);
            } while (queue.Count != 0);
            primalFlags = flags;
            return primalSpanningTree;
        }

        public bool InPrimalSpanningTree(TriMesh.HalfEdge halfedge, TreeNode<TriMesh.Vertex>[] primalFlags)
        {
            TriMesh.Vertex v1, v2;
            v1 = halfedge.FromVertex;
            v2 = halfedge.ToVertex;

            TreeNode<TriMesh.Vertex> v1Node = primalFlags[v1.Index];
            TreeNode<TriMesh.Vertex> v2Node = primalFlags[v2.Index];

            if (v1Node != null && v2Node != null)
            {
                if (v1Node.Parent == v2Node || v2Node.Parent == v1Node)
                {
                    return true;
                }
            }

            return false;
        }

        public bool InDualSpanningTree(TriMesh.HalfEdge halfedge, TreeNode<TriMesh.Face>[] dualFlags)
        {
            TriMesh.Face f1, f2;
            f1 = halfedge.Face;
            f2 = halfedge.Opposite.Face;

            TreeNode<TriMesh.Face> f1Node = dualFlags[f1.Index];
            TreeNode<TriMesh.Face> f2Node = dualFlags[f2.Index];

            if (f1Node != null && f2Node != null)
            {
                if (f1Node.Parent == f2Node || f2Node.Parent == f1Node)
                {
                    return true;
                }
            }

            return false;
        }

        public DynamicTree<TriMesh.Face> BuildDualSpanningCoTree(TriMesh mesh, 
                                                                 TreeNode<TriMesh.Vertex>[] primalFlags, 
                                                                 out TreeNode<TriMesh.Face>[] dualFlags)
        {
            DynamicTree<TriMesh.Face> dualSpanningTree = new DynamicTree<HalfEdgeMesh.Face>();
            TreeNode<TriMesh.Face>[] flags = new TreeNode<HalfEdgeMesh.Face>[mesh.Faces.Count];
            TreeNode<TriMesh.Face> root = new TreeNode<HalfEdgeMesh.Face>(mesh.Faces[0]);
            dualSpanningTree.Root = root;
            flags[mesh.Faces[0].Index] = root;
            Queue<TreeNode<TriMesh.Face>> queue = new Queue<TreeNode<TriMesh.Face>>();
            queue.Enqueue(dualSpanningTree.Root);
            do
            {
                TreeNode<TriMesh.Face> currentNode = queue.Dequeue();
                TriMesh.Face currentFace = currentNode.Attribute;
                TriMesh.HalfEdge he = currentFace.HalfEdge;
                do
                {
                    TriMesh.Face toFace = he.Opposite.Face;
                    if (toFace != null
                        && flags[toFace.Index] == null
                        && toFace != dualSpanningTree.Root.Attribute
                        && !InPrimalSpanningTree(he, primalFlags)
                        )
                    {
                        TreeNode<TriMesh.Face> node = new TreeNode<HalfEdgeMesh.Face>(toFace);
                        flags[toFace.Index] = node;
                        currentNode.AddChild(node);
                        queue.Enqueue(node);
                    }
                    he = he.Next;
                } while (he != currentFace.HalfEdge);
            } while (queue.Count != 0);
            dualFlags = flags;
            return dualSpanningTree;
        }


       

        private TriMesh.HalfEdge GetSharedHalfedge(TriMesh.Face face1, TriMesh.Face face2)   //References to Face1
        {
            TriMesh.HalfEdge shared = null;

            TriMesh.HalfEdge currentFace1Hf = face1.HalfEdge;

            do
            {
                if (currentFace1Hf.Opposite.Face == face2)
                {
                    return currentFace1Hf;
                }

                currentFace1Hf = currentFace1Hf.Next;
            } while (currentFace1Hf != face1.HalfEdge);

            return shared;
        }
        public List<List<TriMesh.HalfEdge>> BuildCycle(TriMesh mesh, 
                                                       TreeNode<TriMesh.Vertex>[] primalFlags, 
                                                       TreeNode<TriMesh.Face>[] dualFlags)
        {
            List<List<TriMesh.HalfEdge>> cycles = new List<List<HalfEdgeMesh.HalfEdge>>(); 
            foreach (TriMesh.Edge edge in mesh.Edges)
            {
                if (edge.OnBoundary)
                {
                    continue;
                } 
                if (!InPrimalSpanningTree(edge.HalfEdge1, primalFlags)
                    && !InDualSpanningTree(edge.HalfEdge1, dualFlags))
                { 
                    List<TriMesh.HalfEdge> rightFaceCollections = new List<HalfEdgeMesh.HalfEdge>();
                    TreeNode<TriMesh.Face> currentRightDualNode = dualFlags[edge.HalfEdge1.Face.Index];
                    while (currentRightDualNode != null && currentRightDualNode.Parent != null)
                    {
                        TriMesh.HalfEdge sharedHf = GetSharedHalfedge(currentRightDualNode.Attribute, currentRightDualNode.Parent.Attribute);
                        rightFaceCollections.Add(sharedHf);
                        currentRightDualNode = currentRightDualNode.Parent;
                    } 
                    List<TriMesh.HalfEdge> leftFaceCollections = new List<HalfEdgeMesh.HalfEdge>();
                    TreeNode<TriMesh.Face> currentLeftDualNode = dualFlags[edge.HalfEdge0.Face.Index];
                    while (currentLeftDualNode != null && currentLeftDualNode.Parent != null)
                    {
                        TriMesh.HalfEdge sharedHf = GetSharedHalfedge(currentLeftDualNode.Attribute, currentLeftDualNode.Parent.Attribute);
                        leftFaceCollections.Add(sharedHf);
                        currentLeftDualNode = currentLeftDualNode.Parent;
                    } 
                    //if (rightFaceCollections.Count == 0 || leftFaceCollections.Count == 0)
                    //{
                    //    continue;
                    //} 
                    List<TriMesh.HalfEdge> g = new List<HalfEdgeMesh.HalfEdge>();
                    g.Add(edge.HalfEdge0); 
                    int m = rightFaceCollections.Count - 1;
                    int n = leftFaceCollections.Count - 1;
                    while (leftFaceCollections[n] == rightFaceCollections[m])
                    {
                        m--;
                        n--;
                    } 
                    for (int i = 0; i <= m; i++)
                    {
                        g.Add(rightFaceCollections[i]);
                    } 
                    for (int i = n; i >= 0; i--)
                    {
                        g.Add(leftFaceCollections[i].Opposite);
                    }

                    //Check boundary
                    if (isDualBoundaryLoop(g))
                    {
                        if (g[0].Next.FromVertex.OnBoundary)
                        {
                            int count = g.Count;
                            for (int i = 0; i < count; i++)
                            {
                                g[i] = g[i].Opposite;
                            }
                            for (int i = 0; i < count / 2; i++)
                            {
                                //Swap
                                TriMesh.HalfEdge temp = g[i];
                                g[i] = g[count - 1 - i];
                                g[count - 1 - i] = temp;
                            }
                        }
                    }

                    cycles.Add(g);
                }


            }

            return cycles;
        }

        public void GeneratorTreeCotree(TriMesh mesh, out DynamicTree<TriMesh.Vertex> primalTree, out DynamicTree<TriMesh.Face> dualTree)
        {
            TreeNode<TriMesh.Vertex>[] primalFlags = null;
            TreeNode<TriMesh.Face>[] dualFlags = null;
            primalTree = BuildPrimalSpanningTree(mesh, out primalFlags);
            dualTree = BuildDualSpanningCoTree(mesh, primalFlags, out dualFlags);
        }

        public List<List<TriMesh.HalfEdge>> ExtractHonologyGenerator(TriMesh mesh)
        {
            TreeNode<TriMesh.Vertex>[] primalFlags = null;
            TreeNode<TriMesh.Face>[] dualFlags = null;
            DynamicTree<TriMesh.Vertex> primalTree = BuildPrimalSpanningTree(mesh, out primalFlags);
            DynamicTree<TriMesh.Face> dualTree = BuildDualSpanningCoTree(mesh, primalFlags, out dualFlags);
            List<List<TriMesh.HalfEdge>> cycles = BuildCycle(mesh, primalFlags, dualFlags);
            ColorCycle(cycles);
            return cycles;

        }

        public void ColorCycle(List<List<TriMesh.HalfEdge>> cycles)
        {
            byte i = 1;
            foreach (List<TriMesh.HalfEdge> list in cycles)
            {
                foreach (TriMesh.HalfEdge hf in list)
                {
                    hf.Edge.Traits.SelectedFlag = i;
                   
                }
                i++;
            }
        }

        

       

        public void DepthFirst(TriMesh mesh)
        {
        }

        public void WidthFirst(TriMesh mesh)
        {

        }




      

        public List<TriMesh.Edge> BuildTree(DynamicTree<TriMesh.Vertex> tree)
        {

            if (tree == null  )
            {
                return null;
            }

            List<TriMesh.Edge> treeMarks = new List<HalfEdgeMesh.Edge>();

            
                treeMarks = new List<HalfEdgeMesh.Edge>();
                TreeNode<TriMesh.Vertex> currentNode = tree.Root;
                Queue<TreeNode<TriMesh.Vertex>> queue = new Queue<TreeNode<TriMesh.Vertex>>();
                queue.Enqueue(currentNode);
                do
                {
                    currentNode = queue.Dequeue();

                    TreeNode<TriMesh.Vertex> currentChild = currentNode.LeftMostChild;
                    while (currentChild != null)
                    {
                        //Mark edge
                        TriMesh.Vertex v1 = currentNode.Attribute;
                        TriMesh.Vertex v2 = currentChild.Attribute;

                        foreach (TriMesh.HalfEdge hf in v1.HalfEdges)
                        {
                            if (hf.ToVertex == v2)
                            {
                                treeMarks.Add(hf.Edge);
                                break;
                            }
                        }

                        queue.Enqueue(currentChild);
                        currentChild = currentChild.RightSibling;
                    }

                } while (queue.Count > 0);
           

            return treeMarks;

             
            
        }

       
        public List<TriMesh.Edge> BuildCoTree(DynamicTree<TriMesh.Face> cotree)
        {

            if (cotree == null)
            {
                return null;
            }

            List<TriMesh.Edge> cotreeMarks = new List<HalfEdgeMesh.Edge>();

            

            
                Vector3D[]  bycenters = new Vector3D[mesh.Faces.Count];
                cotreeMarks = new List<HalfEdgeMesh.Edge>();
                TreeNode<TriMesh.Face> currentCoNode = cotree.Root;
                Queue<TreeNode<TriMesh.Face>> coQueue = new Queue<TreeNode<TriMesh.Face>>();
                coQueue.Enqueue(currentCoNode);
                do
                {
                    currentCoNode = coQueue.Dequeue();
                    bycenters[currentCoNode.Attribute.Index] = TriMeshUtil.GetMidPoint(currentCoNode.Attribute);

                    TreeNode<TriMesh.Face> currentChild = currentCoNode.LeftMostChild;
                    while (currentChild != null)
                    {
                        //Mark edge
                        TriMesh.Face f1 = currentCoNode.Attribute;
                        TriMesh.Face f2 = currentChild.Attribute;

                        foreach (TriMesh.HalfEdge hf in f1.Halfedges)
                        {
                            if (hf.Opposite.Face == f2)
                            {
                                cotreeMarks.Add(hf.Edge);
                                break;
                            }
                        }

                        coQueue.Enqueue(currentChild);
                        currentChild = currentChild.RightSibling;
                    }

                } while (coQueue.Count > 0);
       
            return cotreeMarks; 

        }


        


    }
}
