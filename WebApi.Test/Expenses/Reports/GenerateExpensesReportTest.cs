using Shouldly;
using System.Net.Mime;

namespace WebApi.Test.Expenses.Reports;

public class GenerateExpensesReportTest : CashFlowClassFixture
{
    private const string METHOD = "api/Report";

    private readonly string _adminToken;
    private readonly string _teamMemberToken;
    private readonly DateTime _expenseDate;

    public GenerateExpensesReportTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _adminToken = webApplicationFactory.User_Admin.GetToken();
        _teamMemberToken = webApplicationFactory.User_Team_Member.GetToken();
        _expenseDate = webApplicationFactory.Expense_Admin.GetDate();
    }

    [Fact]
    public async Task Success_Pdf_By_Month()
    {
        var result = await DoGet($"{METHOD}/pdfByMonth?month={_expenseDate:Y}", token: _adminToken);

        result.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

        result.Content.Headers.ContentType.ShouldNotBeNull();
        result.Content.Headers.ContentType.MediaType.ShouldBe(MediaTypeNames.Application.Pdf);
    }

}
