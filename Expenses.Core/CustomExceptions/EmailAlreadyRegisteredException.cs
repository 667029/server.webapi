using System;
using System.Runtime.Serialization;

namespace Expenses.Core.CustomExceptions
{
    public class EmailAlreadyRegisteredException : Exception
    {
        public EmailAlreadyRegisteredException()
        {
        }

        public EmailAlreadyRegisteredException(string message) : base(message)
        {
        }

        public EmailAlreadyRegisteredException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EmailAlreadyRegisteredException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
