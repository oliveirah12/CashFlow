using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Requests;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace WebApi.Test.Users.Register;

public class RegisterUserTest : IClassFixture<CustomWebApplicationFactory>
{
    private const string METHOD = "api/User";
    private readonly HttpClient _httpClient;
    public RegisterUserTest(CustomWebApplicationFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.CreateClient();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var result = await _httpClient.PostAsJsonAsync(METHOD, request);

        result.StatusCode.ShouldBe(HttpStatusCode.Created);
        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("name").GetString().ShouldBe(request.Name);
        response.RootElement.GetProperty("token").GetString().ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task Error_Empty_Name()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var result = await _httpClient.PostAsJsonAsync(METHOD, request);

        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var erros = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        erros.ShouldSatisfyAllConditions(
            _ => erros.ShouldContain(e => e.GetString()!.Equals(ResourceErrorMessages.NAME_EMPTY)),
            _ => erros.Count().ShouldBe(1)
        );
    }

}
