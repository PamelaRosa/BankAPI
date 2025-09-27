using BankAPI.Core.Dtos;
using BankAPI.Core.Models;

namespace BankAPI.Core.Services;

public class AccountService : IAccountService
{
    private readonly Dictionary<string, Account> _accounts = new();

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
        if (_accounts.TryGetValue(destinationId, out var account))
            account.Balance += amount;
        else
        {
            account = new Account { Id = destinationId, Balance = amount };
            _accounts[destinationId] = account;
        }

        return MapToDto(account);
    }

    public AccountDto? Withdraw(string originId, decimal amount)
    {
        if (_accounts.TryGetValue(originId, out var account))
            account.Balance -= amount;
        else         
            return null;

        return MapToDto(account);
    }

    public (AccountDto? origin, Account destination)? Transfer(string originId, string destinationId, decimal amount)
    {
        if (!_accounts.TryGetValue(originId, out var originAccount))
        { 
            return null;
        }
        else
        {
            originAccount.Balance -= amount;
        }

        if (_accounts.TryGetValue(destinationId, out var destinationAccount))
            destinationAccount.Balance += amount;
        else
        {
            destinationAccount = new Account { Id = destinationId, Balance = amount };
            _accounts[destinationId] = destinationAccount;
        }

        var originDto = MapToDto(originAccount);

        return (originDto, destinationAccount);
    }
}
