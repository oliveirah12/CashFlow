using CashFlow.Application.UseCases.Expenses.GetById;
using CashFlow.Communication.Enums;
using CashFlow.Domain.Entities;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using Shouldly;

namespace WebApi.Test.Expenses.GetById;

public class GetExpenseByIdTest
{
    private const string METHOD = "api/Expenses";


    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(user: loggedUser);

        var useCase = CreateUseCase(loggedUser, expense);

        var result = await useCase.Execute(expense.Id);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(expense.Id);
        result.Title.ShouldBe(expense.Title);
        result.Description.ShouldBe(expense.Description);
        result.Date.ShouldBe(expense.Date);
        result.Amount.ShouldBe(expense.Amount);
        result.PaymentType.ShouldBe((PaymentType)expense.PaymentType);
    }

    [Fact]
    public async Task Error_Expense_Not_Found()
    {
        var loggedUser = UserBuilder.Build();

        var useCase = CreateUseCase(loggedUser);

        var act = async () => await useCase.Execute(id: 1000);

        var result = await act.ShouldThrowAsync<NotFoundException>();

        result.GetErrors().ShouldSatisfyAllConditions(
            () => result.GetErrors().Count().ShouldBe(1),
            () => result.GetErrors()[0].ShouldBe(ResourceErrorMessages.EXPENSE_NOT_FOUND)
        );

    }


    private GetExpenseByIdUseCase CreateUseCase(User user, Expense? expense = null)
    {
        var repository = new ExpensesReadOnlyRepositoryBuilder().GetById(user, expense).Build();
        var mapper = MapperBuilder.Build();
        var loggeduser = LoggedUserBuilder.Build(user);

        return new GetExpenseByIdUseCase(repository, mapper, loggeduser);

    }
}
