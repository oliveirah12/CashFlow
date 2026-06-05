using Shouldly;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Expenses.GetAll;

public class GetAllExpensesTest: CashFlowClassFixture
{
    private const string METHOD = "api/Expenses";
    private readonly string _token;

    public GetAllExpensesTest(CustomWebApplicationFactory webApplicationFactory): base(webApplicationFactory)
    {
        _token = webApplicationFactory.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet(
            requestUri: METHOD,
            token: _token
        );

        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var expenses = response.RootElement
            .GetProperty("expenses")
            .EnumerateArray()
            .ToList();

        expenses.ShouldNotBeNull();
        expenses.ShouldNotBeEmpty();

    }

}
