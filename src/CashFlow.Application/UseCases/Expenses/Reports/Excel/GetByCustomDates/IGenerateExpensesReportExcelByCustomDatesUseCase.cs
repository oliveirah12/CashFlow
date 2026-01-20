using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlow.Application.UseCases.Expenses.Reports.Excel.GetByCustomDates;
public interface IGenerateExpensesReportExcelByCustomDatesUseCase
{
    Task<byte[]> Execute(DateOnly startDate, DateOnly endDate);
}
