using CashFlow.Communication.Reports;
using CashFlow.Domain.Repositories.Expenses;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
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

        var worksheet = workbook.Worksheets.Add($"{startDate.ToString().Replace('/', '-')} a {endDate.ToString().Replace('/', '-')}");

        InsertHeader(worksheet);

        var file = new MemoryStream();

        workbook.SaveAs(file);

        return file.ToArray();

    }

    private void InsertHeader(IXLWorksheet worksheet)
    {
        worksheet.Cell("A1").Value = ResourceReportGenerationMessages.TITLE;
        worksheet.Cell("B1").Value = ResourceReportGenerationMessages.DATE;
        worksheet.Cell("C1").Value = ResourceReportGenerationMessages.PAYMENT_TYPE;
        worksheet.Cell("D1").Value = ResourceReportGenerationMessages.AMOUNT;
        worksheet.Cell("E1").Value = ResourceReportGenerationMessages.DESCRIPTION;

        worksheet.Cells("A1:E1").Style.Font.Bold = true;
        worksheet.Cells("A1:E1").Style.Fill.BackgroundColor = XLColor.FromHtml("#F5C2B6");

        worksheet.Cell("A1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("B1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("C1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
        worksheet.Cell("E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);


    }
}
