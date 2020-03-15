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

        public int TestRunsCount { get; private set; }

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

        public Api.Data.TestRun GetTestRun(string projectName, string testRunName)
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
                    .Select(s => new Api.Data.TestCase
                    {
                        ClassName = s.ClassName,
                        File = s.File,
                        Name = s.Name,
                        Status = (Api.Data.TestResult)s.Status,
                        SystemOut = s.TestOutput,
                        Time = s.Time,
                        RecentResults = history.ContainsKey(s.Name) ? history[s.Name] : null
                    });

                return new Api.Data.TestRun
                {
                    Name = testRun.TestRunName,
                    Uri = BuildUri(Organisation, project.Name, testRun.TestRunName),
                    Summary = new Api.Data.TestRunSummary
                    {
                        TestsCount = testCases.Count(),
                        Passed = testCases.Count(t => t.Status == Api.Data.TestResult.Passed),
                        Failed = testCases.Count(t => t.Status == Api.Data.TestResult.Failed),
                        Skipped = testCases.Count(t => t.Status == Api.Data.TestResult.Skipped)
                    },
                     Branch = testRun.Branch,
                     CommitId = testRun.CommitId,
                     Coverage = testRun.Coverage?.Percent,
                     Timestamp = testRun.Timestamp,
                     Time = testRun.Time,
                     TestCases = testCases
                    
                };
            }
            else
            {
                return null;
            }
        }

        private Dictionary<string, IEnumerable<Api.Data.TestResult>> GetTestCaseHistory(Project project, TestRun testRun)
        {
            var recentTrs = _testHubDBContext.TestRuns.Where(
                    t => t.ProjectId == project.Id && t.Id < testRun.Id).OrderBy(t => t.Timestamp).Take(5).Select(t => t.Id).ToList();

            var history = _testHubDBContext.TestCases.Where(c => recentTrs.Contains(c.TestRunId))
                .GroupBy(c => c.Name,
                 pair => pair, 
                 (k,c)=> new { Key = k, Statuses = c.Select(c2 => (Api.Data.TestResult)c2.Status) })
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
                             Passed = c.Count(ic => ic.Status == TestResult.Passed),
                             Failed = c.Count(ic => ic.Status == TestResult.Failed),
                             Skipped = c.Count(ic => ic.Status == TestResult.Skipped)
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

        public Api.Data.Organisation GetOrgSummary(string org)
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
                   (r, c) => new Api.Data.ProjectSummary
                   {
                       Name = r.Name,                       
                       TestRunsCount = c.Count(),
                       RecentTestRuntDate = c.OrderByDescending(s => s.Timestamp).First().Timestamp,                       
                       Uri = BuildUri(org, r.Name),
                       LatestResults = new Api.Data.LatestResults()
                       {
                           TestResults = c.OrderByDescending(s => s.Timestamp)
                               .Take(5)
                               .Select(t => (Api.Data.TestResult)t.Status)
                               .ToArray()
                       },
                       TestsCount = c.OrderByDescending(s => s.Timestamp).First().TestCases.Count,
                       TestQuantityGrowth = getQuantityGrowth(c),
                       TestCoverageGrowth = getCoverageGrowth(c.OrderByDescending(s => s.Timestamp).Take(2).Select(t=>t.Coverage))
                   });

                return new Api.Data.Organisation 
                {
                    Name = organisation.Name,
                    Uri = BuildUri(org),
                    Projects = projects.ToList()
                };
            }
            else
            {
                return null;
            }
        }

        private decimal getQuantityGrowth(IEnumerable<TestRun> c)
        {
            var t = c.OrderByDescending(s => s.Timestamp).Take(2);
            if (t.Count() == 2)
            {
                return t.Last().TestCasesCount - t.First().TestCasesCount;
            }

            return 0;
        }

        private decimal? getCoverageGrowth(IEnumerable<Coverage> c)
        {          
            if (c.Count() == 2)
            {                
                var coverageLast = c.First()?.Percent;
                var coveragePrev = c.Last()?.Percent;
                if (coverageLast.HasValue && coveragePrev.HasValue)
                {
                    return coverageLast.Value - coveragePrev.Value;
                }                
            }

            return null;
        }


        private string BuildUri(params string [] uriParts)
        {
            return string.Join('/', uriParts);
        }
    }

}