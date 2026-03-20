using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts;
using CashFlow.Communication.Reports;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;
using System.Reflection;

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

        CreateHeaderWithProfilePhotoAndName(page);

        var totalSpent = expenses.Sum(e => e.Amount);
        CreateTotalExpentSection(page, date, totalSpent);

        foreach(var expense in expenses)
        {
            var table = CreateExpenseTable(page);
        }


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

    private void CreateHeaderWithProfilePhotoAndName(Section page)
    {
        var table = page.AddTable();
        table.AddColumn();
        table.AddColumn("300");

        var row = table.AddRow();

        var assembly = Assembly.GetExecutingAssembly();
        var imagePath = Path.GetDirectoryName(assembly.Location);
        var image = Path.Combine(imagePath!, "Logo", "user.png");

        row.Cells[0].AddImage(image).Height = 62;

        row.Cells[1].AddParagraph("Olá, Matheus Oliveira").Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 16 };
        row.Cells[1].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center;

    }

    private void CreateTotalExpentSection(Section page, DateOnly date, decimal totalSpent)
    {
        var paragraph = page.AddParagraph();
        paragraph.Format.SpaceBefore = 40;
        paragraph.Format.SpaceAfter = 40;

        var title = string.Format(ResourceReportGenerationMessages.TOTAL_SPENT_IN, date.ToString("Y"));

        paragraph.AddFormattedText(title, new Font { Name = FontHelper.RALEWAY_REGULAR, Size = 15 });
        paragraph.AddLineBreak();

        paragraph.AddFormattedText($"{CURRENCY_SYMBOL} {totalSpent}", new Font { Name = FontHelper.WORKSANS_BLACK, Size = 50 });
    }

    private Table CreateExpenseTable(Section page)
    {
        var table = page.AddTable();
        table.AddColumn("195").Format.Alignment = ParagraphAlignment.Left;
        table.AddColumn("80").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Right;

        return table;
    }

}
