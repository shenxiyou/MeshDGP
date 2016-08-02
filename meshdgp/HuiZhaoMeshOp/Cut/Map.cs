using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class Map<TKey, TValue> : List<Map<TKey,TValue>.Pair>
    {
        public Map()
        {
            
        }

        public Map(IEnumerable<Pair> collection):base(collection)
        {

        }

        public void Add(TKey key, TValue value)
        {
            this.Add(new Pair { Key = key, Value = value });
        }

        public TValue[] GetValues(TKey key)
        {
            List<TValue> list = new List<TValue>();
            foreach (var item in this)
            {
                if (item.Key.Equals(key))
                {
                    list.Add(item.Value);
                }
            }
            return list.ToArray();
        }

        public class Pair
        {
            public TKey Key;
            public TValue Value;
        }
    }
}
