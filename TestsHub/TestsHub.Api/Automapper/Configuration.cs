using AutoMapper;
using TestsHub.Data.DataModel;

namespace TestsHub.Api.Automapper
{
    public class MapperConfiguration
    {
        static MapperConfiguration()
        {
            var configuration = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TestRun, Data.TestRun>(MemberList.Destination)
                .ForMember(m=>m.Name, c=>c.MapFrom(t=>t.TestRunName));                
                cfg.CreateMap<TestCase, Data.TestCase>(MemberList.Destination);              
            });

            Mapper = configuration.CreateMapper();
        }

        public static IMapper Mapper { get; private set; }
    }
}
