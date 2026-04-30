using System;
using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using Shouldly;

namespace UseCases.Test.Users.Register;

public class RegisterUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        //Arrange
        var request = RequestRegisterUserJsonBuilder.Build();
        var useCase = CreateUseCase();

        //Act
        var result = await useCase.Execute(request);

        //Assert
        result.ShouldNotBeNull();
        result.Name.ShouldBe(request.Name);
        result.Token.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_Name_empty()
    {
        //Arrange
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;
        var useCase = CreateUseCase();

        //Act
        var act = async () => await useCase.Execute(request);

        //Assert
        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();
        result.GetErrors().ShouldSatisfyAllConditions(
            e => e.Count.ShouldBe(1),
            e => e[0].ShouldBe(ResourceErrorMessages.NAME_EMPTY)
        );
    }

    [Fact]
    public async Task Error_Email_Already_Exists()
    {
        //Arrange
        var request = RequestRegisterUserJsonBuilder.Build();
        var useCase = CreateUseCase(request.Email);

        //Act
        var act = async () => await  useCase.Execute(request);

        //Assert
        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();
        result.GetErrors().ShouldSatisfyAllConditions(
            e => e.Count.ShouldBe(1),
            e => e[0].ShouldBe(ResourceErrorMessages.EMAIL_ALREADY_REGISTERED)
        );
    }

    private RegisterUserUseCase CreateUseCase(string? email = null)
    {
        var mapper = MapperBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var userWriteOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();
        var passwordEncrypter = new PasswordEncrypterBuilder().Build();
        var jwtTokenGenerator = JwtTokenGeneratorBuilder.Build();
        var readOnlyRepository = new UserReadOnlyRepositoryBuilder();

        if(string.IsNullOrWhiteSpace(email) == false)
        {
            readOnlyRepository.ExistActiveUserWithEmail(email);
        }

        return new RegisterUserUseCase(
            mapper, 
            passwordEncrypter, 
            userWriteOnlyRepository, 
            readOnlyRepository.Build(), 
            unitOfWork, 
            jwtTokenGenerator
        );
    }

}
