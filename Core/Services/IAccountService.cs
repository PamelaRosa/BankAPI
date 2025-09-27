using BankAPI.Core.Dtos;
using BankAPI.Core.Models;

namespace BankAPI.Core.Services;

public interface IAccountService
{
    void Reset();
    AccountDto? GetAccount(string Id);
    AccountDto? Deposit(string destinationId, decimal amount);
    AccountDto? Withdraw(string originId, decimal amount);
    (AccountDto? origin, Account destination)? Transfer(string originId, string destinationId, decimal amount);
}
