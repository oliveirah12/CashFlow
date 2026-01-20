using CashFlow.Domain.Repositories.Expenses;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlow.Application.UseCases.Expenses.Reports.Excel.GetByCustomDates;
public class GenerateExpensesReportExcelByCustomDatesUseCase : IGenerateExpensesReportExcelByCustomDatesUseCase
{
    private readonly IExpensesReadOnlyRepository _repository;

    public GenerateExpensesReportExcelByCustomDatesUseCase(IExpensesReadOnlyRepository repository)
    {
        _repository = repository;
    }

    public async Task<byte[]> Execute(DateOnly startDate, DateOnly endDate)
    {
        var expenses = await _repository.FilterByDates(startDate, endDate);
        if (expenses.Count == 0)
        {
            return [];
        }

        var workbook = new XLWorkbook();

        workbook.Author = "Matheus Oliveira";
        workbook.Style.Font.FontSize = 12;
        workbook.Style.Font.FontName = "Times New Roman";

        var file = new MemoryStream();

        workbook.SaveAs(file);

        return file.ToArray();

    }
}
