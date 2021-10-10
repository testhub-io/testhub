using System;
using System.Runtime.Serialization;

namespace TestsHubUploadEndpoint
{
    [Serializable]
    internal class DataLoadException : Exception
    {
        public DataLoadException()
        {
        }

        public DataLoadException(string message) : base(message)
        {
        }

        public DataLoadException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DataLoadException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}