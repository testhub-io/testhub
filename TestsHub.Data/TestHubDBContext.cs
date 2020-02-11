using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using TestsHub.Commons;

namespace TestsHub.Data.DataModel
{
    public class TestHubDBContext : DbContext
    {
        private readonly string _connectionString;
        private readonly ILogger _logger = TestHubLogger.CreateLoger<TestHubDBContext>();

        public TestHubDBContext (IConfiguration configuration)
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {            
            optionsBuilder
               .UseMySQL(_connectionString)
               .UseLazyLoadingProxies();
        }
    }

}
