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
using CashFlow.Domain.Services.LoggedUser;

namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf.GetByMonth;

public class GenerateExpensesReportPdfUseCase : IGenerateExpensesReportPdfUseCase
{
    private readonly IExpensesReadOnlyRepository _respository;
    private readonly ILoggedUser _loggedUser;
    public GenerateExpensesReportPdfUseCase(IExpensesReadOnlyRepository repository, ILoggedUser loggedUser)
    {
        _respository = repository;
        _loggedUser = loggedUser;

        GlobalFontSettings.FontResolver = new ExpensesReportFontResolver();
    }

    public async Task<byte[]> Execute(DateOnly date)
    {
        var startDate = new DateOnly(year: date.Year, month: date.Month, day: 1);
        var daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);

        var endDate = new DateOnly(year: date.Year, month: date.Month, day: daysInMonth);

        var loggedUser = await _loggedUser.Get();
        var expenses = await _respository.FilterByDates(loggedUser, startDate, endDate);
        if (expenses.Count == 0)
        {
            return [];
        }

        return PdfReportConstructor.StartCreateDocument(expenses, startDate, endDate);

    }



}
