using System;
using System.Collections.Generic;
using System.Text;
using TestsHub.Data.DataModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions;
using System.Data;

namespace TestsHub.Data
{
    public class TestHubRepository : ITestHubRepository
    {
        private readonly Organisation _organisation;
        private readonly TestHubDBContext _testHubDBContext;
        public string Organisation => _organisation.Name;

        public IDbConnection DbConnection => _testHubDBContext.Database.GetDbConnection();

        public TestHubRepository(TestHubDBContext testHubDBContext, string organisation)
        {
            _testHubDBContext = testHubDBContext;
            _organisation = _testHubDBContext.Organisations.Single(o => o.Name == organisation);
        }

        public TestRun GetTestRun(string projectName, string testRunName)
        {
            var project = _testHubDBContext.Projects
                .FirstOrDefault(p => p.Name.Equals(projectName, StringComparison.OrdinalIgnoreCase) && p.Organisation.Id == _organisation.Id);

            if (project != null)
            {
                var testRun = _testHubDBContext.TestRuns
                    .FirstOrDefault(t => t.ProjectId == project.Id && t.TestRunName == testRunName);

                _testHubDBContext.Entry(testRun).Collection(c => c.TestCases);

                return testRun;
            }
            else
            {
                return null;
            }
        }

        public dynamic GetProjectSummary(string projectName)
        {
            var project = _testHubDBContext.Projects
                .Where(p => p.Name.Equals(projectName, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();

            var testRuns = _testHubDBContext.TestRuns
                .Where(r => r.ProjectId == project.Id)
                .GroupJoin(_testHubDBContext.TestCases,
                 r => r.Id,
                 c => c.TestRunId,
                 (r, c) => new
                 {
                     r.Name,
                     r.TestRunName,                  
                     r.Time,
                     r.Timestamp,
                     Count = new
                     {
                         Passed = c.Count(ic => ic.Status.Equals("Passed", StringComparison.OrdinalIgnoreCase)),
                         Failed = c.Count(ic => ic.Status.Equals("Failed", StringComparison.OrdinalIgnoreCase)),
                         Skipped = c.Count(ic => ic.Status.Equals("Skipped", StringComparison.OrdinalIgnoreCase))
                     }
                 });


            return new
            {
                Project = project.Name,
                TestRunsCount = testRuns.Count(),
                TestRuns = testRuns
            };
        }
    }

}