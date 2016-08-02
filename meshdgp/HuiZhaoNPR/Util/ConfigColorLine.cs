using System; 
using System.Data;
using System.Drawing;
 
 

namespace GraphicResearchHuiZhao
{
    public class ConfigColorLine
    {
        public Color ApparentRidges
        {
            get { return NPRLines.All[0].Color; }
            set { NPRLines.All[0].Color = value; }
        }
        public Color BoundaryLine
        {
            get { return NPRLines.All[1].Color; }
            set { NPRLines.All[1].Color = value; }
        }
        public Color Contours
        {
            get { return NPRLines.All[2].Color; }
            set { NPRLines.All[2].Color = value; }
        }
        public Color DwKrLine
        {
            get { return NPRLines.All[3].Color; }
            set { NPRLines.All[3].Color = value; }
        }
        public Color H0Line
        {
            get { return NPRLines.All[4].Color; }
            set { NPRLines.All[4].Color = value; }
        }
        public Color HighlightLine
        {
            get { return NPRLines.All[5].Color; }
            set { NPRLines.All[5].Color = value; }
        }
        public Color Isophotes
        {
            get { return NPRLines.All[6].Color; }
            set { NPRLines.All[6].Color = value; }
        }
        public Color K0Line
        {
            get { return NPRLines.All[7].Color; }
            set { NPRLines.All[7].Color = value; }
        }
        public Color PrincipalHighlightRidges
        {
            get { return NPRLines.All[8].Color; }
            set { NPRLines.All[8].Color = value; }
        }
        public Color PrincipalHighlightValleys
        {
            get { return NPRLines.All[9].Color; }
            set { NPRLines.All[9].Color = value; }
        }
        public Color RidgesLine
        {
            get { return NPRLines.All[10].Color; }
            set { NPRLines.All[10].Color = value; }
        }
        public Color Silluhoute
        {
            get { return NPRLines.All[11].Color; }
            set { NPRLines.All[11].Color = value; }
        }
        public Color SuggestCountour
        {
            get { return NPRLines.All[12].Color; }
            set { NPRLines.All[12].Color = value; }
        }
        public Color TopoLines
        {
            get { return NPRLines.All[13].Color; }
            set { NPRLines.All[13].Color = value; }
        }
        public Color ValleysLine
        {
            get { return NPRLines.All[14].Color; }
            set { NPRLines.All[14].Color = value; }
        }
    }
}
