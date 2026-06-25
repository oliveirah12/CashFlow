using CashFlow.Domain.Entities;
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
    public ExpenseIdentityManager Expense { get; set; } = default!;
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
        var user = AddTeamMemberUser(dbContext, encrypter, accessTokenGenerator);
        AddExpenses(dbContext, user);

        dbContext.SaveChanges();
    }

    private User AddTeamMemberUser(
        CashFlowDbContext dbContext, 
        IPasswordEncrypter encrypter, 
        IAccessTokenGenerator accessTokenGenerator)
    {
        var user = UserBuilder.Build();
        var password = user.Password;
        user.Password = encrypter.Encrypt(password);

        dbContext.Users.Add(user);

        var token = accessTokenGenerator.Generate(user);

        User_Team_Member = new UserIdentityManager(user, password, token);

        return user;
    }

    private void AddExpenses(CashFlowDbContext dbContext, User user)
    {

        var expense = ExpenseBuilder.Build(user);
        dbContext.Expenses.Add(expense);
        Expense = new ExpenseIdentityManager(expense);
    }
}