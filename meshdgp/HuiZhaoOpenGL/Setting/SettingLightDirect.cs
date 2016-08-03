using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Design;

namespace GraphicResearchHuiZhao
{
    public class SettingLightDirect
    {
        public SettingLightDirect(double x, double y, double z, bool enabled)
        {
            this.lightPosition = new Vector3D(x, y, z);
            this.enabled = enabled;
        }

        public SettingLightDirect(Vector3D position, bool enabled)
        {
            this.lightPosition = position;
            this.enabled = enabled;
        }

        private Vector3D lightPosition;

        public Vector3D LightPosition
        {
            get { return lightPosition; }
            set { lightPosition = value; }
        }

        private bool enabled = true;
        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                enabled = value;
            }
        }

        private Color diffuseColor = Color.White;
        public Color DiffuseColor
        {
            get
            {
                return diffuseColor;
            }
            set
            {
                diffuseColor = value;
            }
        }

        private Color ambientColor = Color.Black;
        public Color AmbientColor
        {
            get
            {
                return ambientColor;
            }
            set
            {
                ambientColor = value;
            }
        }

        private Color specularColor = Color.White;
        public Color SpecularColor
        {
            get
            {
                return specularColor;
            }
            set
            {
                specularColor = value;
            }
        }

       


        private bool enabledDraw = false;
        public bool EnabledDraw
        {
            get
            {
                return enabledDraw;
            }
            set
            {
                enabledDraw = value;
            }
        }



    }
}
