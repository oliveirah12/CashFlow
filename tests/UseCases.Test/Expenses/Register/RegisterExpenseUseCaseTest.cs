using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using Shouldly;

namespace UseCases.Test.Expenses.Register;

public class RegisterExpenseUseCaseTest
{

    [Fact]
    public async Task Success()
    {
        // Arrange
        var loggedUser = UserBuilder.Build();
        var request = RequestRegisterExpenseJsonBuilder.Build();
        var useCase = CreateUseCase(loggedUser);

        // Act
        var result = await useCase.Execute(request);

        // Assert
        result.ShouldNotBeNull();
        result.Title.ShouldBe(request.Title);

    }

    [Fact]
    public async Task Error_Title_Empty()
    {
        // Arrange
        var loggedUser = UserBuilder.Build();
        var request = RequestRegisterExpenseJsonBuilder.Build();
        request.Title = string.Empty;

        var useCase = CreateUseCase(loggedUser);

        // Act
        var act = async () => await useCase.Execute(request);
        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();

        // Assert
        result.GetErrors().ShouldSatisfyAllConditions(
            (e) => e.Count.ShouldBe(1),
            (e) => e[0].ShouldBe(ResourceErrorMessages.TITLE_REQUIRED)
        );
        
    }

    private RegisterExpenseUseCase CreateUseCase(CashFlow.Domain.Entities.User user)
    {
        var repository = ExpenseWriteOnlyRepositoryBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var mapper = MapperBuilder.Build();
        var loggeduser = LoggedUserBuilder.Build(user);

        return new RegisterExpenseUseCase(repository, unitOfWork, mapper, loggeduser);

    }

}
