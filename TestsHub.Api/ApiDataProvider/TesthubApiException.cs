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

        public static void ThrowProjectDoesNotExist(string project)
        {
            throw new TesthubApiException($"Project {project} does not exist");
        }

        public static void ThrowTestRunDoesNotExist(string testRun)
        {
            throw new TesthubApiException($"Test-run {testRun} does not exist");
        }
    }
}