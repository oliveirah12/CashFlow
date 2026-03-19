namespace CashFlow.Application.UseCases.Expenses.Reports.Excel.GetByCustomDates;
public interface IGenerateExpensesReportExcelByCustomDatesUseCase
{
    Task<byte[]> Execute(DateOnly startDate, DateOnly endDate);
}
