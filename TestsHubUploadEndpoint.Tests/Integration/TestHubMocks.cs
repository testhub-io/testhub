using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestsHubUploadEndpoint.Tests.Integration
{
    class TestHubMocks
    {
        public static Mock<IConfiguration> ConfigurationMock
        {
            get
            {
                var configurationMock = new Mock<IConfiguration>( MockBehavior.Strict);

                var configSection = new Mock<IConfigurationSection>(MockBehavior.Strict);
                configSection.SetupGet(c => c["DefaultConnection"])
                    .Returns("Host=localhost;Database=testHub;Username=root;Password=test_pass");

                configurationMock
                    .Setup(c => c.GetSection("ConnectionStrings"))
                    .Returns(configSection.Object);
                return configurationMock;
            }
        }
    }
}
