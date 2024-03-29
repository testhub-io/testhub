﻿using System;

namespace TestHub.Api.Data
{
    public class ProjectSummary : DataObjectBase
    {
        public string Name { get; set; }

        public int TestsCount { get; set; }

        public decimal Coverage { get; set; }

        public string TestRunFrequency { get; set; }

        public decimal TestQuantityGrowth { get; set; }

        public decimal? CoverageGrowth { get; set; }

        public LatestResults LatestResults { get; set; }

        public DateTime RecentTestRuntDate { get; set; }

        public int TestRunsCount { get; set; }

    }
}
