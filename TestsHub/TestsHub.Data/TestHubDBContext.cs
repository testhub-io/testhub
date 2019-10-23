using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace TestsHub.Data.DataModel
{
    public class TestHubDBContext : DbContext
    {
        private readonly string _connectionString;

        public TestHubDBContext (IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");            
        }

        public TestHubDBContext()
        {
            _connectionString = "Host=localhost;Database=testHub;Username=root;Password=test_pass";
        }

        public DbSet<TestCase> TestCases { get; set; }

        public DbSet<TestRun> TestRuns { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Organisation> Organisations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            optionsBuilder
               .UseMySQL(_connectionString)
               .UseLazyLoadingProxies();
        }
    }

}
