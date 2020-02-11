using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestsHub.Data.DataModel;
using report = TestsHubUploadEndpoint.ReportModel;
using AutoMapper;
using TestsHubUploadEndpoint.CoverageModel;

namespace TestsHubUploadEndpoint
{
    
    public class DataLoader : IDataLoader
    {
        TestHubDBContext _testHubDBContext;
        IMapper _mapper;
        private TestRun testRunCache;

        public DataLoader(TestHubDBContext testHubDBContext, string projectName, string org)
        {
            _testHubDBContext = testHubDBContext;
            ProjectName = projectName;
            Organisation = testHubDBContext.Organisations.SingleOrDefault(s => s.Name == org);
            if (Organisation == null)
            {
                Organisation = new Organisation() { Name = org };
            }

            testHubDBContext.Entry<Organisation>(Organisation);

            InitMapper();
        }

        private void InitMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<report.TestCase, TestCase>();
                cfg.CreateMap<report.TestRun, TestRun>();
                cfg.CreateMap<report.TestSuite, TestSuite>();
                cfg.CreateMap<CoverageModel.CoverageSummary, Coverage>();
            });
            
            _mapper = new Mapper(config);     
        }

        public string ProjectName { get; }
        public Organisation Organisation { get; private set; }

        public void Add(TestRun testRun)
        {
            var project = _testHubDBContext.Projects.SingleOrDefault(p => p.Name == ProjectName);
            if (project == null && testRun.Project == null)
            {
                project = new Project()
                {
                    Name = ProjectName
                };
                _testHubDBContext.Projects.Add(project);
            }
            
            testRun.Project = project;
            testRun.Project.Organisation = Organisation;
            testRun.Time = testRun.TestCases.Sum(t => t.Time);
            var testCases = testRun.TestCases;

            var existingTestRun = _testHubDBContext.TestRuns.Where(t => t.ProjectId == project.Id
                && t.TestRunName.Equals(testRun.TestRunName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (existingTestRun == null)
            {
                testRun.TestCases = null;
                _testHubDBContext.TestRuns.Add(testRun);
                _testHubDBContext.SaveChanges();
                BatchInsert(testCases, testRun.Id);
            }
            else
            {                
                BatchInsert(testCases, existingTestRun.Id);
            }

            testRunCache = existingTestRun;
        }

        private void BatchInsert(ICollection<TestCase> testCases, int testRunId)
        {
            var batch = new List<TestCase>();
            var testCasesList = testCases.ToList();
            for (var i = 0; i < testCasesList.Count; i++)
            {
                testCasesList[i].TestRunId = testRunId;
                batch.Add(testCasesList[i]);
                if (i % 1000 == 0)
                {
                    _testHubDBContext.TestCases.AddRange(batch);
                    _testHubDBContext.SaveChanges();
                    batch = new List<TestCase>();
                }
            }

            _testHubDBContext.TestCases.AddRange(batch);
            _testHubDBContext.SaveChanges();
        }

        public void Dispose()
        {
            _testHubDBContext.Dispose();
        }

        public void Add(report.TestRun testRun, IEnumerable<report.TestCase> testCases)
        {
            var testRunDto = _mapper.Map<TestRun>(testRun);
            testRunDto.TestCases = _mapper.Map<ICollection<TestCase>>(testCases);
            Add(testRunDto);
        }

        public void Add(CoverageSummary coverageSummary)
        {
            if (testRunCache != null && 
                coverageSummary.TestRunName.Equals(testRunCache.TestRunName, StringComparison.OrdinalIgnoreCase))
            {
                var coverageDto = _mapper.Map<Coverage>(coverageSummary);
                coverageDto.TestRunId = testRunCache.Id;
                _testHubDBContext.Add(coverageDto);
                _testHubDBContext.SaveChanges();
            }
            else
            {
                throw new NotSupportedException("This branch is not supported");
            }           
        }
    }
}
