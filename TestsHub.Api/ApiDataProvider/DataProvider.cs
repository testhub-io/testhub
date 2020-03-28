using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TestHub.Data.DataModel;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using TestHub.Api.Controllers;
using TestHub.Api.Data;

namespace TestHub.Api.ApiDataProvider
{
    public class DataProvider : IDataProvider
    {
        private readonly TestHub.Data.DataModel.Organisation _organisation;
        private readonly TestHubDBContext _testHubDBContext;
        private readonly UrlBuilder _urlBuilder;

        public string Organisation => _organisation.Name;

        public IDbConnection DbConnection => _testHubDBContext.Database.GetDbConnection();

        public TestHubDBContext TestHubDBContext { get { return _testHubDBContext; } }

        public int TestRunsCount { get; private set; }

        public DataProvider(TestHubDBContext testHubDBContext, string organisation, UrlBuilder url)
        {
            _testHubDBContext = testHubDBContext;
            _organisation = TestHubDBContext.Organisations.SingleOrDefault(o => o.Name == organisation);
            if (_organisation == null)
            {
                TestHubDBContext.Organisations.Add(new TestHub.Data.DataModel.Organisation() { Name = organisation });
                TestHubDBContext.SaveChanges();
            }

            _urlBuilder = url;
        }

        public Data.TestRun GetTestRun(string projectName, string testRunName)
        {
            var project = _testHubDBContext.Projects
                .FirstOrDefault(p => p.Name.Equals(projectName, StringComparison.OrdinalIgnoreCase)
                && p.Organisation.Id == _organisation.Id);

            if (project != null)
            {
                var testRun = _testHubDBContext.TestRuns
                    .FirstOrDefault(t => t.ProjectId == project.Id && t.TestRunName == testRunName);

                var history = GetTestCaseHistory(project, testRun.Id);

                var testCases = _testHubDBContext.TestCases.Where(t => t.TestRunId == testRun.Id)
                    .Select(s => new Data.TestCase
                    {
                        ClassName = s.ClassName,
                        File = s.File,
                        Name = s.Name,
                        Status = (Data.TestResult)s.Status,
                        SystemOut = s.TestOutput,
                        Time = s.Time,
                        RecentResults = history.ContainsKey(s.Name) ? history[s.Name] : null
                    });

                return new Data.TestRun
                {
                    Name = testRun.TestRunName,
                    Uri = _urlBuilder.Action("Get", "TestRuns", new { org = Organisation, project = project.Name, testRun = testRun.TestRunName }),
                    Summary = new Data.TestRunSummary
                    {
                        TestsCount = testCases.Count(),
                        Passed = testCases.Count(t => t.Status == Data.TestResult.Passed),
                        Failed = testCases.Count(t => t.Status == Data.TestResult.Failed),
                        Skipped = testCases.Count(t => t.Status == Data.TestResult.Skipped)
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

        private Dictionary<string, IEnumerable<Data.TestResult>> GetTestCaseHistory(TestHub.Data.DataModel.Project project, int testrunId)
        {
            var recentTrs = _testHubDBContext.TestRuns.Where(
                    t => t.ProjectId == project.Id && t.Id < testrunId).OrderBy(t => t.Timestamp).Take(5).Select(t => t.Id).ToList();

            var history = _testHubDBContext.TestCases.Where(c => recentTrs.Contains(c.TestRunId))
                .GroupBy(c => c.Name,
                 pair => pair,
                 (k, c) => new { Key = k, Statuses = c.Select(c2 => (Data.TestResult)c2.Status) })
                .ToDictionary(g => g.Key, v => v.Statuses);

            return history;
        }

        public Data.Project GetProjectSummary(string projectName)
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
                     (r, c) => new Data.TestRunSummary
                     {
                         Name = r.TestRunName,
                         Time = r.Time,
                         TimeStemp = r.Timestamp,
                         Uri = _urlBuilder.Action("Get", "TestRuns", new { org = Organisation, project = project.Name, testRun = r.TestRunName}),
                         Count = new Data.TestRunStats
                         {
                             Passed = c.Count(ic => ic.Status == TestHub.Data.DataModel.TestResult.Passed),
                             Failed = c.Count(ic => ic.Status == TestHub.Data.DataModel.TestResult.Failed),
                             Skipped = c.Count(ic => ic.Status == TestHub.Data.DataModel.TestResult.Skipped)
                         }
                     });


                return new Data.Project
                {
                    Name = project.Name,
                    TestRunsCount = testRuns.Count(),
                    TestRuns = testRuns.ToList()
                };
            }

            return null;
        }

        public Data.Organisation GetOrgSummary(string org)
        {
            var organisation = getOrganisation(org);
            if (organisation != null)
            {
                return new Data.Organisation
                {
                    Name = organisation.Name,
                    Uri = _urlBuilder.Action("Get", "Organisation", new { org = Organisation }),
                    Projects = _urlBuilder.Action("GetProjects", "Organisation", new { org = Organisation }),

                    Summary = new OrgSummary()
                    {
                        ProjectsCount = _testHubDBContext.OrganisationProjects(org).Count(),
                        // there is a bug in MYsql provider that does not allow to use avg
                        AvgTestsCount = _testHubDBContext.OrganisationTestRun(org).Sum(t => t.TestCasesCount) / _testHubDBContext.OrganisationTestRun(org).Count(),
                        AvgCoverage = 65
                    }
                };
            }
            else
            {
                return null;
            }
        }
       

        private TestHub.Data.DataModel.Organisation getOrganisation(string org)
        {
            return _testHubDBContext.Organisations              
              .FirstOrDefault(o => o.Name.Equals(org, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<ProjectSummary> GetProjects(string orgName)
        {
            var org = getOrganisation(orgName);
            if (org != null)
            {
                return getProjectsSummary(org);
            }
            else
            {
                return new List<ProjectSummary>();
            }            
        }

        private IQueryable<ProjectSummary> getProjectsSummary(TestHub.Data.DataModel.Organisation org)
        {
            int id = org.Id;

            var groups = from t in _testHubDBContext.TestRuns
                         group t by t.ProjectId into c
                         select new ProjectSummary
                         {
                             Name = $"Peoject ID {c.Key}",
                             TestRunsCount = c.Count(),
                             RecentTestRuntDate = c.OrderByDescending(s => s.Timestamp).First().Timestamp,
                             Uri = _urlBuilder.Action("Get", "Projects", new { org = org.Name, project = $"Peoject ID {c.Key}" }),
                             LatestResults = new Data.LatestResults()
                             {
                                 TestResults = c.OrderByDescending(s => s.Timestamp)
                                     .Take(5)
                                     .Select(t => (Data.TestResult)t.Status)
                                     .ToArray()
                             },
                             TestsCount = c.OrderByDescending(s => s.Timestamp).First().TestCases.Count,
                             TestQuantityGrowth = getQuantityGrowth(c),
                             TestCoverageGrowth = getCoverageGrowth(c.OrderByDescending(s => s.Timestamp).Take(2).Select(t => t.Coverage))
                         };
            return groups;


            //return _testHubDBContext.Projects
            //    .Where(r => r.OrganisationId == id)
            //    .GroupJoin(_testHubDBContext.TestRuns,
            //        r => r.Id,
            //        c => c.ProjectId,
            //        (r, c) => new ProjectSummary
            //        {
            //            Name = r.Name,
            //            TestRunsCount = c.Count(),
            //            //RecentTestRuntDate = c.OrderByDescending(s => s.Timestamp).First().Timestamp,
            //            //Uri = _urlBuilder.Action("Get", "Projects", new { org = org.Name, project = r.Name }),
            //            //LatestResults = new Data.LatestResults()
            //            //{
            //            //    TestResults = c.OrderByDescending(s => s.Timestamp)
            //            //        .Take(5)
            //            //        .Select(t => (Data.TestResult)t.Status)
            //            //        .ToArray()
            //            //},
            //            //TestsCount = c.OrderByDescending(s => s.Timestamp).First().TestCases.Count,
            //            //TestQuantityGrowth = getQuantityGrowth(c),
            //            //TestCoverageGrowth = getCoverageGrowth(c.OrderByDescending(s => s.Timestamp).Take(2).Select(t => t.Coverage))
            //        });           
        }

        private decimal getQuantityGrowth(IEnumerable<TestHub.Data.DataModel.TestRun> c)
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


    }

}