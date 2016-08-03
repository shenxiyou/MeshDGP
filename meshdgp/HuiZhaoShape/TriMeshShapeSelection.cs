using System;
using System.Collections.Generic;
using System.Text;
using System.IO ;

namespace GraphicResearchHuiZhao
{
     public partial class TriMeshShape
    {

         public TriMesh CreateShapeSel(EnumShapeSel shape)
        {
            TriMesh mesh = null;
            switch (shape)
            {

               case EnumShapeSel.SelectionVertex:
                    mesh = CreateSelectionVertex(MeshInput);
                    break;
               case EnumShapeSel.SelectionVertexSave:
                    CreateSelectionVertexSave(MeshInput);
                    break;
               case EnumShapeSel.SelectionEdge:
                    mesh = CreateSelectionEdge(MeshInput);
                    break;
               case EnumShapeSel.SelectionEdgeSave:
                    CreateSelectionEdgeSave(MeshInput);
                    break;
               case EnumShapeSel.SelectionFace:
                    mesh = CreateSelectionFace(MeshInput);
                    break;
               case EnumShapeSel.SelectionFaceSave:
                    CreateSelectionFaceSave(MeshInput);
                    break;

 
               case EnumShapeSel.SelectionAllSave:
                    CreateSelectionVertexSave(MeshInput);
                    CreateSelectionEdgeSave(MeshInput);
                    CreateSelectionFaceSave(MeshInput);
                    break;
                default:
                    break;
            } 

            return mesh;
            
        }



         

         public TriMesh AddSelectionVertex(TriMesh mesh, int index)
         {
             List<TriMesh> sel = new List<TriMesh>();
             for(int i=0;i<mesh.Vertices.Count;i++)
             {

                 if (mesh.Vertices[i].Traits.SelectedFlag == index)
                 {

                     TriMesh selV = TriMeshIO.FromObjFile(ConfigShape.Instance.VertexFile);
                     for (int j = 0; j < selV.Vertices.Count;j++ )
                     {
                         selV.Vertices[j].Traits.SelectedFlag =(byte)index;
                     }
                         TriMeshUtil.ScaleToUnit(selV, ConfigShape.Instance.Scale);
                     TriMeshUtil.TransformationMove(selV, mesh.Vertices[i].Traits.Position);
                     sel.Add(selV);
                 }
                
             }

            TriMesh result= TriMeshUtil.Combine(sel);
            result.FileName = Path.GetFileNameWithoutExtension(mesh.FileName) + "-V-" + index.ToString();

            TriMeshUtil.SetUpVertexNormal(result, EnumNormal.AreaWeight);
             return result;
         }

         public List<TriMesh> AddSelectionVertex(TriMesh mesh)
         {
             List<TriMesh> meshes = new List<TriMesh>();
             int count = TriMeshUtil.GroupVertice(mesh);
             for(int i=1;i<=count+1 ;i++)
             {
                 meshes.Add(AddSelectionVertex(mesh, i));
             }

             return meshes;

         }

         public TriMesh CreateSelectionVertex(TriMesh mesh)
         {

             List<TriMesh> meshes = AddSelectionVertex(mesh);
             if (!ConfigShape.Instance.SelectionOnly)
             {
                 meshes.Add(mesh);
             }
             TriMesh result= TriMeshUtil.Combine(meshes);

             TriMeshUtil.SetUpVertexNormal(result, EnumNormal.AreaWeight);

             return result;
         }

         public void CreateSelectionVertexSave(TriMesh mesh)
         {

             List<TriMesh> meshes = AddSelectionVertex(mesh);
             string dirName = null;
             if (ConfigShape.Instance.SaveDefaultDir)
             {
                  dirName = Path.Combine(Directory.GetCurrentDirectory(), ConfigShape.Instance.SaveFileDir);
             }
             else
             {
                 dirName = ConfigShape.Instance.SaveFileDir;
             }

             dirName = Path.Combine(dirName, Path.GetFileNameWithoutExtension(mesh.FileName));
             Directory.CreateDirectory(dirName);
             string filename = null;
                
            for(int i=0;i<meshes.Count;i++)
            {
              

               filename=dirName+"/"+meshes[i].FileName+".obj";

               TriMeshIO.WriteToObjFile(filename,meshes[i]);
            }

            filename = dirName + "/" + Path.GetFileNameWithoutExtension(mesh.FileName) + ".obj";

            TriMeshIO.WriteToObjFile(filename, mesh);

         }







         public TriMesh AddSelectionEdge(TriMesh mesh, int index)
         {
             List<TriMesh> sel = new List<TriMesh>();
             for (int i = 0; i < mesh.Edges.Count; i++)
             {

                 if (mesh.Edges[i].Traits.SelectedFlag == index)
                 {

                     TriMesh selV = TriMeshIO.FromObjFile(ConfigShape.Instance.EdgeFile);
                     TriMeshUtil.ScaleToUnit(selV, 1); 
                     TriMeshUtil.ScaleToUnit(selV, ConfigShape.Instance.Scale); 
                     Vector3D direction = mesh.Edges[i].Vertex1.Traits.Position - mesh.Edges[i].Vertex0.Traits.Position;
                     Vector3D loc = (mesh.Edges[i].Vertex1.Traits.Position + mesh.Edges[i].Vertex0.Traits.Position)/2;

                     Matrix4D scale = TriMeshUtil.ComputeMatrixScale(direction.Length() / ConfigShape.Instance.Scale, 1d, 1d);


                   //  TriMeshUtil.TransformationRotation(selV, direction);
                     
                     TriMeshUtil.TransformationScale(selV, scale);
                     TriMeshUtil.TransformRoatationV(selV, direction,Vector3D.UnitX);
                     //TriMeshUtil.TransformRotationX(selV, direction.x);
                     //TriMeshUtil.TransformRotationY(selV, direction.y);
                     //TriMeshUtil.TransformRotationZ(selV, direction.z);
                     TriMeshUtil.TransformationMove(selV, loc);
                     sel.Add(selV);
                 }

             }

             TriMesh result = TriMeshUtil.Combine(sel);
             result.FileName = Path.GetFileNameWithoutExtension(mesh.FileName) + "-E-" + index.ToString();

             TriMeshUtil.SetUpVertexNormal(result, EnumNormal.AreaWeight);
             return result;
         }

         public List<TriMesh> AddSelectionEdge(TriMesh mesh)
         {
             List<TriMesh> meshes = new List<TriMesh>();
             int count = 0;
             for (int i = 0; i < mesh.Edges.Count;i++ )
             {
                 if( mesh.Edges[i].Traits.SelectedFlag>count)
                 {
                     count++;
                 }
             }
                 for (int i = 1; i <= count ; i++)
                 {
                     meshes.Add(AddSelectionEdge(mesh, i));
                 }

             return meshes;

         }

         public TriMesh CreateSelectionEdge(TriMesh mesh)
         {

             List<TriMesh> meshes = AddSelectionEdge(mesh);
             if (!ConfigShape.Instance.SelectionOnly)
             {
                 meshes.Add(mesh);
             }
             TriMesh result = TriMeshUtil.Combine(meshes);

             TriMeshUtil.SetUpVertexNormal(result, EnumNormal.AreaWeight);

             return result;
         }

         public void CreateSelectionEdgeSave(TriMesh mesh)
         {

             List<TriMesh> meshes = AddSelectionEdge(mesh);
             string dirName = null;
             if (ConfigShape.Instance.SaveDefaultDir)
             {
                 dirName = Path.Combine(Directory.GetCurrentDirectory(), ConfigShape.Instance.SaveFileDir);
             }
             else
             {
                 dirName = ConfigShape.Instance.SaveFileDir;
             }

             dirName = Path.Combine(dirName, Path.GetFileNameWithoutExtension(mesh.FileName));
             Directory.CreateDirectory(dirName);
             string filename = null;

             for (int i = 0; i < meshes.Count; i++)
             {


                 filename = dirName + "/" + meshes[i].FileName + ".obj";

                 TriMeshIO.WriteToObjFile(filename, meshes[i]);
             }

             filename = dirName + "/" + Path.GetFileNameWithoutExtension(mesh.FileName) + ".obj";

             TriMeshIO.WriteToObjFile(filename, mesh);

         }






         public TriMesh AddSelectionFace(TriMesh mesh, int index)
         {
             TriMesh result = new TriMesh();

             List<TriMesh.Vertex> arr = new List<HalfEdgeMesh.Vertex>();
             int vIndex = 0;
             List<TriMesh> sel = new List<TriMesh>();
             for (int i = 0; i < mesh.Faces.Count; i++)
             {

                 if (mesh.Faces[i].Traits.SelectedFlag == index)
                 {
                      
                            
                             foreach (TriMesh.Vertex v in mesh.Faces[i].Vertices)
                             {
                                 arr.Add(result.Vertices.Add(new VertexTraits(v.Traits.Position)));
                                 arr[arr.Count-1].Traits = v.Traits;
                             }

                             TriMesh.Face faceNew = result.Faces.Add(arr[vIndex*3+0],
                                                                     arr[vIndex*3 + 1],
                                                                     arr[vIndex*3 + 2]);
                             faceNew.Traits = mesh.Faces[i].Traits;
                             vIndex++;
                             
                 }
             
                    
               

             }


             result.FileName = Path.GetFileNameWithoutExtension(mesh.FileName) + "-F-" + index.ToString();

             TriMeshUtil.SetUpVertexNormal(result, EnumNormal.AreaWeight);
             return result;
         }

         public List<TriMesh> AddSelectionFace(TriMesh mesh)
         {
             List<TriMesh> meshes = new List<TriMesh>();
             int count = 0;
             for (int i = 0; i < mesh.Faces.Count; i++)
             {
                 if (mesh.Faces[i].Traits.SelectedFlag > count)
                 {
                     count++;
                 }
             }
             for (int i = 1; i <= count ; i++)
             {
                 meshes.Add(AddSelectionFace(mesh, i));
             }

             return meshes;

         }

         public TriMesh CreateSelectionFace(TriMesh mesh)
         {

             List<TriMesh> meshes = AddSelectionFace(mesh);
             if (!ConfigShape.Instance.SelectionOnly)
             {
                 meshes.Add(mesh);
             }
             TriMesh result = TriMeshUtil.Combine(meshes);

             TriMeshUtil.SetUpVertexNormal(result, EnumNormal.AreaWeight);

             return result;
         }

         public void CreateSelectionFaceSave(TriMesh mesh)
         {

             List<TriMesh> meshes = AddSelectionFace(mesh);
             string dirName = null;
             if (ConfigShape.Instance.SaveDefaultDir)
             {
                 dirName = Path.Combine(Directory.GetCurrentDirectory(), ConfigShape.Instance.SaveFileDir);
             }
             else
             {
                 dirName = ConfigShape.Instance.SaveFileDir;
             }

             dirName = Path.Combine(dirName, Path.GetFileNameWithoutExtension(mesh.FileName));
             Directory.CreateDirectory(dirName);
             string filename = null;

             for (int i = 0; i < meshes.Count; i++)
             {


                 filename = dirName + "/" + meshes[i].FileName + ".obj";

                 TriMeshIO.WriteToObjFile(filename, meshes[i]);
             }

             filename = dirName + "/" + Path.GetFileNameWithoutExtension(mesh.FileName) + ".obj";

             TriMeshIO.WriteToObjFile(filename, mesh);

         }


        
     
    }
}
