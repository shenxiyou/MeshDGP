using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.ComponentModel;

namespace GraphicResearchHuiZhao
{
    public class ConfigNPR
    {
        private static ConfigNPR singleton = new ConfigNPR(); 
        public static ConfigNPR Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new ConfigNPR();
                return singleton;
            }
        }


        private bool rtsc = true;
        public bool RTSC
        {
            get
            {
                return rtsc;
            }
            set
            {
                rtsc = value;
            }
        }


        public bool[] Enable = new bool[NPRLines.All.Length];

        private MeshStyle curvColor = MeshStyle.None;
        public MeshStyle CurvatureColor
        {
            get { return curvColor; }
            set { curvColor = value; }
        }

        private double lineWidth = 2;
        [Editor(typeof(NumericScrollBarEditor), typeof(UITypeEditor))]
        [Range(0, 20, 0.1)]
        public double LineWidth
        {
            get { return lineWidth; }
            set { lineWidth = value; }
        }

        private double hiddenLineWidth = 1;
        [Editor(typeof(NumericScrollBarEditor), typeof(UITypeEditor))]
        [Range(0, 20, 0.1)]
        public double HiddenLineWidth
        {
            get { return hiddenLineWidth; }
            set { hiddenLineWidth = value; }
        }

        private bool drawhiddenLine = false;
        public bool DrawHiddenLine
        {
            get { return drawhiddenLine; }
            set { drawhiddenLine = value; }
        }

        private double suggestiveContours = 0.01;
        [Editor(typeof(NumericScrollBarEditor), typeof(UITypeEditor))]
        [Range(0, 0.1, 0.001)]
        public double SuggestiveContours
        {
            get { return suggestiveContours; }
            set { suggestiveContours = value; }
        }

        private double suggestiveHlt = 0.02;
        [Editor(typeof(NumericScrollBarEditor), typeof(UITypeEditor))]
        [Range(0, 0.1, 0.001)]
        public double SuggestiveHlt
        {
            get { return suggestiveHlt; }
            set { suggestiveHlt = value; }
        }

        private double principalHlt = 0.04;
        [Editor(typeof(NumericScrollBarEditor), typeof(UITypeEditor))]
        [Range(0, 0.1, 0.001)]
        public double PrincipalHlt
        {
            get { return principalHlt; }
            set { principalHlt = value; }
        }

        private double ridgeValley = 0.1;
        [Editor(typeof(NumericScrollBarEditor), typeof(UITypeEditor))]
        [Range(0, 0.5, 0.001)]
        public double RidgeValley
        {
            get { return ridgeValley; }
            set { ridgeValley = value; }
        }

        private double apparentRidges = 0.1;
        [Editor(typeof(NumericScrollBarEditor), typeof(UITypeEditor))]
        [Range(0, 0.5, 0.001)]
        public double ApparentRidges
        {
            get { return apparentRidges; }
            set { apparentRidges = value; }
        }

        private int isoPhotosNum = 20;
        [Editor(typeof(NumericScrollBarEditor), typeof(UITypeEditor))]
        [Range(1, 100, 1)]
        public int IsoPhotosNum
        {
            get { return isoPhotosNum; }
            set { isoPhotosNum = value; }
        }

        private int topoLineNum = 20;
        [Editor(typeof(NumericScrollBarEditor), typeof(UITypeEditor))]
        [Range(1, 100, 1)]
        public int TopoLineNum
        {
            get { return topoLineNum; }
            set { topoLineNum = value; }
        }

        private double topoLineOffset = 0;
        [Editor(typeof(NumericScrollBarEditor), typeof(UITypeEditor))]
        [Range(-1, 1, 0.01)]
        public double TopoLineOffset
        {
            get { return topoLineOffset; }
            set { topoLineOffset = value; }
        }

        #region 不要了
        //private EnumLine lineType = EnumLine.Contour;
        //public EnumLine LineType
        //{
        //    get
        //    {
        //        return lineType;
        //    }
        //    set
        //    {
        //        lineType = value;
        //    }
        //}

        //private double ridgeValleythreshold1 = 35;
        //private double ridgeValleythreshold2 = -7;

        //public double RidgeValleythreshold1
        //{
        //    get { return ridgeValleythreshold1; }
        //    set { ridgeValleythreshold1 = value; }
        //}
        //public double RidgeValleythreshold2
        //{
        //    get { return ridgeValleythreshold2; }
        //    set { ridgeValleythreshold2 = value; }
        //}


        //private double demarcatingthreshold = 30;


        //public double Demarcatingthreshold
        //{
        //    get { return demarcatingthreshold; }
        //    set { demarcatingthreshold = value; }
        //}


        //private double hightLightthreshold = 10; 
        //public double HightLightthreshold
        //{
        //    get { return hightLightthreshold; }
        //    set { hightLightthreshold = value; }
        //}

        //private double contourthreshold = 0;
        //public double Contourthreshold
        //{
        //    get { return contourthreshold; }
        //    set { contourthreshold = value; }
        //}

        //private double silluhoutethreshold = 0;
        //public double Silluhoutethreshold
        //{
        //    get { return silluhoutethreshold; }
        //    set { silluhoutethreshold = value; }
        //}

        //private double silhouetteAngle = Math.PI / 2;
        //public double SilhouetteAngle
        //{
        //    get { return silhouetteAngle; }
        //    set { silhouetteAngle = value; }
        //}

        //private bool exteriorSilhouette = false;
        //public bool ExteriorSilhouette
        //{
        //    get
        //    {
        //        return exteriorSilhouette;
        //    }
        //    set
        //    {
        //        exteriorSilhouette=value ;
        //    }
        //}


        //private bool occludingCountour = false;
        //public bool OccludingCountour
        //{
        //    get
        //    {
        //        return occludingCountour;
        //    }
        //    set
        //    {
        //        occludingCountour = value;
        //    }
        //}


        //private bool suggestiveCountour = false;
        //public bool SuggestiveCountour
        //{
        //    get
        //    {
        //        return suggestiveCountour;
        //    }
        //    set
        //    {
        //        suggestiveCountour = value;
        //    }
        //}


        //private bool suggestiveHighLight = false;
        //public bool SuggestiveHighLight
        //{
        //    get
        //    {
        //        return suggestiveHighLight;
        //    }
        //    set
        //    {
        //        suggestiveHighLight = value;
        //    }
        //}

        //private bool pricipalHighLightR = false;
        //public bool PricipalHighLightR
        //{
        //    get
        //    {
        //        return pricipalHighLightR;
        //    }
        //    set
        //    {
        //        pricipalHighLightR = value;
        //    }
        //}

        //private bool pricipalHighLightV = false;
        //public bool PricipalHighLightV
        //{
        //    get
        //    {
        //        return pricipalHighLightV;
        //    }
        //    set
        //    {
        //        pricipalHighLightV = value;
        //    }
        //}

        //private bool ridges = false;
        //public bool Ridges
        //{
        //    get
        //    {
        //        return ridges;
        //    }
        //    set
        //    {
        //        ridges = value;
        //    }
        //}
        //private bool vallyes = false;
        //public bool Vallyes
        //{
        //    get
        //    {
        //        return vallyes;
        //    }
        //    set
        //    {
        //        vallyes = value;
        //    }
        //}

        

        //private bool apparentRidge = false;
        //public bool ApparentRidge
        //{
        //    get
        //    {
        //        return apparentRidge;
        //    }
        //    set
        //    {
        //        apparentRidge = value;
        //    }
        //}

        //private bool k0 = false;
        //public bool K0
        //{
        //    get
        //    {
        //        return k0;
        //    }
        //    set
        //    {
        //        k0 = value;
        //    }
        //}

        //private bool h0 = false;
        //public bool H0
        //{
        //    get
        //    {
        //        return h0;
        //    }
        //    set
        //    {
        //        h0 = value;
        //    }
        //}
      
       
        //private bool dwkr = false;
        //public bool Dwkr
        //{
        //    get
        //    {
        //        return dwkr;
        //    }
        //    set
        //    {
        //        dwkr = value;
        //    }
        //}
        //private bool boundary = false;
        //public bool Boundary
        //{
        //    get
        //    {
        //        return boundary;
        //    }
        //    set
        //    {
        //        boundary = value;
        //    }
        //}


        //private bool isoPoto = false;
        //public bool IsoPoto
        //{
        //    get
        //    {
        //        return isoPoto;
        //    }
        //    set
        //    {
        //        isoPoto = value;
        //    }
        //}


        //private bool isoTopo = false;
        //public bool IsoTopo
        //{
        //    get
        //    {
        //        return isoTopo;
        //    }
        //    set
        //    {
        //        isoTopo = value;
        //    }
        //}


        //private bool drawHiddenLine = false;
        //public bool DrawHiddenLine
        //{
        //    get
        //    {
        //        return drawHiddenLine;
        //    }
        //    set
        //    {
        //        drawHiddenLine = value;
        //    }
        //}

        //private bool trimInsideCountour = false;
        //public bool TrimInsideCountour
        //{
        //    get
        //    {
        //        return trimInsideCountour;
        //    }
        //    set
        //    {
        //        trimInsideCountour = value;
        //    }
        //}
        #endregion

        public enum MeshStyle
        {
            None,
            Basic,
            CurvColor,
            CurvGray
        }
    }
}
