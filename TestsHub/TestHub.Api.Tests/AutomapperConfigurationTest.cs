using AutoMapper;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TestsHub.Api.Automapper;
using TestsHub.Api.Data;
using Dto = TestsHub.Data.DataModel;

namespace TestHub.Api.Tests
{
    [TestFixture]
    public class AutomapperConfigurationTest
    {

        IMapper _mapper = TestsHub.Api.Automapper.MapperConfiguration.Mapper;
        
        [TestCase]
        public void TestCaseMappingTest()
        {
            var testRunDto = new Dto.TestRun()
            {
                TestCases = new List<Dto.TestCase>()
                            {
                                new Dto.TestCase()
                            }
            };
            var testRunApi = _mapper.Map<TestRun>(testRunDto);

            Assert.AreEqual(1, testRunApi.TestCases.Count);            
        }
    }
}
