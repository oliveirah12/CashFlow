namespace CashFlow.Application.UseCases.Expenses.Delete;
public interface IDeleteExpenseByIdUseCase
{
    public Task Execute(long id);
}
