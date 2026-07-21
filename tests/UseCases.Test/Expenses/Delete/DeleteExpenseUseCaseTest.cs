using CashFlow.Application.UseCases.Expenses.Delete;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using Shouldly;

namespace WebApi.Test.Expenses.Delete;

public class DeleteExpenseUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggeduser = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(loggeduser);

        var useCase = CreateUseCase(loggeduser, expense);       

        var action = async () => await useCase.Execute(expense.Id);

        await action.ShouldNotThrowAsync();

    }

    [Fact]
    public async Task Error_Expense_Not_found()
    {
        var loggeduser = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(loggeduser);

        var useCase = CreateUseCase(loggeduser);

        var action = async () => await useCase.Execute(100000);

        var result = await action.ShouldThrowAsync<NotFoundException>();

        result.GetErrors().Count.ShouldBe(1);
        result.GetErrors().ShouldContain(ResourceErrorMessages.EXPENSE_NOT_FOUND);
    }



    private DeleteExpenseByIdUseCase CreateUseCase(User user, Expense? expense = null)
    {
        var repositoryWriteOnly = ExpenseWriteOnlyRepositoryBuilder.Build();
        var repository = new ExpensesReadOnlyRepositoryBuilder().GetById(user, expense).Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new DeleteExpenseByIdUseCase(repositoryWriteOnly, unitOfWork, loggedUser, repository);
    }
}
