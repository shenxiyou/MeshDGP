using System;
using System.Collections.Generic;
using System.Text; 
using System.Reflection;

namespace GraphicResearchHuiZhao
{
    public static class NPRLines
    {
        public static LineBase[] All;

        static NPRLines()
        {
            All = new LineBase[15];
            All[0] = new ApparentRidges();
            All[1] = new BoundaryLine();
            All[2] = new Contours();
            All[3] = new DwKrLine();
            All[4] = new H0Line();
            All[5] = new HighlightLine();
            All[6] = new Isophotes();
            All[7] = new K0Line();
            All[8] = new PrincipalHighlightRidges();
            All[9] = new PrincipalHighlightValleys();
            All[10] = new RidgesLine();
            All[11] = new Silluhoute();
            All[12] = new SuggestCountour();
            All[13] = new TopoLines();
            All[14] = new ValleysLine();
        }

        public static LineBase[] CreateAllDerivedObject()
        {
            List<LineBase> list = new List<LineBase>();
            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (var item in assembly.GetTypes())
            {
                if (item.BaseType == typeof(LineBase))
                {
                    LineBase obj = (LineBase)Activator.CreateInstance(item);
                    list.Add(obj);
                }
            }
            return list.ToArray();
        }
    }


}
