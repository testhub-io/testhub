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

        public async Task Read(Stream stream)
        {
            var settings = new XmlReaderSettings
            {
                Async = true
            };

            var result = new List<TestCase>();

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

                                _dataLoader.Add(testCase);
                            }
                            break;

                        case XmlNodeType.Text:
                            Console.WriteLine("Text Node: {0}",
                                     await reader.GetValueAsync());
                            break;

                        case XmlNodeType.EndElement:
                            Console.WriteLine("End Element {0}", reader.Name);
                            break;

                        default:
                            Console.WriteLine("Other node {0} with value {1}",
                                            reader.NodeType, reader.Value);
                            break;
                    }
                }
            }
        }
    }
}
