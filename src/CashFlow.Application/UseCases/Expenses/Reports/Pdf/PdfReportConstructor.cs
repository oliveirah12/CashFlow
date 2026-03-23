using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Colors;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts;
using CashFlow.Communication.Reports;
using CashFlow.Domain.Entities;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System.Reflection;
using CashFlow.Domain.Extensions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf;

public static class PdfReportConstructor
{
    private const string CURRENCY_SYMBOL = "R$";
    private const int LINE_HEIGHT_ROW_EXPENSE_TABLE = 25;


    public static byte[] StartCreateDocument(List<Expense> expenses, DateOnly startDate, DateOnly endDate)
    {
        var document = CreateDocument();
        var page = CreatePage(document);

        CreateHeaderWithProfilePhotoAndName(page);

        var totalSpent = expenses.Sum(e => e.Amount);
        CreateTotalExpentSection(page, startDate, endDate, totalSpent);

        foreach (var expense in expenses)
        {
            var table = CreateExpenseTable(page);

            var row = table.AddRow();
            row.Height = LINE_HEIGHT_ROW_EXPENSE_TABLE;

            AddExpenseTitle(row.Cells[0], expense.Title);

            AddHeaderForAmount(row.Cells[3]);

            /*segunda linha 1 coluna*/
            row = table.AddRow();
            row.Height = LINE_HEIGHT_ROW_EXPENSE_TABLE;

            row.Cells[0].AddParagraph(expense.Date.ToString("D"));
            SetStyleBaseForExpenseInformation(row.Cells[0]);
            row.Cells[0].Format.LeftIndent = 20;

            /*segunda linha 2 coluna*/
            row.Cells[1].AddParagraph(expense.Date.ToString("t"));
            SetStyleBaseForExpenseInformation(row.Cells[1]);
            row.Cells[1].Format.LeftIndent = 20;

            /*segunda linha 3 coluna*/
            row.Cells[2].AddParagraph(expense.PaymentType.ConvertPaymentTypeToString());
            SetStyleBaseForExpenseInformation(row.Cells[2]);
            row.Cells[2].VerticalAlignment = VerticalAlignment.Center;

            /*segunda linha 4 coluna*/
            AddAmountForExpense(row.Cells[3], expense.Amount);
            row.Cells[3].VerticalAlignment = VerticalAlignment.Center;

            /*Terceira Linha */
            row = table.AddRow();
            row.Height = LINE_HEIGHT_ROW_EXPENSE_TABLE;

            AddExpenseDescription(row.Cells[0], expense.Description!);

            AddWhiteSpace(table);


        }

        return RenderDocument(document);
    }

    public static string FormatAmount(decimal amount)
    {
        return $"-{CURRENCY_SYMBOL} {amount:#,##0.00}";
    }

    public static Document CreateDocument()
    {
        var document = new Document();
        document.Info.Title = $"{ResourceReportGenerationMessages.TITLE}";
        document.Info.Author = "Matheus Oliveira";

        var style = document.Styles["Normal"];
        style!.Font.Name = FontHelper.RALEWAY_REGULAR;



        return document;
    }

    public static void AddExpenseTitle(Cell cell, string title)
    {
        cell.AddParagraph(title);
        cell.Format.Font = new Font
        {
            Name = FontHelper.RALEWAY_BLACK,
            Size = 14,
            Color = ColorsHelper.BLACK
        };
        cell.Shading.Color = ColorsHelper.RED_LIGHT;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.MergeRight = 2;
        cell.Format.LeftIndent = 20;
    }

    public static void AddHeaderForAmount(Cell cell)
    {
        cell.AddParagraph(ResourceReportGenerationMessages.AMOUNT);
        cell.Format.Font = new Font 
        { 
            Name = FontHelper.RALEWAY_BLACK, 
            Size = 14, 
            Color = ColorsHelper.WHITE 
        };
        cell.Shading.Color = ColorsHelper.RED_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    public static void SetStyleBaseForExpenseInformation(Cell cell)
    {
        cell.Format.Font = new Font
        {
            Name = FontHelper.RALEWAY_REGULAR,
            Size = 12,
            Color = ColorsHelper.BLACK
        };
        cell.Shading.Color = ColorsHelper.GREEN_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    public static void AddAmountForExpense(Cell cell, decimal amount)
    {
        cell.AddParagraph($"-{CURRENCY_SYMBOL} {amount.ToString()}");
        cell.Format.Font = new Font
        {
            Name = FontHelper.RALEWAY_REGULAR,
            Size = 14,
            Color = ColorsHelper.BLACK
        };
        cell.Shading.Color = ColorsHelper.WHITE;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    public static void AddWhiteSpace(Table table)
    {
        var row = table.AddRow();
        row.Height = 30;
        row.Borders.Visible = false;
    }

    public static Section CreatePage(Document document)
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

    public static byte[] RenderDocument(Document document)
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

    public static void CreateHeaderWithProfilePhotoAndName(Section page)
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

    public static void CreateTotalExpentSection(Section page, DateOnly startDate, DateOnly endDate, decimal totalSpent)
    {
        var paragraph = page.AddParagraph();
        paragraph.Format.SpaceBefore = 40;
        paragraph.Format.SpaceAfter = 40;

        var title = string.Format(ResourceReportGenerationMessages.TOTAL_SPENT_BETWEEN, startDate.ToString("d"), endDate.ToString("d"));

        paragraph.AddFormattedText(title, new Font 
        { 
            Name = FontHelper.RALEWAY_REGULAR, 
            Size = 15 
        });
        paragraph.AddLineBreak();

        paragraph.AddFormattedText($"{CURRENCY_SYMBOL} {FormatAmount(totalSpent)}", 
            new Font 
            { 
                Name = FontHelper.WORKSANS_BLACK, 
                Size = 44 
            });
    }

    public static Table CreateExpenseTable(Section page)
    {
        var table = page.AddTable();
        table.AddColumn("195").Format.Alignment = ParagraphAlignment.Left;
        table.AddColumn("80").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Right;

        return table;
    }

    public static void AddExpenseDescription(Cell cell, string description)
    {
        if (string.IsNullOrEmpty(description)){
            return;
        }

        cell.AddParagraph(description);
        cell.Format.Font = new Font
        {
            Name = FontHelper.RALEWAY_REGULAR,
            Size = 10,
            Color = ColorsHelper.BLACK
        };
        cell.Shading.Color = ColorsHelper.WHITE;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.Format.LeftIndent = 20;
        cell.MergeRight = 3;

    }
}
