﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using TestHub.Data.DataModel;
using TestsHubUploadEndpoint.Coverage;
using report = TestsHubUploadEndpoint.ReportModel;

namespace TestsHubUploadEndpoint
{

    public class DataLoader : IDataLoader
    {
        TestHubDBContext _testHubDBContext;
        IMapper _mapper;
        private TestRun testRunCache;


        public DataLoader(TestHubDBContext testHubDBContext, string projectName, string org):this(testHubDBContext, projectName, org, null)
        {            
        }

        public DataLoader(TestHubDBContext testHubDBContext, string projectName, string org, string testRun)
        {
            _testHubDBContext = testHubDBContext;
            _testHubDBContext.ChangeTracker.AutoDetectChangesEnabled = false;

            ProjectName = projectName;            
            TestRun = testRun;
            Organisation = testHubDBContext.Organisations.SingleOrDefault(s => s.Name.Equals(org, StringComparison.OrdinalIgnoreCase));
            if (Organisation == null)
            {
                Organisation = new Organisation() { Name = org };
            }

            testHubDBContext.Entry(Organisation);

            _mapper = MapperFactory.CreateMapper();
        }

        public string ProjectName { get; }

        public Organisation Organisation { get; private set; }

        public string TestRun { get;  }

        public void Add(TestRun testRun)
        {
            var project = _testHubDBContext.Projects.SingleOrDefault(p => p.Name == ProjectName && p.OrganisationId == Organisation.Id);
            if (project == null && testRun.Project == null)
            {
                project = new Project()
                {
                    Name = ProjectName,
                    Organisation = Organisation,
                    OrganisationId = Organisation.Id
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
                existingTestRun = testRun;
            }
            else
            {
                existingTestRun.TestCasesCount = existingTestRun.TestCasesCount + testCases.Count;
                existingTestRun.Status = testRun.Status == TestResult.Failed ? TestResult.Failed : existingTestRun.Status;
                existingTestRun.Time = existingTestRun.Time + testRun.Time;
                _testHubDBContext.TestRuns.Update(existingTestRun);
                _testHubDBContext.SaveChanges();
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
            if (testRunCache == null)
            {
                testRunCache = (from tr in _testHubDBContext.TestRuns
                                join p in _testHubDBContext.Projects on tr.ProjectId equals p.Id
                                where p.Name == ProjectName && tr.TestRunName == TestRun 
                                select tr).FirstOrDefault();
            }

            if (testRunCache != null &&
                coverageSummary.TestRunName.Equals(testRunCache.TestRunName, StringComparison.OrdinalIgnoreCase))
            {
                var coverageDto = _mapper.Map<TestHub.Data.DataModel.Coverage>(coverageSummary);
                coverageDto.TestRunId = testRunCache.Id;

                var existingCoverage = _testHubDBContext.Coverage.FirstOrDefault(c => c.TestRunId == testRunCache.Id);
                if (existingCoverage == null) 
                {
                    _testHubDBContext.Add(coverageDto);
                    _testHubDBContext.SaveChanges();
                }
                else
                {
                    // TODO: Looks like this is not working
                    existingCoverage.LinesCovered += coverageDto.LinesCovered;
                    existingCoverage.LinesValid += coverageDto.LinesValid;
                    _testHubDBContext.Coverage.Update(existingCoverage);
                    _testHubDBContext.SaveChangesAsync();
                }
            }
            else
            {
                throw new NotSupportedException("This branch is not supported");
            }
        }
    }
}
