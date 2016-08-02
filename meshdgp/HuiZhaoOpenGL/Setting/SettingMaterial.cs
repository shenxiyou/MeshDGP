using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace GraphicResearchHuiZhao 
{
    public class SettingMaterial
    {

        private bool enable = true;
        public bool Enable
        {
            get
            {
                return enable;
            }
            set
            {
                enable = value;
            }
        }

        private Color materialAmbient = Color.Black;
        public Color MaterialAmbient
        {
            get
            {
                return materialAmbient;
            }
            set
            {
                materialAmbient = value;
            }
        }

        private Color materialDiffuse = Color.CornflowerBlue;
        public Color MaterialDiffuse
        {
            get
            {
                return materialDiffuse;
            }
            set
            {
                materialDiffuse = value;
            }
        }


        private Color materialSpecular = Color.Black;
        public Color MaterialSpecular
        {
            get
            {
                return materialSpecular;
            }
            set
            {
                materialSpecular = value;
            }
        }

        private Color materialEmission = Color.Black;
        public Color MaterialEmission
        {
            get
            {
                return materialEmission;
            }
            set
            {
                materialEmission = value;
            }
        }

        private int materialShininess = 128;
        public int MaterialShininess
        {
            get
            {
                return materialShininess;
            }
            set
            {
                materialShininess = value;
            }
        }




        private Color backMaterialAmbient = Color.Black;
        public Color BackMaterialAmbient
        {
            get
            {
                return backMaterialAmbient;
            }
            set
            {
                backMaterialAmbient = value;
            }
        }

        private Color backMaterialDiffuse = Color.Tomato;
        public Color BackMaterialDiffuse
        {
            get
            {
                return backMaterialDiffuse;
            }
            set
            {
                backMaterialDiffuse = value;
            }
        }


        private Color backMaterialSpecular = Color.Black;
        public Color BackMaterialSpecular
        {
            get
            {
                return backMaterialSpecular;
            }
            set
            {
                backMaterialSpecular = value;
            }
        }

        private Color backMaterialEmission = Color.Black;
        public Color BackMaterialEmission
        {
            get
            {
                return backMaterialEmission;
            }
            set
            {
                backMaterialEmission = value;
            }
        }

        private int backMaterialShiness = 128;
        public int BackMaterialShiness
        {
            get
            {
                return backMaterialShiness;
            }
            set
            {
                backMaterialShiness = value;
            }
        }





    }
}
