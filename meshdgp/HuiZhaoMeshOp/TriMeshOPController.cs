//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace GraphicResearchHuiZhao
//{
//    public static class TriMeshOPController
//    {
//        public static TriMesh Subdivision(TriMesh mesh,EnumSubdivision type)
//        {
//            if (mesh == null)
//            {
//                return null;
//            }

//            TriMeshSubdivision sub = new TriMeshSubdivision(mesh);
//            return sub.SubDivision(type); 
            
//        }

//        public static void Smooth(TriMesh mesh)
//        {
//            TriMeshModify.TaubinSmooth(mesh);
//        }

//        public static void Swap(TriMesh mesh)
//        {
//        }


//    }
//}
