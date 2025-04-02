using System;
using System.ComponentModel;
using ReactiveUI;

namespace MyApp.Models;

public class BankAccount : ReactiveObject // Наследуем от ReactiveObject
{
    public string BankName { get; }
    public string INN { get; }
    public string BIK { get; }
    public string Owner { get; }

    private decimal _balance;
    private decimal _creditLimit;
    private decimal _creditUsed;
    private decimal _commissionRate;
    private decimal _interestRate;

    public decimal Balance
    {
        get => _balance;
        private set => this.RaiseAndSetIfChanged(ref _balance, value);
        
    }

    public decimal CreditLimit => _creditLimit;
    public decimal CommissionRate => _commissionRate;
    public decimal InterestRate => _interestRate;
    public decimal CreditUsed => _creditUsed;

    public BankAccount(string bankName, string inn, string bik, string owner,
        decimal creditLimit, decimal commissionRate, decimal interestRate)
    {
        BankName = bankName;
        INN = inn;
        BIK = bik;
        Owner = owner;
        _creditLimit = creditLimit;
        _commissionRate = commissionRate;
        _interestRate = interestRate;
    }

    public void TakeCredit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Значение должно быть > 0!");

        if (_creditUsed + amount > _creditLimit)
            throw new ArgumentException("Превышен лимит кредита!");

        _creditUsed += amount;
        Balance += amount - (amount / 100 * _commissionRate);
    }

    public void PutMoney(decimal amount)
    {
        if (amount <= 0) { throw new ArgumentException("Сумма должна быть > 0!"); }

        Balance += amount;
        Console.WriteLine("Средства успешно зачислены на счёт!");
    }

    public void GetMoney(decimal amount)
    {

        if (amount <= 0)
        {
            throw new ArgumentException("Сумма должна быть > 0!");
        }

        if (Balance - amount >= 0)
        {
            Balance = Balance - amount;
            Console.WriteLine("Средства успешно сняты со счёта!");
        }
        else
        {
            throw new InvalidOperationException("Недостаточно средств для снятия!");
        }

    }

    public void RepayCredit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Сумма должна быть положительной.");

        if (amount > _creditUsed)
            throw new InvalidOperationException("Сумма погашения превышает задолженность.");

        _creditUsed -= amount;
        Balance -= amount;

    }

    // Метод для начисления процента на кредитные средства
    public void AccrueInterest()
    {
        decimal interest = (_creditUsed / 100 * _interestRate);
        _creditUsed += interest;
        Console.WriteLine($"Начислены проценты по кредиту: {interest}. Общий долг: {_creditUsed}");
    }

    internal void Withdraw(decimal transactionAmount)
    {
        throw new NotImplementedException();
    }
}