﻿using Microsoft.Extensions.Configuration;
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
                var configurationMock = new Mock<IConfiguration>();
                configurationMock
                    .Setup(c => c.GetConnectionString("DefaultConnection"))
                    .Returns("Host=localhost;Database=testHub;Username=root;Password=test_pass");
                return configurationMock;
            }
        }
    }
}
