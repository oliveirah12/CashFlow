namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf.GetByCustomDates;

public interface IGenerateExpensesReportPdfByCustomDatesUseCase
{
    Task<byte[]> Execute(DateOnly startDate, DateOnly endDate);
}
