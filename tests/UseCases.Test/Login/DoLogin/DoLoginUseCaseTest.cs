using System;
using CashFlow.Application.UseCases.Login;
using CashFlow.Domain.Entities;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using Shouldly;

namespace UseCases.Test.Login;

public class DoLoginUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        //Arrange
        var user = UserBuilder.Build();
        var request = RequestDoLoginJsonBuilder.Build();
        request.Email = user.Email;

        var useCase = CreateUseCase(user, request.Password);

        //Act
        var result = await useCase.Execute(request);

        //Assert
        result.ShouldNotBeNull();
        result.Name.ShouldBe(user.Name);
        result.Token.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_user_not_Found()
    {
        //Arrange
        var user = UserBuilder.Build();
        var request = RequestDoLoginJsonBuilder.Build();
        var useCase = CreateUseCase(user, request.Password);

        //Act
        var act = async () => await useCase.Execute(request);

        //Assert
        var result = await act.ShouldThrowAsync<InvalidLoginException>();
        result.GetErrors().ShouldSatisfyAllConditions(
            e => e.Count.ShouldBe(1),
            e => e[0].ShouldBe(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID)
        );
        
    }

    [Fact]
    public async Task Error_password_Not_Match()
    {
        //Arrange
        var user = UserBuilder.Build();
        var request = RequestDoLoginJsonBuilder.Build();
        request.Email = user.Email;
        var useCase = CreateUseCase(user);

        //Act
        var act = async () => await useCase.Execute(request);

        //Assert
        var result = await act.ShouldThrowAsync<InvalidLoginException>();
        result.GetErrors().ShouldSatisfyAllConditions(
            e => e.Count.ShouldBe(1),
            e => e[0].ShouldBe(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID)
        );
    }

    private DoLoginUseCase CreateUseCase(User user, string? password = null)
    {
        var passwordEncrypter = new PasswordEncrypterBuilder().Verify(password).Build();
        var jwtTokenGenerator = JwtTokenGeneratorBuilder.Build();
        var userReadOnlyRepository = new UserReadOnlyRepositoryBuilder().GetUserByEmail(user).Build();
        
        return new DoLoginUseCase(
            userReadOnlyRepository, 
            passwordEncrypter, 
            jwtTokenGenerator
        );
    }
}
