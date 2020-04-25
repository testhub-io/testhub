using System;
using System.Runtime.Serialization;

namespace TestHub.Api.ApiDataProvider
{
    [Serializable]
    internal class TesthubApiException : Exception
    {
        public TesthubApiException()
        {
        }

        public TesthubApiException(string message) : base(message)
        {
        }

        public TesthubApiException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TesthubApiException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}