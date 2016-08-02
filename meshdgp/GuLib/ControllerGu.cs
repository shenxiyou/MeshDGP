using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class ControllerGu
    {
        private static ControllerGu singleton = new ControllerGu();


        public static ControllerGu Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new ControllerGu();
                return singleton;
            }
        }


        protected virtual void OnChanged(EventArgs e)
        {
            GlobalData.Instance.OnChanged(e);
        }

        public void Run(TriMesh mesh, EnumGu type)
        {
            switch (type)
            {
                case EnumGu.GlobalConformal:
                    MappingGauss(mesh);
                    break;
                case EnumGu.GenusOneSphere:
                    MappingSphericalTuette(mesh);
                    break;
                case EnumGu.GaussMap:
                    MappingGauss(mesh);
                    break;
            }
        }

        public void MappingGauss(TriMesh mesh) 
        {
            GenusOneSphere hSphere = new GenusOneSphere(mesh);
            hSphere.MappingGauss(mesh);
        }

        public void MappingSphericalTuette(TriMesh mesh)
        {
            GenusOneSphere hSphere = new GenusOneSphere(mesh);
            hSphere.MappingSphericalTuette(mesh);
        }
    }
}
