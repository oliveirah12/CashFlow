using CashFlow.Application.UseCases.Expenses.GetById;
using CashFlow.Communication.Enums;
using CashFlow.Domain.Entities;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using Shouldly;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Expenses.GetById;

public class GetExpenseByIdTest : CashFlowClassFixture
{
    private const string METHOD = "api/Expenses";

    private readonly string _token;
    private readonly long _expenseId;

    public GetExpenseByIdTest(CustomWebApplicationFactory customWebApplicationFactory) : base(customWebApplicationFactory)
    {
        _token =  customWebApplicationFactory.User_Team_Member.GetToken();
        _expenseId = customWebApplicationFactory.Expense.GetId();
        
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet(requestUri: $"{METHOD}/{_expenseId}", token: _token);

        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        var body = await result.Content.ReadAsStreamAsync();

        var response =  await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("id").GetInt64().ShouldBe(_expenseId);
        response.RootElement.GetProperty("title").GetString().ShouldNotBeNullOrWhiteSpace();
        response.RootElement.GetProperty("description").GetString().ShouldNotBeNullOrWhiteSpace();
        response.RootElement.GetProperty("date").GetDateTime().ShouldBeLessThanOrEqualTo(DateTime.Today);
        response.RootElement.GetProperty("amount").GetDecimal().ShouldBeGreaterThan(0);

        var paymentType = response.RootElement.GetProperty("paymentType").GetInt32();
        Enum.IsDefined(typeof(PaymentType), paymentType).ShouldBeTrue() ;
    
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
