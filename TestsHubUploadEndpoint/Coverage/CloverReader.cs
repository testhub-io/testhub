using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;

namespace TestsHubUploadEndpoint.Coverage
{
    public class CloverReader : ICoverageReader
    {
        private readonly IDataLoader _dataLoader;

        public CloverReader(IDataLoader dataLoader)
        {
            _dataLoader = dataLoader;
        }

        public void Read(System.IO.Stream stream, string testRunName)
        {
            var report = XDocument.Load(stream);

            // .net coverage xml
            var linesCovered = 0;
            var linesValid = 0;

            foreach (var element in report.XPathSelectElements("/coverage/project"))
            {

                var metrics = element.XPathSelectElement("metrics");
                var covered = int.Parse(metrics.Attribute("coveredstatements").Value);
                linesCovered += covered;

                linesValid += int.Parse(metrics.Attribute("statements").Value);
            }

            _dataLoader.Add(new CoverageSummary(linesCovered, linesValid, testRunName));

        }
    }
}
