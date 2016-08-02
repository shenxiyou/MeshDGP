using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public class ConfigMeshOP
    {
        private static ConfigMeshOP singleton = new ConfigMeshOP(); 

        public static ConfigMeshOP Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new ConfigMeshOP();
                return singleton;
            }
        }

        private int eigenCount=10;
        public int EigenCount
        {
            get
            {
                return eigenCount;
            }
            set
            {
                eigenCount = value;
            }
        }

        private int randomSelect = 100;
        public int RandomSelect
        {
            get
            {
                return randomSelect;
            }
            set
            {
                randomSelect = value;
            }
        }


        private int smoothTaubin = 100;
        public int SmoothTaubinIterative
        {
            get
            {
                return smoothTaubin;
            }
            set
            {
                smoothTaubin = value;
            }
        }

        private double smoothTaubinLamda = 0.1;
        public double SmoothTaubinLamda
        {
            get
            {
                return smoothTaubinLamda;
            }
            set
            {
                smoothTaubinLamda = value;
            }
        }

        private bool smoothTaubinCot = false ;
        public bool SmoothTaubinCot
        {
            get
            {
                return smoothTaubinCot;
            }
            set
            {
                smoothTaubinCot = value;
            }
        }
    }
}
