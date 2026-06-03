using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Requests;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Globalization;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users.Register;

public class RegisterUserTest : CashFlowClassFixture
{
    private const string METHOD = "api/User";

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

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Empty_Name(string cultureInfo)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(cultureInfo));

        var result = await _httpClient.PostAsJsonAsync(METHOD, request);

        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var erros = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(cultureInfo));

        erros.ShouldSatisfyAllConditions(
            _ => erros.Count().ShouldBe(1),
            _ => erros.ShouldContain(e => e.GetString()!.Equals(expectedMessage))
        );
    }

}
