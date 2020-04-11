using System.Xml.Linq;
using System.Xml.XPath;

namespace TestsHubUploadEndpoint
{
    public class CoberturaReader
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

            foreach (var element in report.XPathSelectElements("/results/modules/module"))
            {
                var covered = int.Parse(element.Attribute("lines_covered").Value);
                linesCovered += covered;

                linesValid += int.Parse(element.Attribute("lines_not_covered").Value) + linesCovered;
            }

            _dataLoader.Add(new CoverageModel.CoverageSummary(linesCovered, linesValid, testRunName));

        }
    }
}
