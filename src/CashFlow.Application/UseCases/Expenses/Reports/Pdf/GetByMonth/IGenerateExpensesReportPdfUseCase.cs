namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf.GetByMonth;

public interface IGenerateExpensesReportPdfUseCase
{
    Task<byte[]> Execute(DateOnly month);
}
