using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace GraphicResearchHuiZhao 
{
    public class DECMeshDouble
    {
        public SparseMatrixDouble HodgeStar0Form = null;
        public SparseMatrixDouble HodgeStar1Form = null;
        public SparseMatrixDouble HodgeStar2Form = null;
        public SparseMatrixDouble ExteriorDerivative0Form = null;
        public SparseMatrixDouble ExteriorDerivative1Form = null;
        public SparseMatrixDouble Laplace = null;

        public DECMeshDouble(TriMesh mesh)
        {
            Stopwatch clock = new Stopwatch();
            clock.Start();

            HodgeStar0Form = DECDouble.Instance.BuildHodgeStar0Form(mesh);

            clock.Stop();
            decimal micro = clock.Elapsed.Ticks / 10m;
            Console.WriteLine("Total Building Matrix star0 time cost:{0}", micro);
            clock.Start();
            HodgeStar1Form = DECDouble.Instance.BuildHodgeStar1Form(mesh);

            clock.Stop();
            micro = clock.Elapsed.Ticks / 10m;
            Console.WriteLine("Total Building Matrix star1 time cost:{0}", micro);
            clock.Start();

            ExteriorDerivative0Form = DECDouble.Instance.BuildExteriorDerivative0Form(mesh);
            clock.Stop();
            micro = clock.Elapsed.Ticks / 10m;
            Console.WriteLine("Total Building Matrix d0 time cost:{0}", micro);
            clock.Start();

            ExteriorDerivative1Form = DECDouble.Instance.BuildExteriorDerivative1Form(mesh);
            clock.Stop();
            micro = clock.Elapsed.Ticks / 10m;
            Console.WriteLine("Total Building Matrix d1 time cost:{0}", micro);

        }


    }
}
