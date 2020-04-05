using TestHub.Data.DataModel;

namespace TestHub.Api.ApiDataProvider
{
    public interface IDataProviderFactory
    {
        IDataProvider GetTestHubDataProvider(string organisation, Microsoft.AspNetCore.Mvc.IUrlHelper url);
        IDataProvider GetTestHubWritableDataProvider(string organisation, Microsoft.AspNetCore.Mvc.IUrlHelper url);
    }
}