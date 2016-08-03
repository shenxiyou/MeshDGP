

using System;
using System.Runtime.Serialization;

namespace GraphicResearchHuiZhao
{
    

    #region BadTopologyException
    /// <summary>
    /// The exception that is thrown when non-manifold topology is detected.
    /// </summary>
    [Serializable]
    public class BadTopologyException : MeshException
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public BadTopologyException() : base() { }

        /// <summary>
        /// Initializes a new instance of this class with a specified error message.
        /// </summary>
        /// <param name="message">A string that describes the error.</param>
        public BadTopologyException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of this class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">A string that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception.</param>
        public BadTopologyException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of this class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected BadTopologyException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    #endregion

     
}
