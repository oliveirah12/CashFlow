using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;
using FluentValidation;

namespace CashFlow.Application.UseCases.Expenses.Register;
public class RegisterExpenseValidator : AbstractValidator<RequestRegisterExpenseJson>
{
    public RegisterExpenseValidator()
    {
        RuleFor(expense => expense.Title).NotEmpty().WithMessage("Title Required");
        RuleFor(expense => expense.Amount).GreaterThan(0).WithMessage("Amount must be greater than zero");
        RuleFor(expense => expense.Date).LessThan(DateTime.UtcNow).WithMessage("Date cannot be in the future");
        RuleFor(expense => expense.PaymentType).IsInEnum().WithMessage("Invalid payment type");
    }
}
