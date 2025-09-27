namespace BankAPI.Core.Models
{
    public class Account
    {
        public string Id { get; }
        public decimal Balance { get; private set; }

        public Account(string? id, decimal initialBalance = 0)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("O id é obrigátorio.");

            if (initialBalance < 0)
                throw new ArgumentException("O saldo inicial não pode ser negativo.");

            Id = id;
            Balance = initialBalance;
        }
        public void Deposit(decimal amount)
        {
            if(amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "O valor não pode ser negativo.");
            Balance += amount;
        }
        public void Withdraw(decimal amount)
        {
            if(amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "O valor não pode ser negativo.");
            if (amount > Balance)
                throw new InvalidOperationException("Saldo insuficiente.");
            Balance -= amount;
        }
    }
}
