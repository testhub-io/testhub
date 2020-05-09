using System;
using System.Runtime.Serialization;

namespace TestsHubUploadEndpoint
{
    [Serializable]
    internal class ReportReaderException : Exception
    {
        public ReportReaderException()
        {
        }

        public ReportReaderException(string message) : base(message)
        {
        }

        public ReportReaderException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ReportReaderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}