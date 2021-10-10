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
            else if (content.Contains("<coverage", StringComparison.InvariantCultureIgnoreCase))
            {
                if (content.Contains("<packages>", StringComparison.InvariantCultureIgnoreCase))
                {
                    return new CoberturaReader(dataLoader);
                }else if(content.Contains("<project", StringComparison.InvariantCultureIgnoreCase))
                {
                    return new CloverReader(dataLoader);
                }                
            }            
            throw new InvalidOperationException("Unknown coverage format");            
        }
    }
}
