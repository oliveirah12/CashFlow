using CashFlow.Application.UseCases.Expenses.Reports.Excel.GetByCustomDates;
using CashFlow.Application.UseCases.Expenses.Reports.Excel.GetByMonth;
using CashFlow.Communication.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace CashFlow.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ReportController : ControllerBase
{

    [HttpGet("excel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetExcel(
        [FromServices] IGenerateExpensesReportExcelUseCase useCase,
        [FromHeader] DateOnly month)
    {
        byte[] file = await useCase.Execute(month);

        if (file.Length > 0)
        {
            return File(file, MediaTypeNames.Application.Octet, "report.xlsx");
        }

        return NoContent();
    }

    [HttpGet("excelCustomDates")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetExcelCustomDates(
    [FromServices] IGenerateExpensesReportExcelByCustomDatesUseCase useCase,
    [FromHeader] DateOnly startDate,
    [FromHeader] DateOnly endDate)
    {
        byte[] file = await useCase.Execute(startDate, endDate);

        if (file.Length > 0)
        {
            return File(file, MediaTypeNames.Application.Octet, "reportByDate.xlsx");
        }

        return NoContent();
    }
}
