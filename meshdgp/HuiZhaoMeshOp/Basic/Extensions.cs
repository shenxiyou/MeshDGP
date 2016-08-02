using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public static class Extensions
    {
        public static T[] ToArray<T>(IEnumerable<T> c)
        {
            List<T> list = new List<T>();
            foreach (T item in c)
            {
                list.Add(item);
            }
            return list.ToArray();
        }
    }
}
