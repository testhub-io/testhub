using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestsHub.Data.DataModel;
using RandomNameGenerator;


namespace TestsHubUploadEndpoint
{
    public class DataLoader : IDataLoader
    {
        TestHubDBContext _testHubDBContext = new TestHubDBContext();

        public DataLoader(TestHubDBContext testHubDBContext, string projectName)
        {
            _testHubDBContext = testHubDBContext;
            ProjectName = projectName;
        }

        public string ProjectName { get; }

        public void Add(TestRun testRun)
        {
            _testHubDBContext.ChangeTracker.AutoDetectChangesEnabled = false;
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
            _testHubDBContext.TestRuns.Add(testRun);
            

            _testHubDBContext.SaveChanges();
        }

        public void Dispose()
        {
            _testHubDBContext.Dispose();
        }
    }
}
