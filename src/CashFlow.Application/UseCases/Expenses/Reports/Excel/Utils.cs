using CashFlow.Communication.Reports;
using CashFlow.Domain.Enums;

namespace CashFlow.Application.UseCases.Expenses.Reports.Excel;

public static class Utils
{
    public static string ConvertPaymentType(PaymentType paymentType)
    {
        return paymentType switch
        {
            PaymentType.Cash => ResourceReportGenerationMessages.CASH,
            PaymentType.CreditCard => ResourceReportGenerationMessages.CREDIT_CARD,
            PaymentType.DebitCard => ResourceReportGenerationMessages.DEBIT_CARD,
            PaymentType.Pix => ResourceReportGenerationMessages.PIX,
            _ => string.Empty
        };
    }

    public const string CURRENCY_SYMBOL = "R$";

    public static string FormatAmount(decimal amount)
    {
        return $"-{CURRENCY_SYMBOL} {amount:#,##0.00}";
    }

}
