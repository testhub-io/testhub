using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHub.Api.Data;

namespace TestHub.Api.ApiDataProvider
{
    public class DummyDataProvider
    {
        public static IEnumerable<CoverageDataItem> GetDummyCoverage()
        {
            var items = new List<Data.CoverageDataItem>();

            var r = new Random();
            for (var i = 0; i < 50; i++)
            {
                items.Add(new Data.CoverageDataItem() { DateTime = DateTime.Now.AddDays((50 - i) * (-1)), Coverage = Convert.ToDecimal(r.Next(30, 45 + i) / 100M ) });
            }

            return items;
        }

        public  static ActionResult<IEnumerable<Data.ProjectSummary>> GetProjects(string org, UrlBuilder urlBuilder)
        {
            return new[]
            {
                new Data.ProjectSummary(){ Name = "Frontend", Coverage = 15, CoverageGrowth = 1,
                    LatestResults = new Data.LatestResults{
                    TestResults = new [] { Data.TestResult.Failed, Data.TestResult.Failed, Data.TestResult.Passed, Data.TestResult.Passed  }},
                    RecentTestRuntDate = DateTime.Now.AddDays(-1),
                    TestQuantityGrowth = 5, TestRunFrequency = "Daily", TestRunsCount = 65, TestsCount = 120,
                    Uri = urlBuilder.Action("Get", typeof(Controllers.ProjectsController), new { org = org, project = "Project1" })
                },

                 new Data.ProjectSummary(){ Name = "Backend", Coverage = 15, CoverageGrowth = 1,
                    LatestResults = new Data.LatestResults{
                    TestResults = new [] { Data.TestResult.Passed, Data.TestResult.Failed, Data.TestResult.Passed, Data.TestResult.Passed  }},
                    RecentTestRuntDate = DateTime.Now.AddDays(-5),
                    TestQuantityGrowth = 5, TestRunFrequency = "Daily", TestRunsCount = 65, TestsCount = 120,
                    Uri = urlBuilder.Action("Get", typeof(Controllers.ProjectsController), new { org = org, project = "Backend" })
                },

                 new Data.ProjectSummary(){ Name = "Data-processing",
                     Coverage = 80, CoverageGrowth = -1,
                    LatestResults = new Data.LatestResults{
                    TestResults = new [] { Data.TestResult.Passed, Data.TestResult.Skipped, Data.TestResult.Passed, Data.TestResult.Passed  }},
                    RecentTestRuntDate = DateTime.Now.AddDays(-10).AddHours(5),
                    TestQuantityGrowth = -5, TestRunFrequency = "Daily", TestRunsCount = 10, TestsCount = 15,
                    Uri = urlBuilder.Action("Get", typeof(Controllers.ProjectsController), new { org = org, project = "Data-processing" })
                }

            };
        }

        internal static ProjectSummary GetDummyProjectSummary(string org, string project, UrlBuilder urlBuilder)
        {
            return new ProjectSummary()
            {
                Coverage = 80,
                CoverageGrowth = 5,
                LatestResults = new LatestResults { TestResults = new Data.TestResult[] { Data.TestResult.Failed, Data.TestResult.Passed, Data.TestResult.Passed, Data.TestResult.Skipped } },
                Name = "DummyData" + project,
                RecentTestRuntDate = DateTime.Now.AddDays(-1),
                TestQuantityGrowth = 15,
                TestRunFrequency = "daily",
                TestRunsCount = 10,
                TestsCount = 150,
                Uri = urlBuilder.Action("Get", typeof(Controllers.ProjectsController), new { org = org, project = "Project1" })

            };
        }
    }
}
