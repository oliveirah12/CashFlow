using System;
using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.tokens;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Login;

public class DoLoginUseCase : IDoLoginUseCase
{
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IPasswordEncrypter _passwordEncrypter;
    private readonly IAccessTokenGenerator _accessTokenGenerator;

    public DoLoginUseCase(
        IUserReadOnlyRepository userReadOnlyRepository, 
        IPasswordEncrypter passwordEncrypter, 
        IAccessTokenGenerator accessTokenGenerator
    )
    {
        _userReadOnlyRepository = userReadOnlyRepository;
        _passwordEncrypter = passwordEncrypter;
        _accessTokenGenerator = accessTokenGenerator;
    }


    public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
    {
        
        var user = await _userReadOnlyRepository.GetUserByEmail(request.Email);
        if (user is null)
        {
            throw new InvalidLoginException();
        }

        var passwordMatch = _passwordEncrypter.Verify(request.Password, user.Password);
        if (!passwordMatch)
        {
            throw new InvalidLoginException();
        }

        return new ResponseRegisteredUserJson
        {
            Name = user.Name,
            Token = _accessTokenGenerator.Generate(user)
        };

    }

}
