using System;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.tokens;
using Moq;

namespace CommonTestUtilities.Repositories;

public class JwtTokenGeneratorBuilder
{
    public static IAccessTokenGenerator Build()
    {
        var mock = new Mock<IAccessTokenGenerator>();
        
        mock.Setup(accessTokenGenerator => 
            accessTokenGenerator.Generate(It.IsAny<User>())).Returns("token");
        
        return mock.Object;
    }
}
