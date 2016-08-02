using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicResearchHuiZhao
{
    public struct MayaObjPropId
    {
        public string name;
        public Type type;

        public MayaObjPropId(string inName, Type inType)
        {
            name = inName;
            type = inType;
        }
    };


    public struct MayaObjPropVal
    {
        public Type type;
        public string value;

        public MayaObjPropVal(Type inTP, string inVal)
        {
            type = inTP;
            value = inVal;
        }
    }


    public class MayaObject
    {
        public string name;
        public string type;
        public Dictionary<string, MayaObjPropVal> properties;
    }

}
