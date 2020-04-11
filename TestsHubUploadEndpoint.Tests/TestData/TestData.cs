using System;
using System.IO;

namespace TestsHubUploadEndpoint.Tests
{
    internal static class TestData
    {
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
