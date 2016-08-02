

using System;
using System.Threading;

namespace GraphicResearchHuiZhao
{
    /// <summary>
    /// A class to fascilitate loading a mesh in a background thread.
    /// </summary>
    public class MeshLoader
    {
        #region Events
        /// <summary>
        /// The event that is triggered when a mesh has finished loading.
        /// </summary>
        public event EventHandler MeshProcessFinished;
        #endregion

        #region Fields
        string file;
        TriMesh mesh;
        Thread loaderThread;
        #endregion

        #region Properties
        /// <summary>
        /// Gets whether the mesh loading thread is currently running.
        /// </summary>
        public bool IsAlive
        {
            get
            {
                if (loaderThread != null)
                {
                    return loaderThread.IsAlive;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// The mesh that has been loaded.
        /// </summary>
        public TriMesh Mesh
        {
            get
            {
                return mesh;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Aborts the mesh loading thread if it is running.
        /// </summary>
        public void Abort()
        {
            if (IsAlive)
            {
                loaderThread.Abort();
            }
        }

        /// <summary>
        /// Loads the specified mesh file in a background thread.
        /// </summary>
        /// <param name="fileName">The name of the file to load.</param>
        public void Load(string fileName)
        {
            file = fileName;
            loaderThread = new Thread(new ThreadStart(Run));
            loaderThread.Name = "Mesh Process thread";
            loaderThread.IsBackground = true;
            loaderThread.Priority = ThreadPriority.BelowNormal;
            loaderThread.Start();
        }

        private void Run()
        {
            try
            {
                mesh = TriMeshIO.ReadFile(file);
                lock (mesh)
                {
                    //mesh.ComputeAllTraits();
                }
                MeshProcessFinished(this, EventArgs.Empty);
            }
            catch (ThreadAbortException)
            {
                Thread.ResetAbort();
                mesh = null;
            }
        }
        #endregion
    }
}
