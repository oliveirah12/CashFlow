using DocumentFormat.OpenXml.Office2013.Word;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using System.Net;
using WebApi.Test.InlineData;

namespace WebApi.Test.Expenses.Delete;

public class DeleteExpenseTest : CashFlowClassFixture
{
    private const string METHOD = "api/Expenses";

    private readonly string _token;
    private readonly long _expenseId;

    public DeleteExpenseTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
        _expenseId = webApplicationFactory.Expense_MemberTeam.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoDelete(requestUri: $"{METHOD}/{_expenseId}", token: _token);
        result.StatusCode.ShouldBe(HttpStatusCode.NoContent);   

        var getResult = await DoGet(requestUri: $"{METHOD}/{_expenseId}", token: _token);   
        getResult.StatusCode.ShouldBe(HttpStatusCode.NotFound);

    }


    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Expense_Not_found(string culture)
    {
        var result = await DoDelete(requestUri: $"{METHOD}/1000", token: _token, culture: culture);

        result.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}
