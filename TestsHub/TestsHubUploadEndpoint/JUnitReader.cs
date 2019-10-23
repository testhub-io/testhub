using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TestsHub.Data.DataModel;

namespace TestsHubUploadEndpoint
{
    public class JUnitReader
    {
        IDataLoader _dataLoader;
        public JUnitReader  (IDataLoader dataLoader)
        {
            _dataLoader = dataLoader;
        }

        public async Task Read(Stream stream, string testRunName)
        {
            var settings = new XmlReaderSettings
            {
                Async = true
            };

            var result = new List<TestCase>();
            var testRun = new TestRun();

            using (var reader = XmlReader.Create(stream, settings))
            {
                while (await reader.ReadAsync())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:                            
                            if (string.Equals(reader.Name, "testcase", StringComparison.OrdinalIgnoreCase))
                            {
                                var testCase = new TestCase();

                                if (reader.HasAttributes)
                                {                                   
                                    while (reader.MoveToNextAttribute())
                                    {
                                        switch (reader.Name)
                                        {
                                            case "name":
                                                testCase.Name = reader.Value;
                                                break;

                                            case "classname":
                                                testCase.ClassName = reader.Value;
                                                break;

                                            case "status":
                                                testCase.Status = reader.Value;
                                                break;

                                            case "time":
                                                testCase.Time = reader.Value;
                                                break;                                            

                                        }
                                    }

                                    // Move the reader back to the element node.
                                    reader.MoveToElement();
                                }

                                result.Add(testCase);
                            }
                            else if (string.Equals(reader.Name, "testsuite", StringComparison.OrdinalIgnoreCase))
                            {
                                if (reader.HasAttributes)
                                {
                                    while (reader.MoveToNextAttribute())
                                    {
                                        switch (reader.Name)
                                        {
                                            case "name":
                                                testRun.Name = reader.Value;
                                                break;

                                            case "hostname":
                                                testRun.Hostname = reader.Value;
                                                break;

                                            case "package":
                                                testRun.Package = reader.Value;
                                                break;

                                            case "junit_id":
                                                testRun.JUnitId = reader.Value;
                                                break;

                                            case "timestamp":
                                                testRun.Timestamp = DateTime.Parse(reader.Value);
                                                break;

                                            case "time":
                                                testRun.Time = decimal.Parse(reader.Value, System.Globalization.CultureInfo.InvariantCulture);
                                                break;

                                        }
                                    }

                                    // Move the reader back to the element node.
                                    reader.MoveToElement();
                                }

                            }
                            break;

                        case XmlNodeType.Text:
                            //Console.WriteLine("Text Node: {0}",
                            //         await reader.GetValueAsync());
                            break;

                        case XmlNodeType.EndElement:
                            //Console.WriteLine("End Element {0}", reader.Name);
                            break;

                        default:
                            //Console.WriteLine("Other node {0} with value {1}",
                            //                reader.NodeType, reader.Value);
                            break;
                    }
                }
            }
            testRun.TestCases = result;
            testRun.TestRunName = testRunName;
            _dataLoader.Add(testRun);
        }
    }
}
