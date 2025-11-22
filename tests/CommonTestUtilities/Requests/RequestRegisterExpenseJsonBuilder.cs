
using Bogus;
using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestRegisterExpenseJsonBuilder
{
    public static RequestRegisterExpenseJson Build()
    {

        return new Faker<RequestRegisterExpenseJson>()
            .RuleFor(r => r.Title, f => f.Lorem.Sentence(3))
            .RuleFor(r => r.Description, f => f.Lorem.Paragraph())
            .RuleFor(r => r.Amount, f => f.Finance.Amount(min: 10, max: 1000))
            .RuleFor(r => r.Date, f => f.Date.Past(1))
            .RuleFor(r => r.PaymentType, f => f.PickRandom<PaymentType>())
            .Generate();
    }
}
