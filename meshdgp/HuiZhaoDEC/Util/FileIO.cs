using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class FileIO
    {
        private static FileIO singleton = new FileIO();

        public static FileIO Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new FileIO();
                return singleton;
            }
        }

        private FileIO()
        {

        }


        public string GetPath()
        {
            string path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "..\\..\\WorkSpace\\";
            return path;
        }


        public void Write(ref SparseMatrixDouble A, string fileName)
        {

        }


        public void Read(out SparseMatrixDouble A, string fileName)
        {
            A = null;
            // A = new dSparseMatrix();
        }


        public void Write(ref DenseMatrixDouble A, string fileName)
        {

        }

        public void Read(out DenseMatrixDouble A, string fileName)
        {
            A = new DenseMatrixDouble();
        }

    }
}
