
using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using System;

namespace CashFlow.Application.UseCases.Expenses.Register;

public class RegisterExpenseUseCase
{
    public ResponseRegisterExpenseJson Execute(RequestRegisterExpenseJson request)
    {
        //TODO : Implement the use case logic to register an expense
        Validate(request);
        
        return new ResponseRegisterExpenseJson();
    }

    private void Validate(RequestRegisterExpenseJson request)
    {
        var titleIsEmprty = string.IsNullOrWhiteSpace(request.Title);
        if(titleIsEmprty)
        {
            throw new ArgumentException("Title required");
        }

        if(request.Amount <= 0)
        {
            throw new ArgumentException("Amount must be greater than zero");
        }

        var result = DateTime.Compare(request.Date, DateTime.UtcNow);
        if(result > 0)
        {
            throw new ArgumentException("Date cannot be in the future");
        }

        var paymentTypeIsValid = Enum.IsDefined(typeof(PaymentType), request.PaymentType);
        if(!paymentTypeIsValid)
        {
            throw new ArgumentException("Invalid payment type");
        }
    }
}
