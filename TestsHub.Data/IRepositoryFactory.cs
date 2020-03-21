using TestHub.Data.DataModel;

namespace TestHub.Data
{
    public interface IRepositoryFactory
    {
        ITestHubRepository GetTestHubRepository(string organisation);

    }
}