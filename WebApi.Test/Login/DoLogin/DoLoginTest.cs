using CommonTestUtilities.Requests;
using Shouldly;
using System.Net.Http.Json;
using System.Net;
using System.Text.Json;
using CashFlow.Communication.Requests;
using WebApi.Test.InlineData;
using System.Net.Http.Headers;
using CashFlow.Exception.ExceptionsBase;


namespace WebApi.Test.Login.DoLogin;

public class DoLoginTest :  IClassFixture<CustomWebApplicationFactory>
{
    private const string METHOD = "api/Login";
    private readonly HttpClient _httpClient;
    private readonly string _email;
    private readonly string _name;
    private readonly string _password;

    public DoLoginTest(CustomWebApplicationFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.CreateClient();
        _email = webApplicationFactory.GetEmail();
        _name = webApplicationFactory.GetName();
        _password = webApplicationFactory.GetPassword();
    }

    [Fact]
    public async Task Success()
    {
        //Arrange
        var request = new RequestLoginJson
        {
            Email = _email,
            Password = _password,
        };

        //Act
        var result = await _httpClient.PostAsJsonAsync(METHOD, request);
        var response = await result.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(response);

        //Assert
        result.StatusCode.ShouldBe(HttpStatusCode.OK);
        responseData.RootElement.GetProperty("name").GetString().ShouldBe(_name);
        responseData.RootElement.GetProperty("token").GetString().ShouldNotBeNullOrEmpty();

    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Login_Invalid(string culture)
    {
        //Arrange
        var request = RequestLoginJsonBuilder.Build();
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(culture));

        //Act
        var response = await _httpClient.PostAsJsonAsync(METHOD, request);
        var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);
        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EMAIL_OR_PASSWORD_INVALID", new System.Globalization.CultureInfo(culture));

        //Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        errors.ShouldSatisfyAllConditions(
            () => errors.Count().ShouldBe(1),
            () => errors.ShouldContain(error => error.GetString() == expectedMessage)
        );
    }

}