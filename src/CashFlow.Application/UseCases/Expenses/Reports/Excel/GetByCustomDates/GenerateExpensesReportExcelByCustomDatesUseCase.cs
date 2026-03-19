using CashFlow.Communication.Reports;
using CashFlow.Domain.Repositories.Expenses;
using ClosedXML.Excel;

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

        using var workbook = new XLWorkbook();

        workbook.Author = "Matheus Oliveira";
        workbook.Style.Font.FontSize = 12;
        workbook.Style.Font.FontName = "Times New Roman";

        var worksheet = workbook.Worksheets.Add($"{startDate.ToString().Replace('/', '-')} a {endDate.ToString().Replace('/', '-')}");

        InsertHeader(worksheet);

        foreach (var expense in expenses)
        {
            var currentRow = worksheet.LastRowUsed().RowNumber() + 1;
            worksheet.Cell($"A{currentRow}").Value = expense.Title;
            worksheet.Cell($"B{currentRow}").Value = expense.Date.ToString("dd/MM/yyyy");
            worksheet.Cell($"C{currentRow}").Value = Utils.ConvertPaymentType(expense.PaymentType);

            worksheet.Cell($"D{currentRow}").Value = expense.Amount;
            worksheet.Cell($"D{currentRow}").Style.NumberFormat.Format = Utils.FormatAmount(expense.Amount);

            worksheet.Cell($"E{currentRow}").Value = expense.Description;

        }

        worksheet.Columns().AdjustToContents();

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
