using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.tokens;
using CashFlow.Infrastructure.DataAccess;
using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Test.Resources;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public ExpenseIdentityManager Expense_MemberTeam { get; set; } = default!;
    public ExpenseIdentityManager Expense_Admin { get; set; } = default!;
    public UserIdentityManager User_Team_Member {  get; private set; } = default!;
    public UserIdentityManager User_Admin {  get; private set; } = default!;



    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                services.AddDbContext<CashFlowDbContext>(config =>
                {
                    config.UseInMemoryDatabase("InMemoryDbForTesting");
                    config.UseInternalServiceProvider(provider);
                });

                var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<CashFlowDbContext>();
                var encrypter = scope.ServiceProvider.GetRequiredService<IPasswordEncrypter>();

                var accessTokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();

                StartDataBase(dbContext, encrypter, accessTokenGenerator);


            });
    }


    private void StartDataBase(
        CashFlowDbContext dbContext, 
        IPasswordEncrypter encrypter,
        IAccessTokenGenerator accessTokenGenerator)
    {
        var userTeamMember = AddTeamMemberUser(dbContext, encrypter, accessTokenGenerator);
        var expenseTeamMember = AddExpenses(dbContext, userTeamMember, expenseId: 1);
        Expense_MemberTeam = new ExpenseIdentityManager(expenseTeamMember);

        var userAdmin = AddAdminUser(dbContext, encrypter, accessTokenGenerator);
        var expenseAdmin = AddExpenses(dbContext, userAdmin, expenseId: 2);
        Expense_Admin = new ExpenseIdentityManager(expenseAdmin);

        dbContext.SaveChanges();
    }

    private User AddTeamMemberUser(
        CashFlowDbContext dbContext, 
        IPasswordEncrypter encrypter, 
        IAccessTokenGenerator accessTokenGenerator)
    {
        var user = UserBuilder.Build();
        user.Id = 1;

        var password = user.Password;
        user.Password = encrypter.Encrypt(password);

        dbContext.Users.Add(user);

        var token = accessTokenGenerator.Generate(user);

        User_Team_Member = new UserIdentityManager(user, password, token);

        return user;
    }

    private User AddAdminUser(
    CashFlowDbContext dbContext,
    IPasswordEncrypter encrypter,
    IAccessTokenGenerator accessTokenGenerator)
    {
        var user = UserBuilder.Build(Roles.ADMIN);
        user.Id = 2;

        var password = user.Password;
        user.Password = encrypter.Encrypt(password);

        dbContext.Users.Add(user);

        var token = accessTokenGenerator.Generate(user);

        User_Admin = new UserIdentityManager(user, password, token);

        return user;
    }

    private Expense AddExpenses(CashFlowDbContext dbContext, User user, long expenseId)
    {

        var expense = ExpenseBuilder.Build(user);
        expense.Id = expenseId; 
        dbContext.Expenses.Add(expense);
        return expense;
    }
}