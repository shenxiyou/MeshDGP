 
using System;
using System.Runtime.Serialization;

namespace GraphicResearchHuiZhao
{
    #region ContextException
    /// <summary>
    /// The exception that is thrown when there is an error in the OpenGL context.
    /// </summary>
    [Serializable]
    public class ContextException : Exception
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public ContextException() : base() { }

        /// <summary>
        /// Initializes a new instance of this class with a specified error message.
        /// </summary>
        /// <param name="message">A string that describes the error.</param>
        public ContextException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of this class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">A string that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception.</param>
        public ContextException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of this class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected ContextException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    #endregion

    #region OpenGLException
    /// <summary>
    /// The base class for exceptions caused by OpenGL errors.
    /// </summary>
    [Serializable]
    public class OpenGLException : Exception
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public OpenGLException() : base() { }

        /// <summary>
        /// Initializes a new instance of this class with a specified error message.
        /// </summary>
        /// <param name="message">A string that describes the error.</param>
        public OpenGLException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of this class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">A string that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception.</param>
        public OpenGLException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of this class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected OpenGLException(SerializationInfo info, StreamingContext context) : base(info, context) {}
    }
    #endregion

    #region OpenGLInvalidEnumException
    /// <summary>
    /// The exception that is thrown when there is a GL_INVALID_ENUM error.
    /// </summary>
    [Serializable]
    public class OpenGLInvalidEnumException : OpenGLException
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public OpenGLInvalidEnumException() : base() { }

        /// <summary>
        /// Initializes a new instance of this class with a specified error message.
        /// </summary>
        /// <param name="message">A string that describes the error.</param>
        public OpenGLInvalidEnumException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of this class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">A string that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception.</param>
        public OpenGLInvalidEnumException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of this class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected OpenGLInvalidEnumException(SerializationInfo info, StreamingContext context) : base(info, context) {}
    }
    #endregion

    #region OpenGLInvalidValueException
    /// <summary>
    /// The exception that is thrown when there is a GL_INVALID_VALUE error.
    /// </summary>
    [Serializable]
    public class OpenGLInvalidValueException : OpenGLException
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public OpenGLInvalidValueException() : base() { }

        /// <summary>
        /// Initializes a new instance of this class with a specified error message.
        /// </summary>
        /// <param name="message">A string that describes the error.</param>
        public OpenGLInvalidValueException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of this class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">A string that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception.</param>
        public OpenGLInvalidValueException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of this class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected OpenGLInvalidValueException(SerializationInfo info, StreamingContext context) : base(info, context) {}
    }
    #endregion

    #region OpenGLInvalidOperationException
    /// <summary>
    /// The exception that is thrown when there is a GL_INVALID_OPERATION error.
    /// </summary>
    [Serializable]
    public class OpenGLInvalidOperationException : OpenGLException
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public OpenGLInvalidOperationException() : base() { }

        /// <summary>
        /// Initializes a new instance of this class with a specified error message.
        /// </summary>
        /// <param name="message">A string that describes the error.</param>
        public OpenGLInvalidOperationException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of this class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">A string that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception.</param>
        public OpenGLInvalidOperationException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of this class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected OpenGLInvalidOperationException(SerializationInfo info, StreamingContext context) : base(info, context) {}
    }
    #endregion

    #region OpenGLStackOverflowException
    /// <summary>
    /// The exception that is thrown when there is a GL_STACK_OVERFLOW error.
    /// </summary>
    [Serializable]
    public class OpenGLStackOverflowException : OpenGLException
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public OpenGLStackOverflowException() : base() { }

        /// <summary>
        /// Initializes a new instance of this class with a specified error message.
        /// </summary>
        /// <param name="message">A string that describes the error.</param>
        public OpenGLStackOverflowException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of this class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">A string that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception.</param>
        public OpenGLStackOverflowException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of this class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected OpenGLStackOverflowException(SerializationInfo info, StreamingContext context) : base(info, context) {}
    }
    #endregion

    #region OpenGLStackUnderflowException
    /// <summary>
    /// The exception that is thrown when there is a GL_STACK_UNDERFLOW error.
    /// </summary>
    [Serializable]
    public class OpenGLStackUnderflowException : OpenGLException
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public OpenGLStackUnderflowException() : base() { }

        /// <summary>
        /// Initializes a new instance of this class with a specified error message.
        /// </summary>
        /// <param name="message">A string that describes the error.</param>
        public OpenGLStackUnderflowException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of this class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">A string that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception.</param>
        public OpenGLStackUnderflowException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of this class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected OpenGLStackUnderflowException(SerializationInfo info, StreamingContext context) : base(info, context) {}
    }
    #endregion

    #region OpenGLOutOfMemoryException
    /// <summary>
    /// The exception that is thrown when there is a GL_OUT_OF_MEMORY error.
    /// </summary>
    [Serializable]
    public class OpenGLOutOfMemoryException : OpenGLException
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public OpenGLOutOfMemoryException() : base() { }

        /// <summary>
        /// Initializes a new instance of this class with a specified error message.
        /// </summary>
        /// <param name="message">A string that describes the error.</param>
        public OpenGLOutOfMemoryException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of this class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">A string that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception.</param>
        public OpenGLOutOfMemoryException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of this class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected OpenGLOutOfMemoryException(SerializationInfo info, StreamingContext context) : base(info, context) {}
    }
    #endregion
}
