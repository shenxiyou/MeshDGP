using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class Segementation
    {
        public static void Run(TriMesh mesh, EnumSegemenation type)
        {
            switch (type)
            {
                case EnumSegemenation.AreaGrow:
                    SegementationAreaGrow seg = new SegementationAreaGrow(mesh);

                    seg.FaceInit();
                    for (int i = 0; i < 40; i++)
                    {
                        seg.AreaGrowByFace();


                       // OnChanged(EventArgs.Empty);
                    }
                    break;
                case EnumSegemenation.KMean:
                    SegementationKMean kmean = new SegementationKMean(mesh);

                    kmean.Init();
                    kmean.KMeans();
                    break;
                case EnumSegemenation.Region:
                    SegementationRegion region = new SegementationRegion(mesh);
                    region.Run();
                    break;

            }
        }
    }
}
