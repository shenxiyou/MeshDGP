using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace GraphicResearchHuiZhao
{
    public  class GlobalSetting
    {
        private static DisplaySetting meshDisplaySetting= new DisplaySetting("Mesh Display Setting");

        private static SettingTexture textureSetting = new SettingTexture();

        private static SettingLight settingLight = new SettingLight();
        private static SettingLightDirect  light0Setting = new SettingLightDirect(0,0,4,true);
        private static SettingLightDirect light1Setting = new SettingLightDirect(10,10,-4,true);
        private static SettingLightDirect light2Setting = new SettingLightDirect(0,-1,0,true);
        private static SettingLightDirect light3Setting = new SettingLightDirect(-1, 1, 0, true);

        private static SettingLightDirect light4Setting = new SettingLightDirect(-10, 10, -10, false);

        private static SettingLightDirect light5Setting = new SettingLightDirect(-10, 10, -5, false);

     

        private static SettingLightSpot spotlightSetting = new SettingLightSpot(0,0,1,false);
        private static SettingLightPoint pointlightSetting = new SettingLightPoint(0,0,1,false);

        private static SettingMaterial materialSetting = new SettingMaterial();


        private static SettingClipPlane clipPlaneSetting = new SettingClipPlane();

        private static SettingFog fogSettting = new SettingFog();

        private static SettingTest testSetting = new SettingTest();

        private static SettingBlend blendSetting = new SettingBlend();

        private static SettingEnableCaps enableCapsSetting = new SettingEnableCaps();

        public static SettingEnableCaps EnalbeCapsSetting
        {
            get
            {
                return enableCapsSetting;
            }
        }

        public static SettingBlend BlendSetting
        {
            get
            {
                return blendSetting;
            }
        }

        public static SettingTest TestSetting
        {
            get
            {
                return testSetting;
            }

        }

        public static DisplaySetting DisplaySetting
        {
            get
            {
                return meshDisplaySetting;
            }

        }

        public static SettingTexture TextureSetting
        {
            get
            {
                return textureSetting;
            }
        }

        public static SettingLightDirect Light0Setting
        {
            get
            {
                return light0Setting;
            }
        }

        public static SettingLight SettingLight
        {
            get
            {
                return settingLight;
            }
        }

        public static SettingLightDirect Light1Setting
        {
            get
            {
                return light1Setting;
            }
        }

        public static SettingLightDirect Light2Setting
        {
            get
            {
                return light2Setting;
            }
        }

        public static SettingLightDirect Light3Setting
        {
            get
            {
                return light3Setting;
            }
        }

        public static SettingLightDirect Light4Setting
        {
            get
            {
                return light4Setting;
            }
        }

        public static SettingLightDirect Light5Setting
        {
            get
            {
                return light5Setting;
            }
        }

        public static SettingLightSpot Light6Setting
        {
            get
            {
                return spotlightSetting;
            }
        }

        public static SettingLightPoint Light7Setting
        {
            get
            {
                return pointlightSetting;
            }
        }


        public static SettingMaterial MaterialSetting
        {
            get
            {
                return materialSetting;
            }
        }


        public static SettingClipPlane ClipPlaneSetting
        {
            get
            {
                return clipPlaneSetting;
            }
            
        }

        public static SettingFog FogSetting
        {
            get
            {
                return fogSettting;
            }
        }
       

    }
}
