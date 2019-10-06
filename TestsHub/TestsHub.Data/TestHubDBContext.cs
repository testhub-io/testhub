using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace TestsHub.Data.DataModel
{
    public class TestHubDBContext : DbContext
    {

        public DbSet<TestCase> TestCases { get; set; }

        public DbSet<TestRun> TestRuns { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Organisation> Organisations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                       .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                       .AddJsonFile("appsettings.json")
                       .Build();
            optionsBuilder
               .UseMySQL(configuration.GetConnectionString("DefaultConnection"))
               .UseLazyLoadingProxies();
        }
    }

}
