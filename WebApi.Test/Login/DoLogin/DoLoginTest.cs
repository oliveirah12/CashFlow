using CommonTestUtilities.Requests;
using Shouldly;
using System.Net.Http.Json;
using System.Net;
using System.Text.Json;
using CashFlow.Communication.Requests;


namespace WebApi.Test.Login.DoLogin;

public class DoLoginTest :  IClassFixture<CustomWebApplicationFactory>
{
    private const string METHOD = "api/Login";
    private readonly HttpClient _httpClient;
    private readonly string _email;

    public DoLoginTest(CustomWebApplicationFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.CreateClient();
        _email = webApplicationFactory.GetEmail();
    }

    [Fact]
    public async Task Success()
    {
        //Arrange
        var request = new RequestLoginJson
        {
            Email = "",
            Password = "",
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

}
