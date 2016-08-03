using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Media3D;
using System.Diagnostics;

using System.CodeDom.Compiler;
using System.Reflection;
using System.IO;
using System.Data;

using Autodesk.Maya;
using Autodesk.Maya.OpenMaya;
using Autodesk.Maya.OpenMayaUI;

namespace GraphicResearchHuiZhao
{
    public class MayaBridge
    {
        private static MayaBridge singleton = new MayaBridge(); 
        public static MayaBridge Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new MayaBridge();
                return singleton;
            }
        }


        public void DeformInit()
        {
            ConvertToTriMesh();
            UpdateMeshSelection(GlobalData.Instance.TriMesh);
            DeformController.Instance.InitDeformer(GlobalData.Instance.TriMesh, EnumDeformIterative.Hybrid);
        }

        public void DeformIterative()
        {
            UpdateTriMesh();
            DeformController.Instance.deformer.Deform();
        }

        public void UpdateTriMesh()
        {
            MFnMesh fnMesh = GetFirstMesh();
            UpdateTriMesh(fnMesh, GlobalData.Instance.TriMesh);
        }

        public void UpdateTriMesh(MFnMesh fnMesh, TriMesh triMesh)
        {
            for (int i = 0; i < triMesh.Vertices.Count; i++)
            {
                MPoint pos = new MPoint();
                fnMesh.getPoint(i, pos);
                triMesh.Vertices[i].Traits.Position.x = pos.x;
                triMesh.Vertices[i].Traits.Position.y = pos.y;
                triMesh.Vertices[i].Traits.Position.z = pos.z;
            }
        }



       public MObject ConvertMeshMaya(TriMesh triMesh)
       {
          MFnMesh meshMaya = new MFnMesh();

          int verticeNum = triMesh.Vertices.Count;
          MFloatPointArray points = new MFloatPointArray();
         
          for(int i=0;i<verticeNum;i++)
          {
              float x=(float)triMesh.Vertices[i].Traits.Position.x;
              float y=(float)triMesh.Vertices[i].Traits.Position.y;
              float z=(float)triMesh.Vertices[i].Traits.Position.z;

              MFloatPoint vertex = new MFloatPoint(x,y,z);
              points.append(vertex);
          }

           
           int faceNum = triMesh.Faces.Count;

           int[] face_Counts=new int[faceNum];
           for(int i=0;i<faceNum;i++)
           {
              face_Counts[i]=3;
           }
           MIntArray faceCounts = new MIntArray(face_Counts);


           int[] faceTopology=new int[faceNum*3];
            
           for(int j=0;j<faceNum ;j++)
           {
              int index=j*3;
              faceTopology[index]=triMesh.Faces[j].GetVertex(0).Index ;
               
              faceTopology[index+1]=triMesh.Faces[j].GetVertex(1).Index ;
               
              faceTopology[index+2]=triMesh.Faces[j].GetVertex(2).Index ;
               
           }

           MIntArray faceConnects = new MIntArray(faceTopology);

           MObject mesh= meshMaya.create(verticeNum, faceNum, points, faceCounts, faceConnects);

           return mesh;
       }


       public TriMesh ConvertTriMesh(MFnMesh mesh)
       {
            if(mesh==null)
            {
                MGlobal.displayInfo("Mesh is null \n");
            }

           TriMesh trimesh = new TriMesh();

           MIntArray indices = new MIntArray();
           MIntArray triangleCounts = new MIntArray();
           MPointArray points = new MPointArray();

           mesh.getTriangles(triangleCounts, indices);
           mesh.getPoints(points);

           // Get the triangle indices 
        
           for (int i = 0; i < (int)points.length; ++i)
           {
               MPoint pt = points[i];
               VertexTraits trait = new VertexTraits(pt.x, pt.y, pt.z);
               trimesh.Vertices.Add(trait);
           }
           
           
           MGlobal.displayInfo( indices.Count.ToString() +"\n");
           int j=0;
           while(j<indices.Count)
           {
               int a = indices[j];
               j++;
               int b = indices[j];
               j++;
               int c = indices[j];
               j++;
               trimesh.Faces.AddTriangles(trimesh.Vertices[a], trimesh.Vertices[b], trimesh.Vertices[c]);
                
           } 
           return trimesh;
       }

 

       public void UpdateMeshMaya(MFnMesh fnMesh, TriMesh triMesh)
       {
           for (int i = 0; i < triMesh.Vertices.Count; i++)
           {
               MPoint pos = new MPoint(triMesh.Vertices[i].Traits.Position.x,
                                       triMesh.Vertices[i].Traits.Position.y,
                                       triMesh.Vertices[i].Traits.Position.z);
               fnMesh.setPoint(i, pos);
           }
       }


       public void HookUp()
       {
           ConvertToTriMesh();
           GlobalData.Instance.Changed += Instance_Changed;
          
       }

       public void HookUpCreate()
       {
          // ToTriMesh();
           GlobalData.Instance.Changed += Instance_ChangedCreate;

       }

       void Instance_ChangedCreate(object sender, EventArgs e)
       {
           //MFnMesh fnMesh = GetFirstMesh();
           //UpdateSelect(fnMesh, GlobalData.Instance.TriMesh);
           ConvertMeshMaya(GlobalData.Instance.TriMesh);
       }

       void Instance_Changed(object sender, EventArgs e)
       {
           MGlobal.displayInfo("update!!!\n");
           MFnMesh fnMesh = GetFirstMesh();
           UpdateMeshMaya(fnMesh, GlobalData.Instance.TriMesh);
       }

       

        public void ConvertToTriMesh()
       {
           MFnMesh fnMesh = GetFirstMesh();
           TriMesh mesh = ConvertTriMesh(fnMesh);
           GlobalData.Instance.TriMesh = mesh;
       }

       

       public void DeleteSelection()
        {
            MObject sel = GetFirstSelectedObject(); 
            MGlobal.deleteNode(sel);

        }
        public MFnMesh GetFirstMesh()
        {
               var selected = GetFirstSelectedMesh();

               MFnMesh mesh=null;
                // The reason why there might not be a selection is if some tool isn't closed
                // and prevent the previous selection command from going through
                if (selected != null)
                {
                    if (selected.node.apiTypeStr == "kMesh")
                    {
                        mesh = new MFnMesh(selected);
                          
                    }

                    if (selected.node.apiTypeStr == "kTransform")
                    {
                        MFnTransform trans = new MFnTransform(selected); 
                        mesh = new MFnMesh(trans.child(0)); 
                    }
                }

                return mesh;
        }


        public void UpdateMeshSelection(TriMesh mesh)
        {
            var selected = MGlobal.activeSelectionList;
            var it = new MItSelectionList(selected, MFn.Type.kMeshVertComponent);

            MObject vertice = new MObject();
            for (; !it.isDone; it.next())
            {

                var path = new MDagPath();
                it.getDagPath(path, vertice);
                MGlobal.displayInfo(path.fullPathName);

                if (!vertice.isNull)
                {

                    MItMeshVertex itvertex = new MItMeshVertex(path, vertice);

                    for (; !itvertex.isDone; itvertex.next())
                    {
                        int index = itvertex.index();

                        mesh.Vertices[index].Traits.SelectedFlag = 1;
                    }

                }

            }

            TriMeshUtil.GroupVertice(mesh);

        }
 
       public void ShowMeshInfo()
       {
           // If it's a mesh, display it
           var selected = GetFirstSelectedMesh();

           if (selected != null)
           { 

               Form1 t = new Form1(selected);
               t.BringToFront();
               t.Show();
           }
       }

       public void GetVerticeSelected()
       { 
           var selected = MGlobal.activeSelectionList;
           var it = new MItSelectionList(selected,MFn.Type.kMeshVertComponent);

           MObject vertice=new MObject();
           for(; !it.isDone; it.next())
		   {
             
                var path = new MDagPath();
                it.getDagPath(path,vertice);
                MGlobal.displayInfo(path.fullPathName);

                if (!vertice.isNull)
                {

                    MItMeshVertex itvertex = new MItMeshVertex(path, vertice);

                    for (; !itvertex.isDone; itvertex.next())
                    {
                           int index=  itvertex.index();

                           MGlobal.displayInfo(index.ToString()+"\n");
                    }

                } 

           }
           
       }



       public   MDagPath GetFirstSelectedMesh()
       {
           var selected = MGlobal.activeSelectionList;
           var it = new MItSelectionList(selected);
           if (it.isDone) return null;
           var path = new MDagPath();
           it.getDagPath(path);
           return path;
       }



       public MObject GetFirstSelectedObject()
       {
           var selected = MGlobal.activeSelectionList;
           var it = new MItSelectionList(selected);
           if (it.isDone) return null;
           var path = new MDagPath();
           MObject mesh=new MObject();
           it.getDagPath(path, mesh);
           return mesh;
       }



















       //public Object GetObject(string command)
       //{
       //    MGlobal.displayInfo(command);
       //    Object ObjList = MayaProxy.Instance.GetDAG(command);

       //    return ObjList;
       //}



       //public void DisplayObjecty(Object ObjList)
       // {
           
       //      // If there was no error
       //     if (ObjList != null)
       //     { 
       //         try
       //         { 
       //             FormConfigMaya.Instance.Datagrid.Rows.Clear();
       //         }

       //         catch
       //         {
       //         }
              
       //         // Get whatever's returned by the script
       //         var ObjEnum = ObjList as IEnumerable<MDagPath>;

          
       //         // Were some objects returned?
 
       //         if (!ObjEnum.Any<MDagPath>())
       //             MessageBox.Show("No object returned.", "DAG Explorer", MessageBoxButton.OK, MessageBoxImage.Information);
       //         else
       //         {
       //             LinkedList<MayaObject> myList;
       //             HashSet<MayaObjPropId> myProps;

       //             FormConfigMaya.Instance.Info = "test3";
       //             MayaProxy.Instance.GatherObjects(ObjEnum, out myList, out myProps);
                   

       //             FormConfigMaya.Instance.Datagrid.Columns.Clear(); 
       //             foreach (var p in myProps)
       //             { 
       //                 FormConfigMaya.Instance.Datagrid.Columns.Add(p.name, p.name);
                            
       //             }
       //             MGlobal.displayInfo(myProps.Count.ToString()+ "   Hello World\n");

               
       //             foreach (var Obj in myList)
       //             {
       //                 Object[] arr = new Object[myProps.Count];
       //                 MGlobal.displayInfo(myProps.Count.ToString());
       //                 //int i = 0;
       //                 //foreach (var p in myProps)
       //                 //{
       //                 //    MayaObjPropVal mopv;
       //                 //    // Search for the property in the object
       //                 //    if (Obj.properties.TryGetValue(p.name, out mopv))
       //                 //    {
       //                 //        arr[i] = mopv.value;
       //                 //    }
       //                 //    else
       //                 //    {
       //                 //        arr[i] = "";
       //                 //    }
                            
       //                 //    i++;
                         
                             
       //                 //}
       //                 MGlobal.displayInfo(myProps.Count.ToString());
                            
                            
                      
       //             }

                    
       //         }

                
       //     }
       // }
    }
}
