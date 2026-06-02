using AutoMapper;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlow.Application.UseCases.Expenses.Delete;
public class DeleteExpenseByIdUseCase : IDeleteExpenseByIdUseCase
{
    private readonly IExpensesReadOnlyRepository _readOnlyRepository;
    private readonly IExpensesWriteOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggedUser _loggedUser;
    public DeleteExpenseByIdUseCase( IExpensesWriteOnlyRepository repository, IUnitOfWork unitOfWork, ILoggedUser loggedUser, IExpensesReadOnlyRepository readOnlyRepository)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _loggedUser = loggedUser;
        _readOnlyRepository = readOnlyRepository;
    }

    public async Task Execute(long id)
    {
        var loggedUser = await _loggedUser.Get();
        var expense = await _readOnlyRepository.GetById(loggedUser, id);

        if (expense is null)
        {
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
        }

        await _repository.Delete(id);

        await _unitOfWork.Commit();
    }
}
