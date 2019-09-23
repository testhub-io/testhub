using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TestsHub.Data.DataModel;

namespace TestsHub.Data.DataModel
{
    public class TestHubDBContext : DbContext
    {
        public DbSet<TestCase> TestCases { get; set; }

        public DbSet<TestRun> TestRuns { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=localhost;Database=testHub;Username=postgres;Password=911");
    }

}
