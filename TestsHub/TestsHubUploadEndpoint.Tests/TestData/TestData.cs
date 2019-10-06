using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TestsHubUploadEndpoint.Tests
{
    internal static class TestData
    {
        public static Stream GetFile (string name)
        {
            var stream = File.OpenRead($"..\\..\\..\\TestData\\{name}");
            return stream;
        }
    }
}
