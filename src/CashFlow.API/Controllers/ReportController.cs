using CashFlow.Application.UseCases.Expenses.Reports.Excel.GetByCustomDates;
using CashFlow.Application.UseCases.Expenses.Reports.Excel.GetByMonth;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf.GetByCustomDates;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf.GetByMonth;
using CashFlow.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace CashFlow.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = Roles.ADMIN)]
public class ReportController : ControllerBase
{

    [HttpGet("excelByMonth")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetExcel(
        [FromServices] IGenerateExpensesReportExcelUseCase useCase,
        [FromQuery] DateOnly month)
    {
        byte[] file = await useCase.Execute(month);

        if (file.Length > 0)
        {
            return File(file, MediaTypeNames.Application.Octet, "ReportByMonth.xlsx");
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
            return File(file, MediaTypeNames.Application.Octet, "ReportByDate.xlsx");
        }

        return NoContent();
    }

    [HttpGet("pdfByMonth")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetPdf(
        [FromServices] IGenerateExpensesReportPdfUseCase useCase,
        [FromHeader] DateOnly month)
    {
        byte[] file = await useCase.Execute(month);
        if (file.Length > 0)
        {
            return File(file, MediaTypeNames.Application.Pdf, "ReportByMonth.pdf");
        }
        return NoContent();
    }

    [HttpGet("pdfByCustomDates")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetPdfByCustomDates(
    [FromServices] IGenerateExpensesReportPdfByCustomDatesUseCase useCase,
    [FromHeader] DateOnly startDate,
    [FromHeader] DateOnly endDate)
    {
        byte[] file = await useCase.Execute(startDate, endDate);
        if (file.Length > 0)
        {
            return File(file, MediaTypeNames.Application.Pdf, "ReportByCustomDates.pdf");
        }
        return NoContent();
    }
}
