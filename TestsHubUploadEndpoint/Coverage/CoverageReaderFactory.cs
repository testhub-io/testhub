using System;
using System.Text;

namespace TestsHubUploadEndpoint.Coverage
{
    public class CoverageReaderFactory
    {
        public ICoverageReader CreateReader(System.IO.Stream reportContent, IDataLoader dataLoader)
        {
            var buffer = new byte[1024];

            reportContent.Read(buffer, 0, buffer.Length);
            reportContent.Seek(0, System.IO.SeekOrigin.Begin);
            var content = Encoding.UTF8.GetString(buffer);

            if (content.Contains("<report", StringComparison.InvariantCultureIgnoreCase))
            {
                return new JacocoReader(dataLoader);
            }

            return new CoberturaReader(dataLoader);
        }
    }
}
