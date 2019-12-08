using TestsHub.Data.DataModel;

namespace TestsHub.Data
{
    public interface IRepositoryFactory
    {
        ITestHubRepository GetTestHubRepository(string organisation);

    }
}