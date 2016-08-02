using System;
using System.Threading;

namespace GraphicResearchHuiZhao 
{
    public class ThreadOperator
    {
        
        public ThreadOperator()
        {
        }

        public event EventHandler Finished;
       
        
        Thread operatorThread;
        
        /// <summary>
        /// Gets whether the mesh loading thread is currently running.
        /// </summary>
        public bool IsAlive
        {
            get
            {
                if (operatorThread!= null)
                {
                    return operatorThread.IsAlive;
                }
                else
                {
                    return false;
                }
            }
        }

      

        
        /// <summary>
        /// Aborts the mesh loading thread if it is running.
        /// </summary>
        public void Abort()
        {
            if (IsAlive)
            {
                operatorThread.Abort();
            }
        }

        /// <summary>
        /// Loads the specified mesh file in a background thread.
        /// </summary>
        /// <param name="fileName">The name of the file to load.</param>
        public void Run(ThreadStart function)
        {
           
            operatorThread = new Thread(function);
            operatorThread.Name = "thread";
            operatorThread.IsBackground = true;
            operatorThread.Priority = ThreadPriority.BelowNormal;
          

           
            Finished(this, EventArgs.Empty);
        }

        
        
    }
}
