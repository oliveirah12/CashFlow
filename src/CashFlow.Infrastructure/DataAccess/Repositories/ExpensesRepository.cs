using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories;

internal class ExpensesRepository : IExpensesReadOnlyRepository, IExpensesWriteOnlyRepository, IExpensesUpdateOnlyRepository
{
    private readonly CashFlowDbContext _dbContext;
    public ExpensesRepository(CashFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(Expense expense)
    {
        await _dbContext.Expenses.AddAsync(expense);
    }

    public async Task Delete(long id)
    {
        var result = await _dbContext.Expenses.FindAsync(id);

        if (result != null)
        {
            _dbContext.Expenses.Remove(result);
        }
    }

    public async Task<List<Expense>> GetAll(User user)
    {
        return await _dbContext.Expenses.AsNoTracking().Where(e => e.UserId == user.Id).ToListAsync();
    }

    async Task<Expense?> IExpensesReadOnlyRepository.GetById(User user, long id)
    {
        return await _dbContext.Expenses.AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id && e.UserId == user.Id);
    }

    async Task<Expense?> IExpensesUpdateOnlyRepository.GetById(User user, long id)
    {
        return await _dbContext.Expenses
            .FirstOrDefaultAsync(e => e.Id == id && e.UserId == user.Id);
    }

    public void Update(Expense expense)
    {
        _dbContext.Expenses.Update(expense);
    }

    public async Task<List<Expense>> FilterByDates(User user,DateOnly date1, DateOnly date2)
    {
        var startDate = new DateTime(year: date1.Year, month: date1.Month, day: date1.Day).Date;
        var endDate = new DateTime(year: date2.Year, month: date2.Month, day: date2.Day, hour: 23, minute: 59, second: 59);

        return await _dbContext
            .Expenses
            .AsNoTracking()
            .Where(expense => expense.Date >= startDate && expense.Date <= endDate && expense.UserId == user.Id )
            .OrderBy(expense => expense.Date)
            .ThenBy(expense => expense.Title)
            .ToListAsync();
    }
}
