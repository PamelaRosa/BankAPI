using BankAPI.Core.Dtos;
using BankAPI.Core.Models;
using System.Collections.Concurrent;
namespace BankAPI.Core.Services;

public class AccountService : IAccountService
{
    private readonly ConcurrentDictionary<string, Account> _accounts = new();

    private static AccountDto? MapToDto(Account? account)
    {
        return account is null ? null : new AccountDto { Id = account.Id, Balance = account.Balance };
    }

    public void Reset()
    {
        _accounts.Clear();
    }
    public AccountDto? GetAccount(string Id)
    {
        _accounts.TryGetValue(Id, out var account);
        return MapToDto(account);
    }

    public AccountDto? Deposit(string destinationId, decimal amount)
    {
        var account = _accounts.GetOrAdd(destinationId, id => new Account(id));
        account.Deposit(amount);

        return MapToDto(account);
    }

    public AccountDto? Withdraw(string originId, decimal amount)
    {
        if (_accounts.TryGetValue(originId, out var account))
            account.Withdraw(amount);
        else
            return null;

        return MapToDto(account);
    }

    public (AccountDto? origin, AccountDto? destination)? Transfer(string originId, string destinationId, decimal amount)
    {
        if (!_accounts.TryGetValue(originId, out var originAccount))
        { 
            return null;
        }
        else
        {
            originAccount.Withdraw(amount);
        }

        if (_accounts.TryGetValue(destinationId, out var destinationAccount))
            destinationAccount.Deposit(amount);
        else
        {
            destinationAccount = new Account(destinationId, amount);
            _accounts[destinationId] = destinationAccount;
        }

        var originDto = MapToDto(originAccount);
        var destinationDto = MapToDto(destinationAccount);

        return (originDto, destinationDto);
    }
}
