using Microsoft.EntityFrameworkCore;

namespace TestsHub.Data.DataModel
{
    public class TestHubDBContext : DbContext
    {
        public DbSet<TestCase> TestCases { get; set; }

        public DbSet<TestRun> TestRuns { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Organisation> Organisations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseMySQL("Host=localhost;Database=testHub;Username=root;Password=test_pass")
                .UseLazyLoadingProxies();
    }

}
