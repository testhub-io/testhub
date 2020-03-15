using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TestsHubUploadEndpoint.ReportModel;

namespace TestsHubUploadEndpoint
{
    public class JUnitReader
    {
        IDataLoader _dataLoader;
        public IVisitor Visitor { get; set; }

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

            var testCases = new List<TestCase>();
            var testRun = new TestRun() { Timestamp = DateTime.UtcNow };

            using (var reader = XmlReader.Create(stream, settings))
            {
                TestSuite testSuite = null;
                while (await reader.ReadAsync())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:                            
                            if (string.Equals(reader.Name, "testcase", StringComparison.OrdinalIgnoreCase))
                            {
                                var testCase = new TestCase
                                {
                                    TestSuite = testSuite
                                };

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
                                                // ignore status, it's optional in schema
                                                testCase.Status = "passed";
                                                break;

                                            case "time":
                                                testCase.Time = ParseFloatWithDefault(reader.Value);

                                                break;

                                        }
                                    }

                                                                        
                                    // Move the reader back to the element node.
                                    reader.MoveToElement();
                   
                                    if (!ReadTestCaseErrorContent("failure", reader, testCase) 
                                        && !ReadTestCaseErrorContent("error", reader, testCase)
                                        && !ReadTestCaseErrorContent("system-err", reader, testCase)
                                        && reader.ReadToDescendant("skipped"))
                                    {
                                        testCase.Status = "skipped";
                                    }
                                    else if (reader.ReadToDescendant("system-out"))
                                    {
                                        testCase.Status = "passed";
                                        testCase.TestOutput = reader.ReadContentAsString();
                                    }
                                }


                                Visitor?.TestCaseAdded(testCase);
                                testCases.Add(testCase);
                            }                     
                            else if (string.Equals(reader.Name, "testsuite", StringComparison.OrdinalIgnoreCase))
                            {
                                testSuite = new TestSuite();
                                if (reader.HasAttributes)
                                {
                                    while (reader.MoveToNextAttribute())
                                    {                                        
                                        switch (reader.Name)
                                        {
                                            case "name":
                                                testSuite.Name = reader.Value;
                                                break;

                                            case "hostname":
                                                testSuite.Hostname = reader.Value;
                                                break;

                                            case "package":
                                                testSuite.Package = reader.Value;
                                                break;

                                            case "junit_id":
                                                testSuite.JUnitId = reader.Value;
                                                break;

                                            case "timestamp":
                                                testSuite.Timestamp = DateTime.Parse(reader.Value);
                                                break;

                                            case "time":
                                                testSuite.Time = decimal.Parse(reader.Value, System.Globalization.CultureInfo.InvariantCulture);
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
      
            testRun.TestRunName = testRunName;
            testRun.TestCasesCount = testRun.TestCasesCount + testCases.Count;
            _dataLoader.Add(testRun, testCases);

        }

        private static decimal ParseFloatWithDefault(string value)
        {
            if (decimal.TryParse(value,System.Globalization.NumberStyles.Float,
                    CultureInfo.InvariantCulture,  out var decimalTime))
            {
                return decimalTime;
            }
            else if (float.TryParse(value, out var floatTime))
            {
                return Convert.ToDecimal(floatTime);
            }
            else
            {
                return 0;
            }
        }

        bool ReadTestCaseErrorContent(string elementName, XmlReader reader, TestCase testCase)
        {            
            if (reader.ReadToDescendant(elementName))
            {
                testCase.Status = "failed";
                var errorText = new StringBuilder();
                do
                {
                    errorText.AppendLine(reader.ReadElementContentAsString());
                    
                } while (reader.ReadToNextSibling(elementName));
                testCase.TestOutput = errorText.ToString();
                return true;
            }
            return false;
        }
    }
}
