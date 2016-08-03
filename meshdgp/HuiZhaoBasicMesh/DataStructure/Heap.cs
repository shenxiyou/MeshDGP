using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    class HeapKey : IComparable<HeapKey>
    {
        public HeapKey(double value)
        {
            
            Value = value;
        }

     
        public double Value { get; private set; }

        public int CompareTo(HeapKey other)
        {
            

            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            var result = Value.CompareTo(other.Value);
            if (result == 0)
                result = 1;
            return result;
        }
    }

}
