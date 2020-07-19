using System.IO;

namespace TestsHubUploadEndpoint.Coverage
{
    public interface ICoverageReader
    {
        void Read(Stream stream, string testRunName);
    }
}