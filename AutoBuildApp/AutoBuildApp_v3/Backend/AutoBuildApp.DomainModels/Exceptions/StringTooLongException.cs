using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AutoBuildApp.DomainModels.Exceptions
{
    [Serializable]
    public class StringTooLongException : Exception
    {
        public StringTooLongException()
        {
        }

        public StringTooLongException(string message) : base(message)
        {
        }

        public StringTooLongException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected StringTooLongException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
