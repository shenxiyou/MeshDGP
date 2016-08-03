

using System;
using System.Runtime.Serialization;

namespace GraphicResearchHuiZhao
{
    

    #region MismatchedMeshException
    /// <summary>
    /// The exception that is thrown when an attempt is made to use a mesh element with an element or method for a different mesh.
    /// </summary>
    [Serializable]
    public class MismatchedMeshException : MeshException
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public MismatchedMeshException() : base() { }

        /// <summary>
        /// Initializes a new instance of this class with a specified error message.
        /// </summary>
        /// <param name="message">A string that describes the error.</param>
        public MismatchedMeshException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of this class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">A string that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception.</param>
        public MismatchedMeshException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of this class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected MismatchedMeshException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    #endregion
}
