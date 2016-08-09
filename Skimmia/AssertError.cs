using System;
using System.Runtime.Serialization;

namespace Skimmia
{
    class AssertError : Exception
    {
        public AssertError()
        {
        }

        public AssertError(string message) : base(message)
        {
        }

        public AssertError(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AssertError(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}