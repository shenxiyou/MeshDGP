//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace GraphicResearchHuiZhao
//{
    
//    public class TriIndexComparer : IEqualityComparer<TriIndex>
//    {
       

//        public bool Equals(TriIndex x, TriIndex y)
//        {

//            return (x.i == y.i && x.j == y.j && x.k == y.k);
//        }

//        public int GetHashCode(TriIndex obj)
//        {
//            return obj.i + obj.j + obj.k;
//        }

        
//    } 
//    public struct TriIndex
//    {
//        public int i, j, k;
//        public int i2, j2, k2;
//        public TriIndex(int c1, int c2, int c3)
//        {
//            i = i2 = c1;
//            j = j2 = c2;
//            k = k2 = c3;
//            if (j < i) { int t = i; i = j; j = t; }
//            if (k < i) { int t = i; i = k; k = t; }
//            if (k < j) { int t = j; j = k; k = t; }
//        }
//    } 
   
//}
