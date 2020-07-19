using System;
using System.Xml.Linq;
using System.Xml.XPath;

namespace TestsHubUploadEndpoint.Coverage
{
    public class JacocoReader : ICoverageReader
    {
        private readonly IDataLoader _dataLoader;

        public JacocoReader(IDataLoader dataLoader)
        {
            _dataLoader = dataLoader;
        }

        public void Read(System.IO.Stream stream, string testRunName)
        {
            var report = XDocument.Load(stream);

            var element = report.XPathSelectElement("/report/counter[@type=\"LINE\"]");
            if (element == null)
            {
                throw new FormatException("Malformed Jacoco coverage report");
            }

            var linesCovered = int.Parse(element.Attribute("covered").Value);
            var linesValid = linesCovered + int.Parse(element.Attribute("missed").Value);

            _dataLoader.Add(new CoverageSummary(linesCovered, linesValid, testRunName));
        }
    }
}
