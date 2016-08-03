//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace GraphicResearchHuiZhao 
//{
//    public class NPRLine
//    {
//        private static NPRLine singleton = new NPRLine();


//        public static NPRLine Instance
//        {
//            get
//            {
//                if (singleton == null)
//                    singleton = new NPRLine();
//                return singleton;
//            }
//        }


//        public TriMesh  Run(TriMesh mesh, EnumLine type)
//        {
//            LineBase line = null;
//            switch (type)
//            {
//                case EnumLine.Contour:
//                    line = new Contours(mesh);
                    
//                    break;
//                case EnumLine.Apparent:
                    
//                    break; ;
//                case EnumLine.RidgeValley:
//                    line = new RidgeAndValley(mesh);
//                    break;
//                case EnumLine.Silluhoute:
//                    line = new Silluhoute(mesh);
//                    break;
//                case EnumLine.SuggestiveContour:
//                    line = new SuggestCountour(mesh);
//                    break;
//                case EnumLine.Demarcating:
//                    line = new DemarcatingCurve(mesh);
//                    break;

//                case EnumLine.Highlight:
//                    line = new HighlightLine(mesh);
//                    break;
                  

//            }

//           TriMesh lineMesh=line.Extract();
//           return lineMesh;

//        }

        

//        public void RunApparent(TriMesh mesh)
//        {
//            Contours line = new Contours(mesh);
//            line.Extract();
//        }

        

//        public void RunSilluhoute(TriMesh mesh)
//        {
//            Silluhoute line = new Silluhoute(mesh);

//            line.Extract();

//        }

        

//    }
//}
