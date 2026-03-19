using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts;
using CashFlow.Communication.Reports;
using CashFlow.Domain.Repositories.Expenses;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Fonts;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf.GetByMonth;

public class GenerateExpensesReportPdfUseCase : IGenerateExpensesReportPdfUseCase
{
    private readonly IExpensesReadOnlyRepository _respository;
    private const string CURRENCY_SYMBOL = "R$";
    public GenerateExpensesReportPdfUseCase(IExpensesReadOnlyRepository repository)
    {
        _respository = repository;

        GlobalFontSettings.FontResolver = new ExpensesReportFontResolver();
    }

    public async Task<byte[]> Execute(DateOnly date)
    {
        var startDate = new DateOnly(year: date.Year, month: date.Month, day: 1);
        var daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);

        var endDate = new DateOnly(year: date.Year, month: date.Month, day: daysInMonth);

        var expenses = await _respository.FilterByDates(startDate, endDate);
        if (expenses.Count == 0)
        {
            return [];
        }

        var document = CreateDocument(date);
        var page = CreatePage(document);

        var table = page.AddTable();
        table.AddColumn();
        table.AddColumn();

        var row = table.AddRow();
        row.Cells[0].AddImage(Path.Combine(
            AppContext.BaseDirectory,
            "UseCases",
            "Expenses",
            "Reports",
            "Pdf",
            "user.png"
        ));

        row.Cells[1].AddParagraph("Olá, Matheus Oliveira").Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 16};






        var paragraph = page.AddParagraph();
        var title = string.Format(ResourceReportGenerationMessages.TOTAL_SPENT_IN, date.ToString("Y"));

        paragraph.AddFormattedText(title, new Font { Name = FontHelper.RALEWAY_REGULAR, Size = 15 });
        paragraph.AddLineBreak();

        var totalSpent = expenses.Sum(e => e.Amount);
        paragraph.AddFormattedText($"{CURRENCY_SYMBOL} {totalSpent}", new Font { Name = FontHelper.WORKSANS_BLACK, Size = 50 });

        return RenderDocument(document);

    }

    private Document CreateDocument(DateOnly month)
    {
        var document = new Document();
        document.Info.Title = $"{ResourceReportGenerationMessages.TITLE} {month:Y}";
        document.Info.Author = "Matheus Oliveira";

        var style = document.Styles["Normal"];
        style!.Font.Name = FontHelper.RALEWAY_REGULAR;



        return document;
    }

    private Section CreatePage(Document document)
    {
        var section = document.AddSection();
        section.PageSetup = document.DefaultPageSetup.Clone();
        section.PageSetup.PageFormat = PageFormat.A4;

        section.PageSetup.LeftMargin = 40;
        section.PageSetup.RightMargin = 40;
        section.PageSetup.TopMargin = 80;
        section.PageSetup.BottomMargin = 80;

        return section;
    }

    private byte[] RenderDocument(Document document)
    {
        var renderer = new PdfDocumentRenderer
        {
            Document = document,

        };

        renderer.RenderDocument();
        using var file = new MemoryStream();
        renderer.PdfDocument.Save(file);
        return file.ToArray();
    }
}
