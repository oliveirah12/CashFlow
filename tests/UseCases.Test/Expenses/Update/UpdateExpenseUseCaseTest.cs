using CashFlow.Application.UseCases.Expenses.UpdateById;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace UseCases.Test.Expenses.Update;

public class UpdateExpenseUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        
    }

    [Fact]
    public async Task Error_Title_Empty()
    {

    }

    [Fact]
    public async Task Error_Expense_Not_Found()
    {

    }

    private UpdateExpenseByIdUseCase CreateUseCase(User user, Expense? expense = null)
    {
        var repository = new ExpensesUpdateOnlyRepositoryBuilder().GetById(user, expense).Build();
        var mapper = MapperBuilder.Build();
        var unitofWork = UnitOfWorkBuilder.Build();
        var loggeduser = LoggedUserBuilder.Build(user);

        return new UpdateExpenseByIdUseCase(mapper, unitofWork, repository, loggeduser);

    }
}
