using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class ConfigDB
    {
        private static ConfigDB singleton = new ConfigDB();


        public static ConfigDB Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new ConfigDB();
                return singleton;
            }
        }


        private string databaseName = "SqlLite.net";

        public string DatabaseName
        {
            get
            {
                return databaseName;
            }
            set
            {
                databaseName = value;
            }
        }

    }
}
