using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace GraphicResearchHuiZhao 
{
    public static class MethodCollector
    {
        public static Type[] GetAllMethods<TInterface>()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            List<Type> list = new List<Type>();
            foreach (var type in assembly.GetExportedTypes())
            {
                if (!type.IsAbstract)
                {
                    foreach (var item in type.GetInterfaces())
                    {
                        if (item == typeof(TInterface))
                        {
                            list.Add(type);
                            break;
                        }
                    }
                }
            }
            return list.ToArray();
        }
    }
}
