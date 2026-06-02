using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using PdfSharp.Fonts;

namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf.GetByCustomDates;

public class GenerateExpensesReportPdfByCustomDatesUseCase : IGenerateExpensesReportPdfByCustomDatesUseCase
{
    private readonly IExpensesReadOnlyRepository _respository;
    private readonly ILoggedUser _loggedUser;
    public GenerateExpensesReportPdfByCustomDatesUseCase(IExpensesReadOnlyRepository repository, ILoggedUser loggedUser)
    {
        _respository = repository;
        _loggedUser = loggedUser;

        GlobalFontSettings.FontResolver = new ExpensesReportFontResolver();
    }
    public async Task<byte[]> Execute(DateOnly startDate, DateOnly endDate)
    {
        var loggedUser = await _loggedUser.Get();
        var expenses = await _respository.FilterByDates(loggedUser, startDate, endDate);
        if (expenses.Count == 0)
        {
            return [];
        }
        return PdfReportConstructor.StartCreateDocument(expenses, startDate, endDate);

    }
}
