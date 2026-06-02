namespace WebApi.Test.Expenses.Register;

public class RegisterExpenseTest : IClassFixture<CustomWebApplicationFactory>
{
    private const string METHOD = "api/Expenses";
    private readonly HttpClient _client;
    public RegisterExpenseTest(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }
}
