using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace RzWpf.Validation
{
    /// <summary>
    /// This exception contains a localized message for displaying the result of a validation error.
    /// </summary>
    [Serializable]
    public sealed class ValidationException : Exception
    {
        public ValidationException()
            : base()
        { }

        public ValidationException( string message )
            : base( message )
        { }

        /// <summary>
        /// Creates a ValidationException whose message is the message format and parameters specified using the system's current culture.
        /// </summary>
        /// <param name="messageFormat">The message string format.</param>
        /// <param name="messageParameters">The message string parameters.</param>
        public ValidationException( string messageFormat, params string[] messageParameters )
            : this( CultureInfo.CurrentCulture, messageFormat, messageParameters )
        { }

        public ValidationException( CultureInfo cultureInfo, string messageFormat, params string[] messageParameters )
            : base( String.Format( cultureInfo, messageFormat, messageParameters ) )
        { }

        public ValidationException( string message, Exception innerException )
            : base( message, innerException )
        { }

        /// <summary>
        /// Creates a ValidationException whose message is the message format and parameters specified using the system's current culture.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="messageFormat">The message string format.</param>
        /// <param name="messageParameters">The message string parameters.</param>
        public ValidationException( Exception innerException, string messageFormat, params string[] messageParameters )
            : this( String.Format( CultureInfo.CurrentCulture, messageFormat, messageParameters ), innerException )
        { }

        private ValidationException( SerializationInfo info, StreamingContext context )
            : base( info, context )
        { }
    }
}
