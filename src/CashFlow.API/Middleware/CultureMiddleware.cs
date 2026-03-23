using System.Globalization;

namespace CashFlow.API.Middleware;

public class CultureMiddleware
{
    private readonly RequestDelegate _next;

    public CultureMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var supportedLanguages = CultureInfo.GetCultures(CultureTypes.AllCultures).ToList();

        var header = context.Request.Headers.AcceptLanguage.FirstOrDefault();

        var requestedCulture = header?
            .Split(',')
            .FirstOrDefault()?
            .Split(';')
            .FirstOrDefault()
            ?.Trim();


        var cultureInfo = new CultureInfo("en");

        if(!string.IsNullOrWhiteSpace(requestedCulture)
            && supportedLanguages.Exists(language => language.Name.Equals(requestedCulture)))
        {
            cultureInfo = new CultureInfo(requestedCulture);
        }

        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;

        await _next(context);
    }
}
