using AutoMapper;
using CashFlow.Application.AutoMapper;

namespace CommonTestUtilities.Mapper;

public class MapperBuilder
{
    public static IMapper Build()
    {
        var mapper = new MapperConfiguration(config => 
        {
            config.AddProfile(new AutoMapping());
        }, null);

        return mapper.CreateMapper();
    } 

}
