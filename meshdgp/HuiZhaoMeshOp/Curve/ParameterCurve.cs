using System;
using System.Collections.Generic;
 
using System.Text;
 


namespace GraphicResearchHuiZhao
{
    public partial class ParameterCurve
    {
        private static ParameterCurve instance = null;
        public static ParameterCurve Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ParameterCurve();
                }
                return instance;
            }
        }

        private ParameterCurve()
        {

        }        


        public int VerticesNum = 4000;

        public double pi = 3.14;
        
        
        public TriMesh CreateMeshLine()
        { 
            TriMesh line = new TriMesh();
            for (int t = 0; t < VerticesNum; t++)
            {
              line.Vertices.Add(new VertexTraits(-1 + t * 0.001, 
                                                (-1 + t * 0.001) / Math.Tan(20), 
                                                0));
            }
            return line;
        }

        public TriMesh CreateMeshCricle(double radius)
        { 
            TriMesh circle = new TriMesh();
            for (int t = 0; t < VerticesNum; t++)
            {

                circle.Vertices.Add(new VertexTraits(0.2 * radius * Math.Cos(360 * t / VerticesNum), 
                                                     0.5 * radius * Math.Sin(360 * t / VerticesNum), 
                                                     0));
            }
            return circle;
        }



        public TriMesh CreateDoubleArcEpicycloidLine(double l, double b)
        {
            TriMesh dael = new TriMesh();
            for (int t = 0; t < VerticesNum; t++)
            {
                dael.Vertices.Add(new VertexTraits(3 * b * Math.Cos(t * 360) + l * Math.Cos(3 * t * 360), 
                                                   3 * b * Math.Sin(t * 360) + l * Math.Sin(3 * t * 360), 
                                                   0));
            }
            return dael;
        }

        public TriMesh CreateStartLine(double a)
        {
            TriMesh sl = new TriMesh();
            for (int t = 0; t < VerticesNum; t++)
            {
                sl.Vertices.Add(new VertexTraits(a * ((Math.Cos(t * 360)) * (Math.Cos(t * 360)) * (Math.Cos(t * 360))), a * ((Math.Sin(t * 360)) * (Math.Sin(t * 360)) * (Math.Sin(t * 360))), 0));
            }
            return sl;
        }

        public TriMesh CreateHeartLine(double a)
        {
            TriMesh heart = new TriMesh();
            for (int t = 0; t < VerticesNum; t++)
            {
                heart.Vertices.Add(new VertexTraits(a * (1 + Math.Cos(360 * t)) * Math.Cos(360 * t), 
                                                    a * (1 + Math.Cos(360 * t)) * Math.Sin(360 * t), 
                                                    0));
            }
            return heart;
        }

        public TriMesh CreateCircularHelixLine()
        {
            TriMesh chl = new TriMesh();
            for (int t = 0; t < VerticesNum; t++)
            {
                chl.Vertices.Add(new VertexTraits((10 + 10 * Math.Sin(6 * t * 360)) * Math.Cos(t * 360), 
                                                 (10 + 10 * Math.Sin(6 * t * 360)) * Math.Sin(t * 360), 
                                                 2 * Math.Sin(6 * t * 360)));
            }
            return chl;
        }

        public TriMesh CreateSineCurve()
        {
            TriMesh sc = new TriMesh();
            for (int t = 0; t < VerticesNum; t++)
            {
                sc.Vertices.Add(new VertexTraits(t / VerticesNum, Math.Sin(t / VerticesNum), 0));
            }
            return sc;
        }

        public TriMesh CreateFermatCurve(double a)
        {
            TriMesh fc = new TriMesh();
            double x = 0;
            double y = 0;
            for (int t = 0; t < VerticesNum; t++)
            {
                x = (a * Math.Sqrt(360 * t * 5 * 180 / Math.PI)) 
                     * Math.Cos(360 * t * 5);
                y = (a * Math.Sqrt(360 * t * 5 * 180 / Math.PI)) 
                    * Math.Sin(360 * t * 5);
                fc.Vertices.Add(new VertexTraits(x, y, 0));
            } 
            return fc;
        }

        public TriMesh CreateTalbotCurve(double a, double b, double f)
        {
            TriMesh tc = new TriMesh();
            for (int t = 0; t < VerticesNum; t++)
            {
                tc.Vertices.Add(new VertexTraits((a * a + f * f * Math.Sin(t * 360) * Math.Sin(t * 360)) * Math.Cos(t * 360) / a, (a * a - 2 * f + f * f * Math.Sin(t * 360) * Math.Sin(t * 360)) * Math.Sin(360 * t) / b, 0));
            }

            return tc;
        }




 

        public TriMesh CreateMeshPeach()
        {
            //rho=t^3+t*(t+1)
            //theta=t*360
            //phi=t^2*360*10*10
            // int VerticeNum = 4000;
            double rho = 0;
            double theta, phi, x, y, z, t;
            TriMesh peach = new TriMesh();
            for (int i = 0; i <= VerticesNum; i++)
            {
                t = (double)i / (double)VerticesNum;
                rho = Math.Pow(t, 3) + t * (1 + t);
                //  rho = 0.3;
                theta = t * 2 * pi;
                phi = Math.Pow(t, 2) * 2 * pi * 10 * 10;
                x = rho * Math.Sin(theta) * Math.Cos(phi);
                y = rho * Math.Sin(theta) * Math.Sin(phi);
                z = rho * Math.Cos(theta);
                peach.Vertices.Add(new VertexTraits(x, y, z));
            }
            return peach;
        }
        public TriMesh CreateMeshSpring(double radius)
        {
            // int VerticeNum = 4000;
            TriMesh spring = new TriMesh();
            for (int i = 0; i <= VerticesNum; i++)
            {
                double theta = i * 3600 / VerticesNum;
                double z = Math.Sin(3.5 * theta - 90) + 24 / 10;
                spring.Vertices.Add(new VertexTraits(radius * Math.Cos(theta), radius * Math.Sin(theta), z));
            }

            return spring;
        }
        public TriMesh CreateMeshHxecqx()
        {
            // int VerticeNum = 4000;
            TriMesh hxecqx = new TriMesh();
            for (int i = 0; i <= VerticesNum; i++)
            {
                double theta = i * 3600 / VerticesNum;
                double x = 0.5 * Math.Cos(theta);
                double y = 0.5 * Math.Sin(theta);
                double z = 0.1 * (Math.Sin(8 * theta - 90) + 24 / 10);
                hxecqx.Vertices.Add(new VertexTraits(x, y, z));
            }

            return hxecqx;
        }

        public TriMesh CreateButterflyLine()
        { 
            TriMesh circle = new TriMesh();
            for (int i = 0; i <= VerticesNum; i++)
            {
                double x = ((4 * Math.Sin(i * 2 * pi / VerticesNum) + 
                            6* Math.Cos(i * 2 * pi * 360 / VerticesNum)) 
                            * Math.Sin(i * 2 * pi / VerticesNum) 
                            * Math.Cos(Math.Log(1 + i * 2 * pi / VerticesNum) 
                            * i * 2 * pi / VerticesNum));
                double y = ((4 * Math.Sin(i * 2 * pi / VerticesNum) + 
                           6 * Math.Cos(i * 2 * pi * 360 / VerticesNum)) 
                           * Math.Sin(i * 2 * pi / VerticesNum) * 
                           Math.Sin(Math.Log(1 + i * 2 * pi / VerticesNum) 
                           * i * 2 * pi / VerticesNum));
                double z = ((4 * Math.Sin(i * 2 * pi / VerticesNum) +
                           6 * Math.Cos(i * 2 * pi * 360 / VerticesNum)) 
                           * Math.Cos(i * 2 * pi / VerticesNum));
                circle.Vertices.Add(new VertexTraits(x, y, z));
            }
            return circle;
        }

        public TriMesh CreateMeshSpan()
        {
            TriMesh circle = new TriMesh();
            for (int i = 0; i <= 10000; i++)
            {
                double x = (50 + 10 * Math.Sin(i * 360 * 15)) * Math.Cos(i * 360);
                double y = (50 + 10 * Math.Sin(i * 360 * 15)) * Math.Sin(i * 360);
                double z = 10 * Math.Sin(5 * i * 360);
                circle.Vertices.Add(new VertexTraits(x, y, z));
            }
            return circle;
        }
        public TriMesh CreateMeshSin()
        {
            TriMesh circle = new TriMesh();
            for (int i = 0; i <= VerticesNum; i++)
            {
                double ang1 = i * 2 * pi;
                double ang2 = i * 2 * pi * 20;
                double x = ang1 * 2 * pi / 360;
                double y = Math.Sin(ang1) * 5 + Math.Cos(ang2);
                double z = Math.Sin(ang2);
                circle.Vertices.Add(new VertexTraits(x, y, z));
            }
            return circle;
        }
        public TriMesh CreateMeshImage()
        {
            TriMesh image = new TriMesh();
            for (int i = 0; i <= VerticesNum; i++)
            {
                double x = (2 * Math.Cos(2 * pi * 10 * i / VerticesNum) + Math.Cos(pi * 10 * i / VerticesNum));
                double y = (2 * Math.Sin(2 * pi * 10 * i / VerticesNum) + Math.Sin(pi * 10 * i / VerticesNum));
                double z = 0.0006 * i;
                image.Vertices.Add(new VertexTraits(x, y, z));
            }

            return image;
        }
        public TriMesh CreateMeshPhoto()
        {
            TriMesh photo = new TriMesh();
            for (int i = 0; i <= 10000; i++)
            {
                double x = 3 * Math.Cos(2 * pi * i * 8 / 10000) - 1.5 * Math.Cos(2 * pi / 3 * i * 8 / 10000);
                double y = 3 * Math.Sin(2 * pi * i * 8 / 10000) - 1.5 * Math.Sin(2 * pi / 3 * i * 8 / 10000);
                double z = 0.0008 * i;
                photo.Vertices.Add(new VertexTraits(x, y, z));
            }

            return photo;
        }


        public TriMesh CreateEpitrochoid()
        {
            TriMesh line = new TriMesh();
            float a = 0.5F;
            float b = 0.3F;
            float c = 0.5F;
            float theta; 
            for (int i = 0; i < 20000; i++)
            {
                theta = i * 360 * 10 / VerticesNum; 
                line.Vertices.Add(new VertexTraits(((a + b) * Math.Cos(theta)
                                       - c * Math.Cos((a / b + 1) * theta)) / 10, 
                                      ((a + b) * Math.Sin(theta) - 
                                      c * Math.Sin((a / b + 1) * theta)) / 10,
                                      0));
            }

            return line; 
        }

        public TriMesh CreateTricuspidValveLine()
        {
            TriMesh tvl = new TriMesh();
            int a = 10;

            for (int i = 0; i < 20000; i++)
            {
                tvl.Vertices.Add(new VertexTraits((a * (2 * Math.Cos(i * 360) + Math.Cos(2 * i * 360))) / 100, (a * (2 * Math.Sin(i * 360) - Math.Sin(2 * i * 360))) / 100, 0));
            }

            return tvl;

        }

        public TriMesh CreateProbabilityCurve()
        {
            TriMesh pc = new TriMesh();


            for (int i = 0; i < 2000; i++)
            {
                pc.Vertices.Add(new VertexTraits((-1 + i * 0.001 * 10 - 5) / 10, (Math.Exp(0 - (-1 + i * 0.001 * 10 - 5) * (-1 + i * 0.001 * 10 - 5))) / 10, 0));
            }

            return pc;

        }

        public TriMesh CreateVersiera()
        {
            TriMesh v = new TriMesh();

            float a = 1;
            for (int i = 0; i < 2000; i++)
            {
                v.Vertices.Add(new VertexTraits((-1 + i * 0.001 * 10 - 5) / 10, (8 * a * a / ((-1 + i * 0.001 * 10 - 5) * (-1 + i * 0.001 * 10 - 5) + 4 * a * a)) / 10, 0));
            }

            return v;

        }

        public TriMesh CreateAchimedeanSpiral()
        {
            TriMesh A = new TriMesh();

            int a = 100;
            double r;
            double theta;
            for (int t = 0; t < 2000; t++)
            {
                theta = t * 400 / VerticesNum;
                r = a * theta;
                A.Vertices.Add(new VertexTraits((r * 0.000005 * Math.Cos(theta * 0.00942)), (r * 0.000005 * Math.Sin(theta * 0.00942)), 0));
            }

            return A;

        }

        public TriMesh CreateLog()
        {
            TriMesh log = new TriMesh();

            double a = 0.005;
            double theta;
            double r;

            for (int t = 0; t < 2000; t++)
            {
                theta = t * 360 * 2.2 * 0.00000942;
                r = a * Math.Exp(theta);
                log.Vertices.Add(new VertexTraits((r * Math.Cos(theta) * 0.0001), (r * Math.Sin(theta) * 0.0001), 0));
            }

            return log;

        }

        public TriMesh CreateCissoid()
        {
            TriMesh man = new TriMesh();

            int a = 10;

            for (int t = 0; t < 2000; t++)
            {
                double tt = t * 0.314;
                man.Vertices.Add(new VertexTraits((2 * a * Math.Sin(tt) * Math.Sin(tt)) * 0.008, (2 * a * Math.Sin(tt) * Math.Sin(tt) * Math.Sin(tt) * 0.008 / Math.Cos(tt)), 0));
            }

            return man;
        }

        public TriMesh CreateTan()
        {
            TriMesh tan = new TriMesh();

            for (int t = 0; t < 2000; t++)
            {
                tan.Vertices.Add(new VertexTraits((t * 0.00015 * 8.5 - 4.25) / 10, (Math.Tan(t * 0.00015 * 8.5 - 4.25) * 20) / 100, 0));
            }

            return tan;

        }




        public TriMesh CreateSpineLine()
        {

            TriMesh line = new TriMesh();
            double x = 0;
            double y = 0;
            double z = 0;
            for (int t = 0; t <= VerticesNum; t++)
            {
                x = 4 * Math.Cos(t * (5 * 360));
                y = 4 * Math.Sin(t * (5 * 360));
                z = 10 * t;
                line.Vertices.Add(new VertexTraits(x, y, z));
            }

            return line;
        }

        public TriMesh CreateFoliumCircle(double radius)
        {

            TriMesh circle = new TriMesh();

            double x = 0;
            double y = 0;
            double z = 0;

            for (int t = 0; t < VerticesNum; t++)
            {
                x = 3 * 10 * t / (1 + Math.Pow(t, 3));
                y = 3 * 10 * Math.Pow(t, 2) / (1 + Math.Pow(t, 3));
                circle.Vertices.Add(new VertexTraits(x, y, 0));
            }




            return circle;
        }



        public TriMesh CreateConeshapeLine(double l, double b)
        {
            TriMesh dael = new TriMesh();
            double x = 0;
            double y = 0;
            double z = 0;

            for (int t = 0; t < VerticesNum; t++)
            {
                x = t * Math.Cos(10 + t * (20 * 360));
                y = t * Math.Sin(10 + t * (20 * 360));
                z = t * 3;
                dael.Vertices.Add(new VertexTraits(x, y, z));
            }
            return dael;


        }

        public TriMesh CreateButterflyLine(double a)
        {
            TriMesh sl = new TriMesh();
            ;
            double x = 0;
            double y = 0;
            double z = 0;

            for (int i = 0; i <= VerticesNum; i++)
            {
                x = 8 * i * Math.Sin(2 * Math.PI * i / VerticesNum * 4) * Math.Cos(-2 * Math.PI * i / VerticesNum * 8);
                y = 8 * i * Math.Sin(2 * Math.PI * i / VerticesNum * 4) * Math.Sin(-2 * Math.PI * i / VerticesNum * 8);
                z = 8 * i * Math.Cos(2 * Math.PI * i / VerticesNum * 4);
                sl.Vertices.Add(new VertexTraits(x, y, z));

            }

            return sl;

        }

        public TriMesh CreateInvoluteCurve(double a)
        {
            TriMesh line = new TriMesh();
            int r = 1;
            for (int t = 0; t <= VerticesNum; t++)
            {
                double ang = 2 * Math.PI * t / VerticesNum;
                double s = 2 * Math.PI * r * t / VerticesNum;
                double x0 = s * Math.Cos(ang);
                double y0 = s * Math.Sin(ang);
                line.Vertices.Add(new VertexTraits(x0 + s * Math.Sin(ang), 
                                                 y0 - s * Math.Cos(ang),
                                                 0));
            } 
            return line;
        }

        public TriMesh CreateSpringLine()
        {
            TriMesh chl = new TriMesh();
            double x = 0;
            double y = 0;
            double z = 0;
            for (int t = -100; t <= 3100; t++)
            {
                x = 100 * Math.Cos(1440 * t);
                y = 100 * Math.Sin(1440 * t);
                z = Math.Sin(3.5 * 1440 * t - 90) + 2 * t;

                chl.Vertices.Add(new VertexTraits(x, y, z));
            }

            return chl;
        }

       

        public TriMesh CreateLogarithmCurve(double a)
        {
            TriMesh fc = new TriMesh();
            for (int i = 0; i <= VerticesNum; i++)
            {
                fc.Vertices.Add(new VertexTraits(i * 0.01 - 1, (i * 0.001 - 1) * Math.Log((i * 0.01) + 0.0001), 10));
            }



            return fc;
        }

        public TriMesh CreateGlobalCurve(double a, double b, double f)
        {
            TriMesh tc = new TriMesh();
            double x = 0;
            double y = 0;
            double z = 0;
            for (int t = -VerticesNum; t <= VerticesNum; t++)
            {
                x = 4 * Math.Sin(180 * t / VerticesNum) * Math.Cos(t * 360 * 20);
                y = 4 * Math.Sin(180 * t / VerticesNum) * Math.Sin(t * 360 * 20);
                z = 4 * Math.Cos(180 * t / VerticesNum);
                tc.Vertices.Add(new VertexTraits(x, y, z));
            }


            return tc;
        }




            //  Long_short
        public TriMesh CreateLong()
        {

            float a = 5f,  b = 7f,  c = 2.2f;
            TriMesh long_short = new TriMesh();
          
            for (int t = 0; t <  VerticesNum; t++)
            {
                int theta = 360 * t * 10;
                double x = (a - b) * Math.Cos(theta) + c * Math.Cos((a / b - 1) * theta);
                double y = (a - b) * Math.Sin(theta) - c * Math.Sin((a / b - 1) * theta);
                long_short.Vertices.Add(new VertexTraits(x, y, 0));
            }

            return long_short;
        }


        //  Lissajous 利萨如曲线

        public  TriMesh CreateLissajous()
        {

            float a = 1,  b = 1,   c = 100 ;
            int n=3;
            TriMesh lissajous = new TriMesh();

            for (int t = 0; t <  VerticesNum; t++)
            {
                int theta = t * 360;
                double x=a* Math.Sin(n*theta+c);
                double y = b * Math.Sin(theta);
                lissajous.Vertices.Add(new VertexTraits(x, y, 0));
            }

            return lissajous;
        }

        //epicycloid  外摆线

        public  TriMesh CreateEpicycloid( )
        {

            float a = 5, b = 8;
            TriMesh epicycloid = new TriMesh();

            for (int t = 0; t <  VerticesNum; t++)
            {
                int theta = t * 720 * 5;
                double x = (a + b) * Math.Cos(theta) - b * Math.Cos((a / b + 1) * theta);
                double y = y = (a + b) * Math.Sin(theta) - b * Math.Sin((a / b + 1) * theta);
                epicycloid.Vertices.Add(new VertexTraits(x, y, 0));
            }

            return epicycloid;
        }



        //threee leaves 三叶线
        public  TriMesh CreateThree()
        {
            float a = 1;
            TriMesh threee_leaves = new TriMesh();

            for (int t = 0; t <  VerticesNum; t++)
            {
                float theta =2* t  * 380;
                double b = Math.Sin(theta);
                double x = a * Math.Cos(theta) * (2* b * b - 1) * Math.Cos(theta);
                double y = a * Math.Cos(theta) * (2* b * b - 1) * Math.Sin(theta);
                threee_leaves.Vertices.Add(new VertexTraits(x, y, 0));
            }

            return threee_leaves;
        }


        // parabolic 抛物线

        public  TriMesh CreateParabolic()
        {
            TriMesh parabolic = new TriMesh();

            for (int t = 0; t <  VerticesNum; t++)
            {
                double tt = t * 0.1;
                double x = (0.4 * tt);
                double y = (0.3 * tt * 0.1) + (0.5 * tt * tt * 0.01);
                parabolic.Vertices.Add(new VertexTraits(x, y, 0));
            }

            return parabolic;
        }



        //  .Rhodonea 曲线
        public  TriMesh CreateRhodonea()
        {
            TriMesh rhodonea = new TriMesh();
            double a = 10;
            double b = 6;

            for (int t = 0; t <  VerticesNum; t++)
            {
                int theta  = t * 360 * 4;
                double x =  (a - b) * Math.Cos(theta) + a * Math.Cos((a / b - 1) * theta);
                double y = (a - b) * Math.Sin(theta) - b * Math.Sin((a / b - 1) * theta);
                rhodonea.Vertices.Add(new VertexTraits(x, y, 0));
            }

            return rhodonea;
        }






        // helix  螺旋线  曲线
        public  TriMesh CreateHelix()
        {
            double r = 5f;
            int n = 5;
            float height = 10;

            TriMesh helix = new TriMesh();
            

            for (int t = 0; t <  VerticesNum; t++)
            {
              //  double tt = t /  VerticesNum;
                double theta = t * 1800;
                double x = 20 * r * Math.Cos(theta);
                double y =  20* r * Math.Sin(theta);
                double z = (Math.Cos(theta - 90) + 24 * t)*0.1;
                helix.Vertices.Add(new VertexTraits(x, y, z));
            }

            return helix;
        }


        // Four_leaves
        public TriMesh Four_leaves()
        {

            float a = 1;
            TriMesh four_leaves = new TriMesh();


            for (int t = 0; t <  VerticesNum; t++)
            {
                float theta =  t*380 ;
                double b = Math.Sin(2*theta);
                double x = a * Math.Cos(2 * theta) * Math.Cos(theta);
                double y = a * Math.Cos(2 * theta)  * Math.Sin(theta);
                four_leaves.Vertices.Add(new VertexTraits(x, y, 0));
            }


            return four_leaves;
        }


        public TriMesh CreateMeshSun()
        {
            TriMesh Sun = new TriMesh();
            for (int t = 0; t < VerticesNum; t++)
            {
                Sun.Vertices.Add(new VertexTraits((1.5 * Math.Cos(50 * t * 360) + 1) * Math.Sin(t * 360), (1.5 * Math.Cos(50 * t * 360) + 1) * Math.Cos(t * 360), 0));
            }
            return Sun;
        }

        public TriMesh CreateTALine()
        {
            TriMesh Ta = new TriMesh();
            for (int t = 0; t < VerticesNum; t++)
            {
                Ta.Vertices.Add(new VertexTraits((t * 80 + 5) * Math.Sin(t * 360 * 10), (t * 80 + 5) * Math.Cos(t * 360 * 10), t * 80));
            }
            return Ta;
        }

        public TriMesh CreateButterFly()
        {
            TriMesh bt = new TriMesh();

            for (int t = 0; t < VerticesNum; t++)
            {
                bt.Vertices.Add(new VertexTraits(2 * t * Math.Sin(t * 3600), 2.5 * t * Math.Cos(t * 3600), 3 * t * Math.Sin(t * 1800)));
            }
            return bt;
        }

        public TriMesh CreateDoubleY()
        {
            TriMesh Dby = new TriMesh();
            for (int t = 0; t < VerticesNum; t++)
            {
                Dby.Vertices.Add(new VertexTraits((Math.Sin(t * 360 * 10) + 30) * Math.Sin(Math.Sin(t * 360 * 15)), (Math.Sin(t * 360 * 10) + 30) * Math.Cos(Math.Sin(t * 360 * 15)), Math.Sin(t * 3)));
            }
            return Dby;
        }

       

        public TriMesh CreateFlower()
        {
            TriMesh Fl = new TriMesh();
            for (int t = 0; t < VerticesNum; t++)
            {
                Fl.Vertices.Add(new VertexTraits((t * 20) * Math.Cos(t * 360 * 90) * Math.Sin(t * 360 * 10), (t * 20) * Math.Sin(t * 360 * 90) * Math.Sin(t * 360 * 10), t * 20 * Math.Cos(t * 360 * 10)));
            }
            return Fl;
        }

        public TriMesh CreateDoubleFish()
        {
            TriMesh df = new TriMesh();
            for (int t = 0; t < VerticesNum; t++)
            {
                df.Vertices.Add(new VertexTraits((30 + 10 * Math.Sin(t * 360 * 10)) * Math.Cos(t * 180 * Math.Cos(t * 360 * 10)) * Math.Sin(t * 360 * 30), (30 + 10 * Math.Sin(t * 360 * 10)) * Math.Sin(t * 180 * Math.Cos(t * 360 * 10)) * Math.Sin(t * 360 * 30), (30 + 10 * Math.Sin(t * 360 * 10)) * Math.Cos(t * 360 * 30)));
            }
            return df;
        }

        public TriMesh CreateAjLine()
        {
            TriMesh aj = new TriMesh();
            for (int t = 0; t < VerticesNum; t++)
            {
                aj.Vertices.Add(new VertexTraits((10 * 360 * 2 * (t - 0.5)) * Math.Sin(360 * 2 * (t - 0.5)), (10 * 360 * 2 * (t - 0.5)) * Math.Cos(360 * 2 * (t - 0.5)), 0));
            }
            return aj;
        }

        public TriMesh CreateJk()
        {
            TriMesh Jk = new TriMesh();
            double r, pi;
            double ang;
            pi = 3.14;
            r = 20;
            for (int t = 0; t < VerticesNum; t++)
            {
                ang = 360 * t;
                Jk.Vertices.Add(new VertexTraits(r * Math.Cos(ang) + 2 * pi * r * t * Math.Sin(ang), r * Math.Sin(ang) - 2 * pi * r * t * Math.Cos(ang), 0));
            }
            return Jk;
        }

         
    }
}
