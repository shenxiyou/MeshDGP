using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace GraphicResearchHuiZhao 
{
    public static partial class TriMeshUtil
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

        public static string[] GetAllMethodName(Type type)
        {
            
            List<string> list = new List<string>();
            foreach (var method in type.GetMethods())
            {
                if (method.DeclaringType.Name == type.Name &&
                    method.Name != MethodInfo.GetCurrentMethod().Name)
                {
                    list.Add(MethodToString(method, true, true));
                }
            }
            return list.ToArray();
        }

        public static string MethodToString(MethodInfo method, bool withRet, bool withParams)
        {
            StringBuilder sb = new StringBuilder();
            if (withRet)
            {
                sb.Append(string.Format("{0,-15}", method.ReturnType.Name));
            }
            sb.Append(method.Name);
            if (withParams)
            {
                sb.Append('(');
                foreach (var param in method.GetParameters())
                {
                    sb.Append(param.ParameterType.Name);
                    sb.Append(',');
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(')');
            }
            return sb.ToString();
        }
    }
}
