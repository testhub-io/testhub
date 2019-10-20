namespace TestsHub.Data
{
    public interface IRepositoryFactory
    {
        ITestHubRepository GetTestHubRepository(string organisation);
    }
}