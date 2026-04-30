using System;
using Bogus;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Cryptography;

namespace CommonTestUtilities.Entities;

public class UserBuilder
{
    public static User Build()
    {
        var passwordEncripter = new PasswordEncrypterBuilder().Build();

        var user = new Faker<User>()
            .RuleFor(u => u.Id, _ => 1)
            .RuleFor(u => u.Name, f => f.Name.FirstName())
            .RuleFor(u => u.Email, (f, user) => f.Internet.Email(user.Name))
            .RuleFor(u => u.Password, (_, user) => passwordEncripter.Encrypt(user.Password))
            .RuleFor(u => u.UserIdentifier, _ => Guid.NewGuid())
            .Generate();

        return user;
    }
}
