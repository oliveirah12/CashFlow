using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Colors;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf;
using CashFlow.Communication.Reports;
using CashFlow.Domain.Repositories.Expenses;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;
using System.Reflection;

namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf.GetByMonth;

public class GenerateExpensesReportPdfUseCase : IGenerateExpensesReportPdfUseCase
{
    private readonly IExpensesReadOnlyRepository _respository;
    public GenerateExpensesReportPdfUseCase(IExpensesReadOnlyRepository repository)
    {
        _respository = repository;

        GlobalFontSettings.FontResolver = new ExpensesReportFontResolver();
    }

    public async Task<byte[]> Execute(DateOnly date)
    {
        var startDate = new DateOnly(year: date.Year, month: date.Month, day: 1);
        var daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);

        var endDate = new DateOnly(year: date.Year, month: date.Month, day: daysInMonth);

        var expenses = await _respository.FilterByDates(startDate, endDate);
        if (expenses.Count == 0)
        {
            return [];
        }

        return PdfReportConstructor.StartCreateDocument(expenses, startDate, endDate);

    }



}
