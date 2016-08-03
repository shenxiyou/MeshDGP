using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class RTSC
    {

        private static RTSC singleton = new RTSC();
        public static RTSC Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new RTSC();
                return singleton;
            }
        }


        public   Rtsc rtsc;
        public   void ChangeSettings()
        {
            RtscSettings settings = new RtscSettings();
            settings.sug_thresh = (float)ConfigNPR.Instance.SuggestiveContours;
            settings.sh_thresh = (float)ConfigNPR.Instance.SuggestiveHlt;
            settings.ph_thresh = (float)ConfigNPR.Instance.PrincipalHlt;
            settings.rv_thresh = (float)ConfigNPR.Instance.RidgeValley;
            settings.ar_thresh = (float)ConfigNPR.Instance.ApparentRidges;
            //settings.use_hermite = 1;
            settings.draw_hidden = ConfigNPR.Instance.DrawHiddenLine ? 1 : 0;
            settings.DrawAll();
            settings.SetUp();
        }

        public void Init(TriMesh mesh)
        {
            string file = "rtsc.obj"; 
            TriMeshIO.WriteToObjFile(file, mesh);
            this.rtsc = new Rtsc(file);
            this.rtsc.InitTriMesh();

           
        }

  
        public LineGLInfo GetLine(EnumLine type)
        {
            if(rtsc==null)
            {
                this.Init(GlobalData.Instance.TriMesh);
            }
            ChangeSettings();

            Vector3D viewpos = ToolPool.Instance.Tool.ComputeViewPoint();
            rtsc.InitRtsc(viewpos);

            switch (type)
            {
                case EnumLine.ApparentRidges:
                rtsc.DrawApparentRidges();
                break;

                case EnumLine.BoundaryLine:
                rtsc.DrawBoundaries();
                break;

                case EnumLine.Contours:
                rtsc.DrawOccludingContours();
                break;

                case EnumLine.DwKrLine:
                rtsc.DrawDwKr();
                break;

                case EnumLine.H0Line:
                rtsc.DrawH();
                break;
                case EnumLine.HighlightLine:
                rtsc.DrawSuggestiveHighlights();
                break;
                case EnumLine.Isophotes:
                Vector3D lightdir = GlobalSetting.Light0Setting.LightPosition;
                rtsc.DrawIsophotes(lightdir, ConfigNPR.Instance.IsoPhotosNum);
                break;
                case EnumLine.K0Line:
                rtsc.DrawK();
                break;
                case EnumLine.PrincipalHighlightRidges:
                rtsc.DrawPrincipalHighlightRidges();
                break;
                case EnumLine.PrincipalHighlightValleys:
                rtsc.DrawPrincipalHighlightValleys();
                break;
                case EnumLine.RidgesLine:
                rtsc.DrawRidges();
                break;
                case EnumLine.Silluhoute:
                rtsc.DrawExteriorSilhouette();
                break;
                case EnumLine.SuggestCountour:
                rtsc.DrawSuggestiveContours();
                break;
                case EnumLine.TopoLines:
                rtsc.DrawTopolines(ConfigNPR.Instance.TopoLineNum, ConfigNPR.Instance.TopoLineOffset);
                break;
                case EnumLine.ValleysLine:
                rtsc.DrawValleys();
                break;
               

            }
            LineGLInfo info=new LineGLInfo(rtsc.gl_alpha, rtsc.gl_vertex, rtsc.front_index);
            return info;
        }
    }
}
