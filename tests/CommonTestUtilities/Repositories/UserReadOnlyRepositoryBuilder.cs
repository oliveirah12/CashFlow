using System;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Users;
using Moq;

namespace CommonTestUtilities.Repositories;

public class UserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _repositoryMock;

    public UserReadOnlyRepositoryBuilder()
    {
        _repositoryMock = new Mock<IUserReadOnlyRepository>();
    }

    public void ExistActiveUserWithEmail(string email)
    {
        _repositoryMock.Setup(x => x.ExistActiveUserWithEmail(email)).ReturnsAsync(true);
    }

    public UserReadOnlyRepositoryBuilder GetUserByEmail(User user)
    {
        _repositoryMock.Setup(x => x.GetUserByEmail(user.Email)).ReturnsAsync(user);
        return this;
    }


    public IUserReadOnlyRepository Build() => _repositoryMock.Object;

}
