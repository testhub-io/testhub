using System.Xml.Linq;
using System.Xml.XPath;

namespace TestsHubUploadEndpoint.Coverage
{
    public class CoberturaReader : ICoverageReader
    {
        private readonly IDataLoader _dataLoader;

        public CoberturaReader(IDataLoader dataLoader)
        {
            _dataLoader = dataLoader;
        }

        public void Read(System.IO.Stream stream, string testRunName)
        {
            var report = XDocument.Load(stream);

            // .net coverage xml
            var linesCovered = 0;
            var linesValid = 0;

            foreach (var element in report.XPathSelectElements("/coverage"))
            {
                var covered = int.Parse(element.Attribute("lines-covered").Value);
                linesCovered += covered;

                linesValid += int.Parse(element.Attribute("lines-valid").Value);
            }

            _dataLoader.Add(new CoverageSummary(linesCovered, linesValid, testRunName));

        }
    }
}
