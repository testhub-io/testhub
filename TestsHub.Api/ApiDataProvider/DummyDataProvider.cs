using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestHub.Api.ApiDataProvider
{
    public class DummyDataProvider
    {
        public static Data.CoverageHistoricalData GetDummyCoverage()
        {
            var items = new List<Data.DataItem>();

            var r = new Random();
            for (var i = 0; i < 50; i++)
            {
                items.Add(new Data.DataItem() { DateTime = DateTime.Now.AddDays((50 - i) * (-1)), Coverage = Convert.ToDecimal(r.Next(30, 45 + i) / 100M ) });
            }

            return new Data.CoverageHistoricalData()
            {
                Items = items
            };
        }

        public  static ActionResult<IEnumerable<Data.ProjectSummary>> GetProjects(string org, UrlBuilder urlBuilder)
        {
            return new[]
            {
                new Data.ProjectSummary(){ Name = "Frontend", Coverage = 15, CoverageGrowth = 1,
                    LatestResults = new Data.LatestResults{
                    TestResults = new [] { Data.TestResult.Failed, Data.TestResult.Failed, Data.TestResult.Passed, Data.TestResult.Passed  }},
                    RecentTestRuntDate = DateTime.Now.AddDays(-1),
                    TestQuantityGrowth = 5, TestRunFrequenct = "Daily", TestRunsCount = 65, TestsCount = 120,
                    Uri = urlBuilder.Action("Get", "Projects", new { org = org, project = "Project1" })
                },

                 new Data.ProjectSummary(){ Name = "Backend", Coverage = 15, CoverageGrowth = 1,
                    LatestResults = new Data.LatestResults{
                    TestResults = new [] { Data.TestResult.Passed, Data.TestResult.Failed, Data.TestResult.Passed, Data.TestResult.Passed  }},
                    RecentTestRuntDate = DateTime.Now.AddDays(-5),
                    TestQuantityGrowth = 5, TestRunFrequenct = "Daily", TestRunsCount = 65, TestsCount = 120,
                    Uri = urlBuilder.Action("Get", "Projects", new { org = org, project = "Backend" })
                },

                 new Data.ProjectSummary(){ Name = "Data-processing",
                     Coverage = 80, CoverageGrowth = -1,
                    LatestResults = new Data.LatestResults{
                    TestResults = new [] { Data.TestResult.Passed, Data.TestResult.Skipped, Data.TestResult.Passed, Data.TestResult.Passed  }},
                    RecentTestRuntDate = DateTime.Now.AddDays(-10).AddHours(5),
                    TestQuantityGrowth = -5, TestRunFrequenct = "Daily", TestRunsCount = 10, TestsCount = 15,
                    Uri = urlBuilder.Action("Get", "Projects", new { org = org, project = "Data-processing" })
                }

            };
        }
    }
}
