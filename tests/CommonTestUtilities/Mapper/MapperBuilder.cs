using AutoMapper;
using CashFlow.Application.AutoMapper;
using Microsoft.Extensions.Logging;

namespace CommonTestUtilities.Mapper;

public class MapperBuilder
{
    public static IMapper Build()
    {
        var loggerFactory = LoggerFactory.Create(builder => { });

        var mapper = new MapperConfiguration(config => 
        {
            config.AddProfile(new AutoMapping());
        }, loggerFactory);

        return mapper.CreateMapper();
    } 

}
