using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace GraphicResearchHuiZhao
{
    public partial class IOHuiZhao
    {
        //每个文件的格式是“路径+模型名称+类型。例如workspace/bunny.Lc.matrix "

        public string Workpath = "..\\..\\WorkSpace\\";

        public string BasicPath = "..\\..\\WorkSpace\\";

        public string GetFileName(EnumFileType type, string modelName)
        {
            string path = GetPath() + modelName + "//";

            switch (type)
            {
                case EnumFileType.MatrixG:
                    break;

                case EnumFileType.MatrixF:
                    break;
                case EnumFileType.Lc:
                    path = path + "Lc.Matrix.";
                    break;
                case EnumFileType.B:
                    path = path + "B.Matrix.";
                    break;
                case EnumFileType.E:
                    path = path + "E.Matrix.";
                    break;

                case EnumFileType.G:
                    path = path + "E.Matrix.";
                    break;

                case EnumFileType.EigenMatrix:
                    path = path + "matrix.Matrix.";
                    break;
            }

            return path;
        }

        public string GetPath()
        {
            string path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + Workpath;
            return path;
        }


        public string GetFileMatrix(string modelName)
        {
            string fileName = Path.GetFileNameWithoutExtension(modelName);
            string path = GetPath() + fileName + ".matrix";

            return path;
        }

        public string GetFileVectorSolution(string modelName)
        {
            string fileName = Path.GetFileNameWithoutExtension(modelName);
            string path = GetPath() + fileName + "_solution.vector";
            return path;
        }

        public string GetFileVector(string modelName)
        {
            string fileName = Path.GetFileNameWithoutExtension(modelName);
            string path = GetPath() + fileName + ".vector";
            return path;
        }

        public string GetFileEigen(string modelName)
        {
            string fileName = Path.GetFileNameWithoutExtension(modelName);
            string path = GetPath() + fileName + ".eigens";
            return path;
        }
        public string GetName(string fileName)
        {
             
            String[] tokens = fileName.Split("\\".ToCharArray());
            String lastToekn = tokens[tokens.Length - 1];
            int dotIndex = lastToekn.IndexOf(lastToekn);
            string modelName = lastToekn.Substring(0, dotIndex);
            return modelName;
        }
    }
}
