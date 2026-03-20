using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts;
using CashFlow.Domain.Repositories.Expenses;
using PdfSharp.Fonts;

namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf.GetByCustomDates;

public class GenerateExpensesReportPdfByCustomDatesUseCase : IGenerateExpensesReportPdfByCustomDatesUseCase
{
    private readonly IExpensesReadOnlyRepository _respository;
    private const string CURRENCY_SYMBOL = "R$";
    public GenerateExpensesReportPdfByCustomDatesUseCase(IExpensesReadOnlyRepository repository)
    {
        _respository = repository;

        GlobalFontSettings.FontResolver = new ExpensesReportFontResolver();
    }
    public async Task<byte[]> Execute(DateOnly startDate, DateOnly endDate)
    {
        var expenses = await _respository.FilterByDates(startDate, endDate);
        if (expenses.Count == 0)
        {
            return [];
        }
        return [];

    }
}
