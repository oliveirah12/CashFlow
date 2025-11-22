
using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Communication.Enums;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using Shouldly;

namespace Validators.Test.Expenses.Register;

public class RegisterExpenseValidatorTests
{
    [Fact]
    public void Success()
    {
        //Arrange
        var validator = new RegisterExpenseValidator();
        var request = RequestRegisterExpenseJsonBuilder.Build();

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("              ")]
    public void Fail_When_Title_Is_Empty(string title)
    {
        //Arrange
        var validator = new RegisterExpenseValidator();
        var request = RequestRegisterExpenseJsonBuilder.Build();
        request.Title = title;

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(
            e => e.Count.ShouldBe(1),
            e => e[0].ErrorMessage.ShouldBe(ResourceErrorMessages.TITLE_REQUIRED)
        );
    }

    [Fact]
    public void Fail_When_Date_Is_In_The_Future()
    {
        //Arrange
        var validator = new RegisterExpenseValidator();
        var request = RequestRegisterExpenseJsonBuilder.Build();
        request.Date = DateTime.UtcNow.AddDays(1);

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(
            e => e.Count.ShouldBe(1),
            e => e[0].ErrorMessage.ShouldBe(ResourceErrorMessages.EXPENSES_CANNOT_FOR_THE_FUTURE)
        );
    }

    [Fact]
    public void Fail_When_Payment_Is_Invalid()
    {
        //Arrange
        var validator = new RegisterExpenseValidator();
        var request = RequestRegisterExpenseJsonBuilder.Build();
        request.PaymentType = (PaymentType)100;

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(
            e => e.Count.ShouldBe(1),
            e => e[0].ErrorMessage.ShouldBe(ResourceErrorMessages.PAYMENT_TYPE_INVALID)
        );
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-6)]
    public void Fail_When_Amount_Is_Invalid(decimal amount)
    {
        //Arrange
        var validator = new RegisterExpenseValidator();
        var request = RequestRegisterExpenseJsonBuilder.Build();
        request.Amount = amount;

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(
            e => e.Count.ShouldBe(1),
            e => e[0].ErrorMessage.ShouldBe(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_ZERO)
        );
    }
}
