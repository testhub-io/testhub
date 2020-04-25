using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TestHub.Api.Data;
using TestHub.Data.DataModel;

namespace TestHub.Api.ApiDataProvider
{
    public class DataProvider : IDataProvider
    {

        private const string DEFAULT_BRANCH = "develop";
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
            _organisation = TestHubDBContext.Organisations.SingleOrDefault(o => o.Name.Equals(organisation, StringComparison.OrdinalIgnoreCase));
            if (_organisation == null)
            {
                TestHubDBContext.Organisations.Add(new TestHub.Data.DataModel.Organisation() { Name = organisation.ToLower() });
                TestHubDBContext.SaveChanges();
            }
            _organisation = getOrganisation(organisation);

            _urlBuilder = url;
        }

        public IEnumerable<TestRunSummary> GetTestRuns(string projectName)
        {
            var project = _testHubDBContext.Projects.First(p => p.Name.Equals(projectName, StringComparison.CurrentCultureIgnoreCase));
            var testRuns = _testHubDBContext.TestRuns.Where(t => t.ProjectId == project.Id).OrderByDescending(t => t.Timestamp).Include(c => c.Coverage);
            TestHub.Data.DataModel.TestRun previousTestRun = null;
            decimal previousCoverage = 0;
            foreach (var t in testRuns)
            {
                var currentCoverage = t.Coverage?.Percent ?? 0;

                // we returning previous test run because we go descending and want to calc difference ! 
                if (previousTestRun != null)
                {
                    yield return PrepareTestSummary(project, previousTestRun, currentCoverage, t, previousCoverage);                    
                }

                previousTestRun = t;
                previousCoverage = currentCoverage;
            }

            if (previousTestRun != null)
            {
                yield return PrepareTestSummary(project, null, 0, previousTestRun, 0);
            }
        }

        private TestRunSummary PrepareTestSummary(Project project, TestHub.Data.DataModel.TestRun t, decimal currentCoverage, TestHub.Data.DataModel.TestRun previousTestRun, decimal previousCoverage)
        {
            return new TestRunSummary()
            {
                Name = previousTestRun.TestRunName,
                Branch = previousTestRun.Branch,
                Coverage = currentCoverage,
                Result = (Data.TestResult)previousTestRun.Status,
                Stats = new TestRunStats()
                {
                    TotalCount = previousTestRun.TestCasesCount,

                },
                CoverageGrowth = currentCoverage - previousCoverage,
                TestCountGrowth = previousTestRun.TestCasesCount - (t?.TestCasesCount ?? 0),
                Time = previousTestRun.Time,
                TimeStemp = previousTestRun.Timestamp,
                Uri = _urlBuilder.Action("Get", "TestRuns", new { org = Organisation, project = project.Name, testRun = previousTestRun.TestRunName })
            };
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

        public Data.Organisation GetOrgSummary()
        {
            var organisation = getOrganisation(Organisation);    

            if (organisation != null)
            {
                var result = _testHubDBContext.Query<(decimal cov, int casesCount, int status, int pId)>(@"WITH tc
    AS(
      SELECT
           Timestamp,
           Status,
           TestRunName,
           ProjectId,
           TestCasesCount,
           id,
           Branch, Time, CommitId,
           ROW_NUMBER() OVER(PARTITION BY ProjectId) row_num
        FROM
           TestRuns
        where Branch = @branch or Branch is NULL
        order by Timestamp DESC
     )
    SELECT
      c.LinesCovered / c.LinesValid,
      r.TestCasesCount,
      r.Status,
      ProjectId
    FROM
       tc r
    inner join Projects p on p.Id = r.ProjectId
    left join Coverage c on c.Id = r.Id
    WHERE
       p.OrganisationId = @orgId and row_num <= 1;", new { branch = DEFAULT_BRANCH, orgId = organisation.Id }).ToList();
                
                return new Data.Organisation
                {
                    Name = organisation.Name,
                    Uri = _urlBuilder.Action("Get", "Organisation", new { org = Organisation }),
                    Projects = _urlBuilder.Action("GetProjects", "Projects", new { org = Organisation }),
                    Coverage = _urlBuilder.Action("GetCoverage", "Organisation", new { org = Organisation }),

                    Summary = new OrgSummary()
                    {
                        ProjectsCount = _testHubDBContext.OrganisationProjects(Organisation).Count(),                        
                        AvgTestsCount = result.DefaultIfEmpty().Average(c=>c.casesCount),
                        AvgCoverage = result.DefaultIfEmpty().Average(c => c.cov), 
                        ProjectsInGreen = result.Where(c=>c.status == 1).Count(),
                        ProjectsInRed = result.Where(c => c.status == 0).Count()
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

        public IEnumerable<ProjectSummary> GetProjects()
        {

            var org = getOrganisation(Organisation);
            if (org != null)
            {
                return getProjectsSummary(org);
            }
            else
            {
                return new List<ProjectSummary>();
            }
        }

        private IEnumerable<ProjectSummary> getProjectsSummary(TestHub.Data.DataModel.Organisation org)
        {
            var id = org.Id;

            var tr = getLast5TestRuns(org);

            var projects = _testHubDBContext.Query<ProjectExtended>(
                @"select p.Name, p.Id, count(t.Id) as TestRunsCount
                        from Projects p 
                      inner join TestRuns t on t.ProjectId = p.id
                      where OrganisationId = @orgId
                    group by p.Id"
                    , new { orgId = org.Id }).ToDictionary(k => k.Id);

            var groups = tr.GroupBy(t => t.ProjectId)
                .Select(
                       g => new ProjectSummary
                       {
                           Name = projects[g.Key].Name,
                           TestRunsCount = projects[g.Key].TestRunsCount,
                           RecentTestRuntDate = g.First().Timestamp,
                           Uri = _urlBuilder.Action("Get", "Projects", new { org = org.Name, project = projects[g.Key].Name }),
                           LatestResults = new LatestResults()
                           {
                               TestResults = g.Select(t => (Data.TestResult)t.Status).ToArray()
                           },
                           TestsCount = g.First().TestCasesCount,
                           TestQuantityGrowth = getQuantityGrowth(g),
                           CoverageGrowth = getCoverageGrowth(g)
                       });
            return groups.AsEnumerable();
        }

        private IEnumerable<TestRunExtended> getLast5TestRuns(TestHub.Data.DataModel.Organisation org)
        {
            return _testHubDBContext.Query<TestRunExtended>(@"WITH tc
                                                        AS(
                                                          SELECT
                                                               Timestamp,
                                                               Status,
                                                               TestRunName,
                                                               ProjectId,
                                                               TestCasesCount,
                                                               id,
                                                                Branch, Time, CommitId,
                                                               ROW_NUMBER() OVER(
                                                                  PARTITION BY ProjectId
                                                                  ) row_num
                                                            FROM
                                                               TestRuns
                                                            order by Timestamp DESC
                                                         )
                                                        SELECT
                                                            Timestamp,
                                                            Status,
                                                            TestRunName,
                                                            ProjectId,
                                                            row_num,
                                                            TestCasesCount,
                                                            p.Name,
                                                            r.id ,
                                                            Branch, Time, CommitId                                                            
                                                        FROM
                                                           tc r
                                                        inner join Projects p on p.Id = r.ProjectId
                                                        left join Coverage c on c.Id = r.Id
                                                        WHERE
                                                           p.OrganisationId = @orgId and  row_num <= 5",
                                                        new { orgId = org.Id });
        }

        private decimal getQuantityGrowth(IEnumerable<TestRunExtended> c)
        {
            var t = c.OrderByDescending(s => s.Timestamp).Take(2);
            if (t.Count() == 2)
            {
                return t.Last().TestCasesCount - t.First().TestCasesCount;
            }

            return 0;
        }

        private decimal? getCoverageGrowth(IEnumerable<TestRunExtended> c)
        {
            if (c.Count() == 2)
            {
                var coverageLast = c.First()?.CoveragePercent;
                var coveragePrev = c.Last()?.CoveragePercent;
                if (coverageLast.HasValue && coveragePrev.HasValue)
                {
                    return coverageLast.Value - coveragePrev.Value;
                }
            }

            return null;
        }

        public ProjectSummary GetProjectSummary(string project)
        {
            throw new NotImplementedException();
        }
    }

}