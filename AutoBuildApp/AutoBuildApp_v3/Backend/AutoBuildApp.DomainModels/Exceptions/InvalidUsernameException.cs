using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AutoBuildApp.DomainModels.Exceptions
{
    [Serializable]
    public class InvalidUsernameException : Exception
    {
        public InvalidUsernameException()
        {
        }

        public InvalidUsernameException(string message) : base(message)
        {
        }

        public InvalidUsernameException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidUsernameException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
