using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace TestsHub.Data.DataModel
{
    public class TestHubDBContext : DbContext
    {
        public TestHubDBContext (IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public DbSet<TestCase> TestCases { get; set; }

        public DbSet<TestRun> TestRuns { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Organisation> Organisations { get; set; }
        public IConfiguration Configuration { get; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {                       
            optionsBuilder
               .UseMySQL(Configuration.GetConnectionString("DefaultConnection"))
               .UseLazyLoadingProxies();
        }
    }

}
