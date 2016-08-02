using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public partial class MatlabProxy
    {
        private static MatlabProxy singleton = new MatlabProxy();
        public static MatlabProxy Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new MatlabProxy();
                return singleton;
            }
        }


        private MLApp.MLApp matlab = new MLApp.MLApp();


        //        function [x,y] = myfunc(a,b,c) 
        //x = a + b; 
        //y = sprintf('Hello %s',c); 

        public void Test1()
        {
            // Create the MATLAB instance 

            //   matlab.Visible = 0;

            // Change to the directory where the function is located 
            matlab.Execute(@"cd C:\Develop");

            // Define the output 
            object result = null;

            // Call the MATLAB function myfunc
            matlab.Feval("myfunc", 2, out result, 3.14, 42.0, "world");

            // Display result 
            object[] res = result as object[];

            Console.WriteLine(res[0]);
            Console.WriteLine(res[1]);
            Console.ReadLine();




        }

        public void Test2()
        {
            matlab.Execute(@"cd C:\Develop\gptoolbox-master\mesh");
            string sss = matlab.Execute(@"help arap");

            sss = matlab.Execute(@"[V,F,UV,TF,N,NF] = readOBJ('C:\\Develop\\bu_head.obj');L = cotmatrix(V,F)");


         
         //   GetMatrixV1("L" );
            //  GetArrayV1("F", out row, out col);
         //    Console.WriteLine(sss);

            //double[] data = new double[5] { 0, 0, 2, 4, 4 };
            //double[,] data=new double[3,4];
            //data[0, 0] = 1;

            //matlab.PutWorkspaceData("test", "base", data);
        }


        public void Test3()
        {
            System.Array pr = new double[4];
            pr.SetValue(11, 0);
            pr.SetValue(12, 1);
            pr.SetValue(13, 2);
            pr.SetValue(14, 3);

            System.Array pi = new double[4];
            pi.SetValue(1, 0);
            pi.SetValue(2, 1);
            pi.SetValue(3, 2);
            pi.SetValue(4, 3);

            matlab.PutFullMatrix("a", "base", pr, pi);

            System.Array prresult = new double[4];
            System.Array piresult = new double[4];

            matlab.GetFullMatrix("a", "base", ref prresult, ref piresult);
            PrintArray((double[])prresult);
        }






        public void Test4()
        {
            System.Array pr = new double[2, 2];

            pr.SetValue(11, 0, 0);
            pr.SetValue(12, 0, 1);
            pr.SetValue(13, 1, 0);
            pr.SetValue(14, 1, 1);

            System.Array pi = new double[2, 2];
            //pi.SetValue(1, 0);
            //pi.SetValue(2, 1);
            //pi.SetValue(3, 2);
            //pi.SetValue(4, 3);

            matlab.PutFullMatrix("a", "base", pr, pi);

            System.Array prresult = new double[2, 2];
            System.Array piresult = new double[2, 2];

            matlab.GetFullMatrix("a", "base", ref prresult, ref piresult);

            Console.WriteLine(prresult.GetValue(0, 1).ToString());
            Console.WriteLine(piresult.GetValue(0, 1).ToString());



        }


        public void Test5()
        {
            matlab.Execute(@"cd C:\Develop\gptoolbox-master\mesh");
            // string sss = matlab.Execute(@"help readOBJ ");

            object result = null;
            matlab.Feval("readOBJ", 1, out result, "C:\\Develop\\bu_head.obj");
            //    matlab.Feval("cotangent", 2, out result, V , F );

            //   string sss = matlab.Execute(@"help cotmatrix ");

            object[] res = result as object[];

            Console.WriteLine(res.Length);
            Console.WriteLine(res[0].ToString());


            // Console.WriteLine(sss);
        }








        public string TestCore(EnumMatlabTest type)
        {
            switch (type)
            {
                case EnumMatlabTest.test1:
                    Test1();
                    break;
                case EnumMatlabTest.test2:
                    Test2();
                    break;
                case EnumMatlabTest.test3:
                    Test3();
                    break;
                case EnumMatlabTest.test4:
                    Test4();
                    break;
                case EnumMatlabTest.test5:
                    Test5();
                    break;
                //case EnumMatlabTest.test6:
                //    Test6();
                //    break;
            }

            return null;
        }



        public Eigen eigen = null;


        public bool TestMesh(TriMesh mesh, EnumMatlabMesh type)
        {
            if (mesh == null)
                return false;


            switch (type)
            {
                case EnumMatlabMesh.GetLap:
                    Test1();
                    break;
                case EnumMatlabMesh.OutPutLap:
                    Test2();
                    break;
                
            }

            return true;
        }
    }
}

