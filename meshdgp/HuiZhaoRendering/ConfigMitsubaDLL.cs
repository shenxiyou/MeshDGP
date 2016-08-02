using System;
using System.Collections.Generic; 
using System.Text; 
using System.ComponentModel;
namespace GraphicResearchHuiZhao
{
    public class ConfigMitsubaDLL
    {

        private static ConfigMitsubaDLL singleton = new ConfigMitsubaDLL();


        public static ConfigMitsubaDLL Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new ConfigMitsubaDLL();
                return singleton;
            }
        }

        private string remoteIP = "10.251.211.123";
          [Category("Basic")]
        public string RemoteIP
        {
            get
            {
                return remoteIP;
            }
            set
            {
                remoteIP = value;
            }
        }

        private string cmdFile = "C:\\software\\Mitsuba\\mitsuba.exe";
          [Category("Basic")]
        public string CmdFile
        {
            get
            {
                return cmdFile;
            }
            set
            {
                cmdFile = value;
            }
        }



        private string workPath = "./";

        public string WorkPath
        {
            get
            {
                return workPath;
            }
            set
            {
                workPath = value;
            }
        }



        private string modelPara = "-DmodelFile=";
         [Category("Bunny")]
        public string ModelPara
        {
            get
            {
                return modelPara;
            }
            set
            {
                modelPara = value;
            }
        }

        private string sceneFile = "C:\\workspace\\scene.xml";
         [Category("Bunny")]
        public string SceneFile
        {
            get
            {
                return sceneFile;
            }
            set
            {
                sceneFile = value;
            }
        }

        private string outputImage = "C:\\workspace\\renderedPic.png";

        public string OutputImage
        {
            get
            {
                return outputImage;
            }
            set
            {
                outputImage = value;
            }
        }

        private string modelFile = "C:\\workspace\\armidillo.obj";

        public string ModelFile
        {
            get
            {
                return modelFile;
            }
            set
            {
                modelFile = value;
            }
        }


        private bool overWrite =true;

        public bool OverWrite
        {
            get
            {
                return overWrite;
            }
            set
            {
                overWrite = value;
            }
        }


        private string modelDir = "G:\\flowdata\\BunnyConformalFlow";
         [Category("Bunny")]
        public string ModelDir
        {
            get
            {
                return modelDir;
            }
            set
            {
                modelDir = value;
            }
        }


        private string outputImageDir = "G:\\flowdata\\BunnyPic";
          [Category("Bunny")]
        public string OutputImageDir
        {
            get
            {
                return outputImageDir;
            }
            set
            {
                outputImageDir = value;
            }
        }


          private string sceneFileDir = "G:\\workspace\\bunny";
          [Category("Bunny")]
          public string SceneFileDir
          {
              get
              {
                  return sceneFileDir;
              }
              set
              {
                  sceneFileDir = value;
              }
          }





        private string deformDataDir =  "G:\\workspace\\DeformRender\\RenderData";
         [Category("Deform")]
        public string DeformDataDir
        {
            get
            {
                return deformDataDir;
            }
            set
            {
                deformDataDir = value;
            }
        }


         private string deformSceneDir = "G:\\workspace\\DeformRender\\scenewireframe";
         [Category("Deform")]
         public string DeformSceneDir
         {
             get
             {
                 return deformSceneDir;
             }
             set
             {
                 deformSceneDir = value;
             }
         }

         private string deformImageDir = "G:\\workspace\\DeformRender\\images";
         [Category("Deform")]
        public string DeformImageDir
        {
            get
            {
                return deformImageDir;
            }
            set
            {
                deformImageDir = value;
            }
        }









        private string deformModelFile = "-DmodelFile=";
        [Category("Deform")]
        public string DeformModelFile
        {
            get
            {
                return deformModelFile;
            }
            set
            {
                deformModelFile = value;
            }
        }

        private string deformModelColorName = "  -DmodelColorName=\"0.95 0.95 0.95\"  ";
        [Category("Deform")]
        public string DeformModelColorName
        {
            get
            {
                return deformModelColorName;
            }
            set
            {
                deformModelColorName = value;
            }
        }

        private string deformModelColorValue = "  ";
        [Category("Deform")]
        public string DformModelColorValue
        {
            get
            {
                return deformModelColorValue;
            }
            set
            {
                deformModelColorValue = value;
            }
        }


        private string deformEdgeColorName = "   -DedgeColorName==\"0.0 0.0 0.0\" ";
        [Category("Deform")]
        public string DeformEdgeColorName
        {
            get
            {
                return deformEdgeColorName;
            }
            set
            {
                deformEdgeColorName = value;
            }
        }

        private string deformEdgeColorValue = " ";
        [Category("Deform")]
        public string DeformEdgeColorValue
        {
            get
            {
                return deformEdgeColorValue;
            }
            set
            {
                deformEdgeColorValue = value;
            }
        }


        private string deformEdgeWidth = "$edgeWidth";
        [Category("Deform")]
        public string DeformEdgeWidth
        {
            get
            {
                return deformEdgeWidth;
            }
            set
            {
                deformEdgeWidth = value;
            }
        }


        private string deformWidth = "  -Dwidth=2560 ";
        [Category("Deform")]
        public string DeformWidth
        {
            get
            {
                return deformWidth;
            }
            set
            {
                deformWidth = value;
            }
        }


        private string deformHeight = "  -Dheight=1440  ";
        [Category("Deform")]
        public string DeformHeight
        {
            get
            {
                return deformHeight;
            }
            set
            {
                deformHeight = value;
            }
        }

        private bool goOn = false;
        [Category("Deform")]
        public bool GoOn
        {
            get
            {
                return goOn;
            }
            set
            {
                goOn = value;
            }
        }
    }
}
