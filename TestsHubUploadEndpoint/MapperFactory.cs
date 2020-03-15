using AutoMapper;
using TestsHub.Data.DataModel;
using report = TestsHubUploadEndpoint.ReportModel;

namespace TestsHubUploadEndpoint
{
    public class MapperFactory
    {
        class StatusConverter : IValueConverter<string, TestResult>
        {
            public TestResult Convert(string sourceMember, ResolutionContext context)
            {
                var lowerStat = sourceMember.ToLower();
                if (lowerStat.Contains("pass"))
                {
                    return TestResult.Passed;
                }
                else if (lowerStat.Contains("fail"))
                {
                    return TestResult.Failed;
                }
                else if (lowerStat.Contains("skip"))
                {
                    return TestResult.Skipped;
                }

                throw new DataLoadException($"Unknown test status: {sourceMember}");
            }
        }

        public static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<report.TestCase, TestCase>()
                    .ForMember(r => r.Status, m => m.ConvertUsing<StatusConverter, string>());
                cfg.CreateMap<report.TestRun, TestRun>()
                    .ForMember(r => r.Status, m => m.ConvertUsing<StatusConverter, string>());

                cfg.CreateMap<report.TestSuite, TestSuite>();
                cfg.CreateMap<CoverageModel.CoverageSummary, Coverage>();
            });

            return new Mapper(config);
        }
    }
}
