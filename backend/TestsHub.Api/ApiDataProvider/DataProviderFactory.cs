﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TestHub.Data.DataModel;

namespace TestHub.Api.ApiDataProvider
{
    public class DataProviderFactory : IDataProviderFactory
    {
        private readonly IConfiguration _configuration;

        public DataProviderFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDataProvider GetTestHubDataProvider(string organisation, IUrlHelper url)
        {            
            var context = new TestHubDBContext(_configuration);            
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return new DataProvider(context, organisation, new UrlBuilder(url), false);
        }

        public IDataProvider GetTestHubWritableDataProvider(string organisation, IUrlHelper url)
        {
            var context = new TestHubDBContext(_configuration);
            return new DataProvider(context, organisation, new UrlBuilder(url), true);
        }
    }
}
