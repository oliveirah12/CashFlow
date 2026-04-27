using System;
using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Security.tokens;

public interface IAccessTokenGenerator
{
    string Generate(User user);  
}
