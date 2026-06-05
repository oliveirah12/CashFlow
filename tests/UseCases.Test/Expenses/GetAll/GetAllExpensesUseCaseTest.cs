using CashFlow.Application.UseCases.Expenses.GetAll;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using Shouldly;

namespace WebApi.Test.Expenses.GetAll;

public class GetAllExpensesUseCaseTest
{

    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var expenses = ExpenseBuilder.Collection(loggedUser);

        var useCase = CreateUseCase(loggedUser, expenses);

        var result = await useCase.Execute();

        result.ShouldNotBeNull();
        result.Expenses.ShouldNotBeNull().ShouldNotBeEmpty();

        foreach (var expense in result.Expenses)
        {
            expense.Id.ShouldBeGreaterThan(0);
            expense.Title.ShouldNotBeNullOrWhiteSpace();
            expense.Amount.ShouldBeGreaterThan(0);
        }

    } 

    public GetAllExpensesUseCase CreateUseCase(User user, List<Expense> expenses)
    {
        var repository = new ExpensesReadOnlyRepositoryBuilder().GetAll(user, expenses).Build();
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new GetAllExpensesUseCase(repository, mapper, loggedUser);
    }

}
