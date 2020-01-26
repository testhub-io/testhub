﻿using System;
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
        private const string PassedTestValue = "Passed";
        private const string FailedTestValue = "Failed";
        private const string SkippedTestValue = "Skipped";
        private readonly Organisation _organisation;
        private readonly TestHubDBContext _testHubDBContext;
        private const int RECORDS_LIMIT = 200;
        public string Organisation => _organisation.Name;

        public IDbConnection DbConnection => _testHubDBContext.Database.GetDbConnection();
        public TestHubDBContext TestHubDBContext => _testHubDBContext;

        public TestHubRepository(TestHubDBContext testHubDBContext, string organisation)
        {
            _testHubDBContext = testHubDBContext;
            _organisation = _testHubDBContext.Organisations.SingleOrDefault(o => o.Name == organisation);
            if (_organisation == null)
            {
                _testHubDBContext.Organisations.Add(new DataModel.Organisation() { Name = organisation });
                _testHubDBContext.SaveChanges();
            }
        }

        public dynamic GetTestRun(string projectName, string testRunName)
        {
            var project = _testHubDBContext.Projects
                .FirstOrDefault(p => p.Name.Equals(projectName, StringComparison.OrdinalIgnoreCase) 
                && p.Organisation.Id == _organisation.Id);

            if (project != null)
            {
                var testRun = _testHubDBContext.TestRuns
                    .FirstOrDefault(t => t.ProjectId == project.Id && t.TestRunName == testRunName);

                var history = GetTestCaseHistory(project, testRun);

                var testCases = _testHubDBContext.TestCases.Where(t => t.TestRunId == testRun.Id)
                    .Select(s => new
                    {
                        s.ClassName,
                        s.File,
                        s.Name,
                        s.Status,
                        s.TestOutput,
                        s.Time,
                        recentResults = history[s.Name]
                    }).Take(RECORDS_LIMIT);

                return new
                {
                    Name = testRun.TestRunName,
                    uri = BuildUri(Organisation, project.Name, testRun.TestRunName),
                    Summary = new
                    {
                        TestsCount = testCases.Count(),
                        Passed = testCases.Count(t => t.Status.Equals(PassedTestValue, StringComparison.OrdinalIgnoreCase)),
                        Failed = testCases.Count(t => t.Status.Equals(FailedTestValue, StringComparison.OrdinalIgnoreCase)),
                        Skipped = testCases.Count(t => t.Status.Equals(SkippedTestValue, StringComparison.OrdinalIgnoreCase))
                    },
                    TestCases = testCases,

                };
            }
            else
            {
                return null;
            }
        }

        private Dictionary<string, IEnumerable<string>> GetTestCaseHistory(Project project, TestRun testRun)
        {
            //@"SELECT name, status  from TestCases c
            //        inner join(select id from TestRuns WHERE projectid = 1
            //        order by TimeStamp desc LIMIt 3) tr
            //        on tr.id = c.TestRunId
            //        order by c.Name"

            var recentTrs = _testHubDBContext.TestRuns.Where(
                    t => t.ProjectId == project.Id && t.Id < testRun.Id).OrderBy(t => t.Timestamp).Take(5).Select(t => t.Id).ToList();

            var history = _testHubDBContext.TestCases.Where(c => recentTrs.Contains(c.TestRunId))
                .GroupBy(c => c.Name,
                 pair => pair, 
                 (k,c)=> new { Key = k, Statuses = c.Select(c2 => c2.Status) })
                .ToDictionary(g => g.Key, v=> v.Statuses);
            
            return history;
        }

        public dynamic GetProjectSummary(string projectName)
        {
            var project = _testHubDBContext.Projects
                .Where(p => p.Name.Equals(projectName, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();
            if (project != null)
            {
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
                         uri = BuildUri(Organisation, project.Name, r.TestRunName),
                         Count = new
                         {
                             Passed = c.Count(ic => ic.Status.Equals(PassedTestValue, StringComparison.OrdinalIgnoreCase)),
                             Failed = c.Count(ic => ic.Status.Equals(FailedTestValue, StringComparison.OrdinalIgnoreCase)),
                             Skipped = c.Count(ic => ic.Status.Equals(SkippedTestValue, StringComparison.OrdinalIgnoreCase))
                         }
                     }).Take(RECORDS_LIMIT);


                return new
                {
                    Project = project.Name,
                    TestRunsCount = testRuns.Count(),
                    TestRuns = testRuns.ToList()
                };
            }

            return null;
        }

        public dynamic GetOrgSummary(string org)
        {
            var organisation = _testHubDBContext.Organisations 
              .Where(o => o.Name.Equals(org, StringComparison.OrdinalIgnoreCase))
              .FirstOrDefault();
            if (organisation != null)
            {
                var projects = _testHubDBContext.Projects
                  .Where(r => r.OrganisationId == organisation.Id)
                  .GroupJoin(_testHubDBContext.TestRuns,
                   r => r.Id,
                   c => c.ProjectId,
                   (r, c) => new
                   {
                       r.Name,
                       Status = new
                       {
                           TestRunsCount = c.Count(),
                           RecentTestRun = c.OrderBy(s => s.Timestamp).First().Timestamp
                       },
                       uri = BuildUri(org, r.Name)
                   });

                return new
                {
                    Name = organisation.Name,
                    uri = BuildUri(org),
                    Projects = projects.ToList()
                };
            }
            else
            {
                return null;
            }
        }

        private string BuildUri(params string [] uriParts)
        {
            return string.Join('/', uriParts);
        }
    }

}