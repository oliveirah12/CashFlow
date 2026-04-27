using CashFlow.Application.AutoMapper;
using CashFlow.Application.UseCases.Expenses.Delete;
using CashFlow.Application.UseCases.Expenses.GetAll;
using CashFlow.Application.UseCases.Expenses.GetById;
using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Application.UseCases.Expenses.Reports.Excel.GetByCustomDates;
using CashFlow.Application.UseCases.Expenses.Reports.Excel.GetByMonth;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf.GetByCustomDates;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf.GetByMonth;
using CashFlow.Application.UseCases.Expenses.UpdateById;
using CashFlow.Application.UseCases.Login;
using CashFlow.Application.UseCases.Users.Register;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        AddAutoMapper(services);    
        AddUseCases(services);

    }

    private static void AddAutoMapper(IServiceCollection services)
    {
        services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapping>());
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterExpenseUseCase, RegisterExpenseUseCase>();
        services.AddScoped<IGetAllExpensesUseCase, GetAllExpensesUseCase>();
        services.AddScoped<IGetExpenseByIdUseCase, GetExpenseByIdUseCase>();
        services.AddScoped<IDeleteExpenseByIdUseCase, DeleteExpenseByIdUseCase>();
        services.AddScoped<IUpdateExpenseByIdUseCase, UpdateExpenseByIdUseCase>();
        services.AddScoped<IGenerateExpensesReportExcelUseCase, GenerateExpensesReportExcelUseCase>();
        services.AddScoped<IGenerateExpensesReportExcelByCustomDatesUseCase, GenerateExpensesReportExcelByCustomDatesUseCase>();
        services.AddScoped<IGenerateExpensesReportPdfUseCase, GenerateExpensesReportPdfUseCase>();
        services.AddScoped<IGenerateExpensesReportPdfByCustomDatesUseCase, GenerateExpensesReportPdfByCustomDatesUseCase>();
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
    }

}
