using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using TestHub.Commons;

namespace TestHub.Data.DataModel
{
    public class TestHubDBContext : DbContext
    {
        private readonly string _connectionString;
        private readonly ILogger _logger = TestHubLogger.CreateLoger<TestHubDBContext>();

        public static readonly LoggerFactory _myLoggerFactory =
                new LoggerFactory(new[] {
                    new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider()
                });


        public TestHubDBContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger.LogInformation("Using connection string:" + _connectionString);
        }

        public TestHubDBContext()
        {
            _connectionString = "Host=localhost;Database=testHub;Username=root;Password=test_pass";
        }

        public DbSet<TestCase> TestCases { get; set; }

        public DbSet<TestRun> TestRuns { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Organisation> Organisations { get; set; }

        public DbSet<Coverage> Coverage { get; set; }

        // ReadOnly query
        public IEnumerable<T> Query<T>(string sql, object param)
        {
            return this.Database.GetDbConnection().Query<T>(sql, param);
        }




        public IQueryable<TestCase> OrganisationTestCases(string org)
        {
            return from t in this.TestCases
                   join r in this.TestRuns on t.TestRunId equals r.Id
                   join p in this.Projects on r.ProjectId equals p.Id
                   join o in this.Organisations on p.OrganisationId equals o.Id
                   where o.Name == org
                   select t;
        }

        public IQueryable<TestRun> OrganisationTestRun(string org)
        {
            return from t in this.TestRuns
                   join p in this.Projects on t.ProjectId equals p.Id
                   join o in this.Organisations on p.OrganisationId equals o.Id
                   where o.Name == org
                   select t;
        }

        public IQueryable<Project> OrganisationProjects(string org)
        {
            return from p in this.Projects
                   join o in this.Organisations on p.OrganisationId equals o.Id
                   where o.Name == org
                   select p;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
               .UseMySql(_connectionString)
               .UseLazyLoadingProxies()
               .UseLoggerFactory(_myLoggerFactory);
        }
    }

}
