using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicResearchHuiZhao 
{
    public class ConfigMaya
    {

        private static ConfigMaya singleton = new ConfigMaya();


        public static ConfigMaya Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new ConfigMaya();
                return singleton;
            }
        }

        private string selectMesh = @" dagpath.node.apiTypeStr == ""kMesh""";

        public string SelectMesh
        {
            get
            {
                return selectMesh;
            }
            set
            {
                selectMesh = value;
            }

        }


        private string selectPolygon = @" 
{
  if (dagpath.node.apiTypeStr == ""kMesh"")
  {
    var m = new MFnMesh(dagpath);

    return (m.numPolygons > 10);
  }

  return false;
}";

        public string SelectPolygon
        {
            get
            {
                return selectPolygon;
            }
            set
            {
                selectPolygon = value;
            }

        }



        private string selectName = @"dagpath.partialPathName.StartsWith(""collision"")";

        public string SelectName
        {
            get
            {
                return selectName;
            }
            set
            {
                selectName = value;
            }

        }






        private string selectAll = "true";

        public string SelectAll
        {
            get
            {
                return selectAll;
            }
            set
            {
                selectAll = value;
            }

        }

        private double move =1.0d;

        public double Move
        {
            get
            {
                return move;
            }
            set
            {
                move = value;
            }

        }

    }
}
