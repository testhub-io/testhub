﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestsHub.Data.DataModel;
using RandomNameGenerator;

namespace TestsHubUploadEndpoint
{
    public class DataLoader : IDataLoader
    {
        TestHubDBContext _testHubDBContext = new TestHubDBContext();

        public DataLoader(TestHubDBContext testHubDBContext, string projectName)
        {
            _testHubDBContext = testHubDBContext;
            ProjectName = projectName;
        }

        public string ProjectName { get; }

        public void Add(TestRun testRun)
        {
            var project = _testHubDBContext.Projects.SingleOrDefault(p => p.Name == ProjectName);
            if (project == null && testRun.Project == null)
            {
                project = new Project()
                {
                    Name = ProjectName
                };
                _testHubDBContext.Projects.Add(project);
            }

            testRun.Project = project;
            var testCases = testRun.TestCases;
            testRun.TestCases = null;
            _testHubDBContext.TestRuns.Add(testRun);
            _testHubDBContext.SaveChanges();

            BatchInsert(testCases, testRun.Id);
        }

        private void BatchInsert(List<TestCase> testCases, int testRunId)
        {
            var batch = new List<TestCase>();
            for (var i = 0; i < testCases.Count; i++)
            {
                testCases[i].TestRunId = testRunId;
                batch.Add(testCases[i]);
                if (i % 1000 == 0)
                {
                    _testHubDBContext.TestCases.AddRange(batch);
                    _testHubDBContext.SaveChanges();
                    batch = new List<TestCase>();
                }
            }

            _testHubDBContext.TestCases.AddRange(batch);
            _testHubDBContext.SaveChanges();
        }

        public void Dispose()
        {
            _testHubDBContext.Dispose();
        }
    }
}
