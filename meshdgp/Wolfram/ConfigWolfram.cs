using System;
using System.Collections.Generic; 
using System.Text; 
using System.ComponentModel;
namespace GraphicResearchHuiZhao
{
    public class ConfigWolfram
    {

        private static ConfigWolfram singleton = new ConfigWolfram();


        public static ConfigWolfram Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new ConfigWolfram();
                return singleton;
            }
        }

        private string defaultFile = @"C:\\wallpaper\\WallPapers\\1.jpg";
        [Category("Basic")]
        public string DefaultFile
        {
            get
            {
                return defaultFile;
            }
            set
            {
                defaultFile = value;
            }
        }

 
    }
}
