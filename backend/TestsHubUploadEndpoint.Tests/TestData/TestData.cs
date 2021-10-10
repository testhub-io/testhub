using System;
using System.IO;

namespace TestsHubUploadEndpoint.Tests
{
    internal static class TestData
    {
        public static Stream GetTestReport(string type, string name)
        {
            return GetFile(Path.Join("test-reports", type, name));
        }

        public static Stream GetCoverageReport(string type, string name)
        {
            return GetFile(Path.Join("coverage", type, name));
        }

        public static Stream GetFile(string name)
        {
            if (!File.Exists($"../../../TestData/{name}"))
            {
                throw new Exception("File does not exist");
            }
            var stream = File.OpenRead($"../../../TestData/{name}");
            return stream;
        }
    }
}
