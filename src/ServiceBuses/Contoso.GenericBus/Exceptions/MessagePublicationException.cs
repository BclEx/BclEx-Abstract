using System;
using System.Runtime.Serialization;

namespace Contoso.GenericBus.Exceptions
{
    /// <summary>
    /// MessagePublicationException
    /// </summary>
    [Serializable]
    public class MessagePublicationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessagePublicationException"/> class.
        /// </summary>
        public MessagePublicationException() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="MessagePublicationException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MessagePublicationException(string message) : base(message) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="MessagePublicationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public MessagePublicationException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="MessagePublicationException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected MessagePublicationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}