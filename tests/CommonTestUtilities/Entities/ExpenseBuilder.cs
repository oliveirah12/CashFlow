using Bogus;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;

namespace CommonTestUtilities.Entities;

public class ExpenseBuilder
{
    public static List<Expense> Collection(User user, uint count = 2)
    {
        var list = new List<Expense>();
        if (count == 0)
        {
            count = 1;
        }

        var expenseId = 1;

        for(int i = 0; i < count; i++)
        {
            var expense = Build(user);
            expense.Id = expenseId++;
            
            list.Add(expense);
        }

        return list;

    }

    public static Expense Build(User user)
    {
        var expense = new Faker<Expense>()
            .RuleFor(u => u.Id, _ => 1)
            .RuleFor(u => u.Title, f => f.Commerce.ProductName())
            .RuleFor(u => u.Description, f => f.Commerce.ProductDescription())
            .RuleFor(u => u.Date, f => f.Date.Past())
            .RuleFor(u => u.Amount, f => f.Random.Decimal(1, 1000))
            .RuleFor(u => u.PaymentType, f => f.PickRandom<PaymentType>())
            .RuleFor(u => u.UserId, _ => user.Id);

        return expense;
    }
}
