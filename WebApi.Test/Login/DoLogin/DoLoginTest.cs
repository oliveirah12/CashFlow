using CashFlow.Communication.Requests;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Requests;
using Shouldly;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;


namespace WebApi.Test.Login.DoLogin;

public class DoLoginTest :  CashFlowClassFixture
{
    private const string METHOD = "api/Login";
    private readonly string _email;
    private readonly string _name;
    private readonly string _password;

    public DoLoginTest(CustomWebApplicationFactory webApplicationFactory): base(webApplicationFactory)
    {
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
        var result = await DoPost(METHOD, request);
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

        //Act
        var response = await DoPost(
            requestUri: METHOD,
            request: request,
            culture: culture
        );
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