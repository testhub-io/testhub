using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TestHub.Api.Controllers;
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
            var project = _testHubDBContext.Projects.FirstOrDefault(p => p.Name.Equals(projectName, StringComparison.CurrentCultureIgnoreCase));
            if (project == null)
            {
                TesthubApiException.ThrowProjectDoesNotExist(projectName);
            }

            var testRuns = _testHubDBContext.TestRuns.Where(t => t.ProjectId == project.Id)
                .OrderByDescending(t => t.Timestamp).Include(c => c.Coverage);
            TestHub.Data.DataModel.TestRun previousTestRun = null;
            decimal? previousCoverage = null;
            foreach (var t in testRuns)
            {
                var currentCoverage = t.Coverage?.Percent ?? null;

                // we returning previous test run because we go descending and want to calc difference ! 
                if (previousTestRun != null)
                {
                    yield return PrepareTestSummary(project, previousTestRun, previousCoverage, t, currentCoverage);                    
                }

                previousTestRun = t;
                previousCoverage = currentCoverage;
            }

            if (previousTestRun != null)
            {
                yield return PrepareTestSummary(project, previousTestRun, 0, null, 0);
            }
        }

        private TestRunSummary PrepareTestSummary(Project project, TestHub.Data.DataModel.TestRun t, decimal? currentCoverage, TestHub.Data.DataModel.TestRun previousTestRun, decimal? previousCoverage)
        {
            return new TestRunSummary()
            {
                Name = t.TestRunName,
                Branch = t.Branch,
                Coverage = currentCoverage,
                Result = (Data.TestResult)t.Status,
                Stats = new TestRunStats()
                {
                    TotalCount = t.TestCasesCount,

                },
                CoverageGrowth = !currentCoverage.HasValue ? null : (currentCoverage - previousCoverage),
                TestCountGrowth = t.TestCasesCount - (previousTestRun?.TestCasesCount ?? 0),
                Time = t.Time,
                Timestamp = t.Timestamp,
                Uri = _urlBuilder.Action("Get", typeof(TestRunsController), new { org = Organisation, project = project.Name, testRun = t.TestRunName })
            };        
        }

        public TestRunTests GetTests(string projectName, string testRunName)
        {
            var testRun = (from tr in _testHubDBContext.TestRuns
                           join p in _testHubDBContext.Projects on tr.ProjectId equals p.Id
                           where p.Name == projectName && tr.TestRunName == testRunName
                           select new { tr.Id, ProjectId = p.Id, tr.Branch }).FirstOrDefault();

            if (testRun == null)
            {
                throw new TesthubApiException("Test run does not exist");
            }

            var history = GetTestCaseHistory(testRun.ProjectId, projectName, testRun.Id, testRun.Branch);

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

            return new TestRunTests
            {
                Tests = testCases,
                Uri = _urlBuilder.Action("GetTests", typeof(TestRunsController), new { org = Organisation, project = projectName, testRun = testRunName })
            };
        }

        public Data.TestRun GetTestRunSummary(string projectName, string testRunName)
        {
            var project = getProjectIntity(projectName);

            var testRun = _testHubDBContext.TestRuns
                .Include(c=>c.Coverage)
                .Include(c=>c.TestCases)
                .FirstOrDefault(t => t.ProjectId == project.Id && t.TestRunName.Equals(testRunName, StringComparison.OrdinalIgnoreCase));
                      
            if (testRun == null)
            {
                TesthubApiException.ThrowTestRunDoesNotExist(testRunName);
            }

            return new Data.TestRun
            {
                Name = testRun.TestRunName,
                Uri = _urlBuilder.Action("Get", typeof(TestRunsController), new { org = Organisation, project = project.Name, testRun = testRun.TestRunName }),
                Branch = testRun.Branch,
                CommitId = testRun.CommitId,
                Coverage = testRun.Coverage?.Percent,
                Timestamp = testRun.Timestamp,
                Time = testRun.Time,
                TestCasesCount = testRun.TestCasesCount,
                Summary = new TestRunStats
                {
                     // TODO: Optimize, could be done in one go 
                     Failed = testRun.TestCases.Count(t=>t.Status == TestHub.Data.DataModel.TestResult.Failed),
                     Passed = testRun.TestCases.Count(t => t.Status == TestHub.Data.DataModel.TestResult.Passed),
                     Skipped  = testRun.TestCases.Count(t => t.Status == TestHub.Data.DataModel.TestResult.Skipped),
                }                
            };
        }

        private Project getProjectIntity(string projectName)
        {
            var project = _testHubDBContext.Projects
                .FirstOrDefault(p => p.Name.Equals(projectName, StringComparison.OrdinalIgnoreCase)
                && p.Organisation.Id == _organisation.Id);

            if (project == null)
            {
                TesthubApiException.ThrowProjectDoesNotExist(projectName);
            }

            return project;
        }

        private Dictionary<string, IEnumerable<Data.TestResultItem>> GetTestCaseHistory(int projectId, string projectName, int testrunId, string branch)
        {
            var recentTrs = _testHubDBContext.TestRuns
                .Where(tr => tr.ProjectId == projectId && tr.Id < testrunId && tr.Branch.Equals(branch, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(tr=>tr.Id)
                .Take(5)
                .ToDictionary(t=>t.Id);


            var recentTestCases = from t in _testHubDBContext.TestCases
                            where recentTrs.Keys.Contains (t.TestRunId) 
                            select new 
                            { 
                                t.Name, 
                                t.Status, 
                                recentTrs[t.TestRunId].TestRunName,
                                recentTrs[t.TestRunId].Timestamp
                            };
            
            var res = recentTestCases
                .AsEnumerable()
                .GroupBy( p => p.Name, (key, g) => new { Id = key, Results = g })
                .ToDictionary(s => s.Id, s=> s.Results.Select(rs=> new TestResultItem {
                     Status = (Data.TestResult)rs.Status, 
                     TestRunName = rs.TestRunName,
                     Uri = _urlBuilder.Action("Get", typeof(TestRunsController), new { org = Organisation, project = projectName, testRun = rs.TestRunName }),
                     Timestamp = rs.Timestamp
                    }).Take(5));

            return res;
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
           ROW_NUMBER() OVER(PARTITION BY ProjectId  order by Timestamp DESC) row_num
        FROM
           TestRuns
        where Branch = @branch or Branch = 'master' or Branch = 'refs/heads/master' or Branch is NULL or Branch = 'not specified'        
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
                    Uri = _urlBuilder.Action("Get", typeof(OrganisationController), new { org = Organisation }),
                    Projects = _urlBuilder.Action("GetProjects", typeof(ProjectsController), new { org = Organisation }),
                    Coverage = _urlBuilder.Action("GetCoverage", typeof(OrganisationController), new { org = Organisation }),

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

        public IQueryable<ProjectSummary> GetProjects()
        {
            var org = getOrganisation(Organisation);
            if (org != null)
            {
                return getProjectsSummary(org);
            }
            else
            {
                return new List<ProjectSummary>().AsQueryable();
            }
        }

        private IQueryable<ProjectSummary> getProjectsSummary(TestHub.Data.DataModel.Organisation org)
        {
            var id = org.Id;

            var tr = getLast5TestRuns(org);

            var projects = _testHubDBContext.Query<ProjectExtended>(
                @"select p.Name, p.Id, count(t.Id) as TestRunsCount, max(t.TestCasesCount) as TestCasesCount, (sum(c.LinesCovered)/sum(c.LinesValid)) * 100 as coverage
                        from Projects p 
                      left join TestRuns t on t.ProjectId = p.id
                      left join Coverage c on c.TestRunId = t.Id
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
                           Uri = _urlBuilder.Action("Get", typeof(Controllers.ProjectsController), new { org = org.Name, project = projects[g.Key].Name }),
                           LatestResults = new LatestResults()
                           {
                               TestResults = g.Select(t => (Data.TestResult)t.Status).ToArray()
                           },
                           TestsCount = projects[g.Key].TestCasesCount,
                           TestQuantityGrowth = getQuantityGrowth(g),
                           CoverageGrowth = getCoverageGrowth(g),
                           TestRunFrequency = "occasionally",
                           Coverage = projects[g.Key].Coverage
                       });
            return groups.AsQueryable();
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
            // Last seven days
            var t = c.OrderByDescending(s => s.Timestamp)
                    .Where(s => s.Timestamp < DateTime.Now.AddDays(-7));
            if (t.Count() >= 2)
            {
                return t.First().TestCasesCount - t.Last().TestCasesCount;
            }else if (t.Count() == 1)
            {
                return t.First().TestCasesCount;
            }

            return 0;
        }

        private decimal? getCoverageGrowth(IEnumerable<TestRunExtended> c)
        {
            // last seven days
            var t = c.OrderByDescending(s => s.Timestamp)
                    .Where(s => s.Timestamp < DateTime.Now.AddDays(-7));
            if (c.Count() >= 2)
            {
                var coverageFirst = c.First()?.CoveragePercent;
                var coverageLast = c.Last()?.CoveragePercent;
                if (coverageFirst.HasValue && coverageLast.HasValue)
                {
                    return coverageFirst.Value - coverageLast.Value;
                }
                else if(coverageFirst.HasValue && coverageLast.HasValue)
                {
                    return coverageFirst.Value;
                }
            }

            return null;
        }

        public ProjectSummary GetProjectSummary(string projectName)
        {
            var projectSummary = GetProjects().FirstOrDefault(p => p.Name.Equals(projectName, StringComparison.OrdinalIgnoreCase));
            if (projectSummary == null)
            {
                TesthubApiException.ThrowProjectDoesNotExist(projectName);
            }
            return projectSummary;
        }

        public TestResultsHistoricalData GetTestResultsForOrganisation()
        {
            
            var data = _testHubDBContext.Query<TestResultsHistoricalItem>(@"SELECT DATE_FORMAT(r.Timestamp, '%Y-%m-%d') date, p.Id projectId,  r.TestRunName, r.Timestamp,  r.TestCasesCount as count
                                                              from TestRuns r
                                                              inner join Projects p on r.ProjectId = p.Id                                                              
                                                              WHERE p.OrganisationId = @orgId
                                                              GROUP BY  DATE_FORMAT(r.Timestamp, '%Y-%m-%d'), p.Id
                                                              ORDER BY r.Timestamp DESC",
                                                              new { orgId = this._organisation.Id });

            var dataConverted = convertToTestResultsDataItems(data);

            return new TestResultsHistoricalData
            {
                Data = dataConverted,
                Uri = _urlBuilder.Action(
                    "GetTestResults",
                    typeof(ProjectsController),
                    new
                    {
                        org = _organisation.Name,
                        project = project.Name
                    })
            };
        }

        public TestResultsHistoricalData GetTestResultsForProject(string projectName)
        {
            var project = getProjectIntity(projectName);

            var data = _testHubDBContext.Query<TestResultsHistoricalItem>(@"SELECT r.id,  r.TestRunName, r.Timestamp, tc.Status, COUNT(tc.Id) as count
                                                              from TestRuns r
                                                              left JOIN TestCases tc on tc.TestRunId = r.Id
                                                              WHERE ProjectId = @projId
                                                              GROUP by r.Id, tc.Status 
                                                              ORDER BY r.Timestamp DESC",
                                                              new { projId = project.Id });

            var dataConverted = convertToTestResultsDataItems(data);

            return new TestResultsHistoricalData
            {
                Data = dataConverted,
                Uri = _urlBuilder.Action(
                    "GetTestResults",
                    typeof(ProjectsController),
                    new
                    {
                        org = _organisation.Name,
                        project = project.Name
                    })
            };
        }

        private static IEnumerable<TestResultsDataItem> convertToTestResultsDataItems(IEnumerable<TestResultsHistoricalItem> data)
        {
            return data.GroupBy(g => g.Id).Select(g => new TestResultsDataItem
            {
                DateTime = g.First().Timestamp,
                Name = g.First().TestRunName,
                Passed = g.FirstOrDefault(s => s.Status == (int)Data.TestResult.Passed)?.Count ?? 0,
                Failed = g.FirstOrDefault(s => s.Status == (int)Data.TestResult.Failed)?.Count ?? 0,
                Skipped = g.FirstOrDefault(s => s.Status == (int)Data.TestResult.Skipped)?.Count ?? 0
            });
        }

        public CoverageHistoricalData GetCoverageHistory(string projectName)
        {
            var project = getProjectIntity(projectName);

            var coverage =  _testHubDBContext.Query<CoverageHistoricalItem>(@"select tr.TestRunName, tr.Timestamp, sum(c.LinesCovered)/sum(c.LinesValid)*100 as percent
                      from  TestRuns tr
                      left join Coverage c on c.TestRunId = tr.Id
                    where tr.ProjectId = @projectId   
                      group by tr.id
                      order by tr.Timestamp desc", new { projectId = project.Id });

            return new CoverageHistoricalData
            {
                Items = coverage.Select(c => new CoverageDataItem
                {
                    DateTime = c.Timestamp,
                    Coverage = c.Percent,
                    TestRunName = c.TestRunName,
                    Uri = _urlBuilder.Action("Get", typeof(TestRunsController), new { org = Organisation, project = project.Name, testRun = c.TestRunName })
                }),
                Uri = _urlBuilder.Action(
                    "GetTestResults",
                    typeof(ProjectsController),
                    new
                    {
                        org = _organisation.Name,
                        project = project.Name
                    })
            };
        }

    }

}