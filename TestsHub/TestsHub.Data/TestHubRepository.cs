using System;
using System.Collections.Generic;
using System.Text;
using TestsHub.Data.DataModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TestsHub.Data
{
    public class TestHubRepository : ITestHubRepository
    {
        private readonly Organisation _organisation;
        private readonly TestHubDBContext _testHubDBContext;
        public string Organisation => _organisation.Name;

        public TestHubRepository(TestHubDBContext testHubDBContext, string organisation)
        {
            _testHubDBContext = testHubDBContext;
            _organisation = _testHubDBContext.Organisations.Single(o => o.Name == organisation);
        }

        public TestRun GetTestRun(string projectName, string testRunName)
        {
            var project = _testHubDBContext.Projects
                .FirstOrDefault(p => p.Name == projectName && p.Organisation.Id == _organisation.Id);
            
            if (project != null) 
            {
                var testRun = _testHubDBContext.TestRuns
                    .FirstOrDefault(t => t.ProjectId == project.Id &&  t.TestRunName == testRunName);
                
                _testHubDBContext.Entry(testRun).Collection(c => c.TestCases);

                return testRun;
            }
            else
            {
                return null;
            }
        }
    }
}