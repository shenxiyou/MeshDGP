//using System;
//using System.Collections.Generic; 
//using System.Windows.Forms;

//namespace FaceExpression
//{
//    static class Program
//    {
//        /// <summary>
//        /// The main entry point for the application.
//        /// </summary>
//        [STAThread]
//        static void Main()
//        {
//            Application.EnableVisualStyles();
//            Application.SetCompatibleTextRenderingDefault(false);            
//            Application.DoEvents();
//            FaceForm.CurrForm = new FaceForm();
//            Application.Run(FaceForm.CurrForm);
//        }
//    }
//}
 



 
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Windows.Forms;
 

namespace GraphicResearchHuiZhao
{
    internal sealed class Startup
    {
        private static Startup instance;
        private static DateTime startupTime;
        private string[] args;
        private FormMain mainForm;

        private Startup(string[] args)
        {
            this.args = args;
        }

          
   
        public void Start()
        {
 
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
 
            // Initialize some misc. Windows Forms settings
            Application.SetCompatibleTextRenderingDefault(false);
            Application.EnableVisualStyles();

            this.mainForm =new FormMain();
             

                // 3 2 1 go
                Application.Run(this.mainForm);

                try
                {
                    this.mainForm.Dispose();
                }

                catch (Exception)
                {
                }

                this.mainForm = null;
        }

       

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static int Main(string[] args) 
        {
            startupTime = DateTime.Now;

 
            try
            { 

                instance = new Startup(args);
                instance.Start();
 
            }

            catch (Exception ex)
            {
                try
                {
                    UnhandledException(ex);
                    Process.GetCurrentProcess().Kill();
                }

                catch (Exception)
                {
                    MessageBox.Show(ex.ToString());
                    Process.GetCurrentProcess().Kill();
                }
            }
 

            return 0;
        }

       

        private static void UnhandledException(Exception ex)
        {
            string dir = Application.StartupPath;
            const string fileName = "crash.log";
            string fullName = Path.Combine(dir, fileName);

            using (StreamWriter stream = new System.IO.StreamWriter(fullName, true))
            {
                stream.AutoFlush = true;
                WriteCrashLog(ex, stream);
            }

   

            
        }

        public static string GetCrashLogHeader()
        {
            StringBuilder headerSB = new StringBuilder();
            StringWriter headerSW = new StringWriter(headerSB);
            WriteCrashLog(null, headerSW);
            return headerSB.ToString();
        }

        private static void WriteCrashLog(Exception ex, TextWriter stream)
        {

        }

        

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            UnhandledException((Exception)e.ExceptionObject);
            Process.GetCurrentProcess().Kill();
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            UnhandledException(e.Exception);
            Process.GetCurrentProcess().Kill();
        }
    }
}
