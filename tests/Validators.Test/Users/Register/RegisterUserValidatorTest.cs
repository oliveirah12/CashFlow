using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Communication.Requests;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Requests;
using FluentValidation;
using Shouldly;

namespace Validators.Test.Users.Register;

public class RegisterUserValidatorTest
{
    [Fact]
    public void Success()
    {
        //Arrange
        var validator = new RegisterUserValidator();
        var request =  RequestRegisterUserJsonBuilder.Build();

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("              ")]
    public void Error_Name_Empty(string name)
    {
        //Arrange
        var validator = new RegisterUserValidator();
        var request =  RequestRegisterUserJsonBuilder.Build();
        request.Name = name;

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(
            e => e.Count.ShouldBe(1),
            e => e[0].ErrorMessage.ShouldBe(ResourceErrorMessages.NAME_EMPTY)
        );
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("              ")]
    public void Error_Email_Empty(string email)
    {
        //Arrange
        var validator = new RegisterUserValidator();
        var request =  RequestRegisterUserJsonBuilder.Build();
        request.Email = email;

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(
            e => e.Count.ShouldBe(1),
            e => e[0].ErrorMessage.ShouldBe(ResourceErrorMessages.EMAIL_EMPTY)
        );
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        //Arrange
        var validator = new RegisterUserValidator();
        var request =  RequestRegisterUserJsonBuilder.Build();
        request.Email = "invalid-email.com";

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(
            e => e.Count.ShouldBe(1),
            e => e[0].ErrorMessage.ShouldBe(ResourceErrorMessages.EMAIL_INVALID)
        );
    }

    [Fact]
    public void Error_Password_Empty()
    {
        //Arrange
        var validator = new RegisterUserValidator();
        var request =  RequestRegisterUserJsonBuilder.Build();
        request.Password = string.Empty;

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(
            e => e.Count.ShouldBe(1),
            e => e[0].ErrorMessage.ShouldBe(ResourceErrorMessages.PASSWORD_INVALID)
        );
    }
}
