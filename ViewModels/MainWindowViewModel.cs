using System;
using System.Reactive;
using Avalonia.Threading;
using MyApp.Models;
using ReactiveUI;

namespace MyApp.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
    private BankAccount _account = new BankAccount(
            bankName: "",
            inn: "7707083893",
            bik: "044525225",
            owner: "Иванов Иван",
            creditLimit: 50000,
            commissionRate: 1.5m,
            interestRate: 5.0m
        );

    private decimal _amount;
    private string _statusMessage = string.Empty;
    private string _statusColor = "Black";


    // Счет
    public BankAccount Account
    {
        get => _account;
        set => this.RaiseAndSetIfChanged(ref _account, value);
    }
    public decimal Amount
    {
        get => _amount;
        set => this.RaiseAndSetIfChanged(ref _amount, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => this.RaiseAndSetIfChanged(ref _statusMessage, value);
    }

    public string StatusColor
    {
        get => _statusColor;
        set => this.RaiseAndSetIfChanged(ref _statusColor, value);
    }
    public decimal AvailableCredit => Account.CreditLimit - Account.CreditUsed;

    // Команды
    public ReactiveCommand<Unit, Unit> PutMoneyCommand { get; }
    public ReactiveCommand<Unit, Unit> GetMoneyCommand { get; }
    public ReactiveCommand<Unit, Unit> TakeCreditCommand { get; }
    public ReactiveCommand<Unit, Unit> RepayCreditCommand { get; }
    public ReactiveCommand<Unit, Unit> AccrueInterestCommand { get; }

    public MainWindowViewModel()
    {
        
        // Инициализация команд
        PutMoneyCommand = ReactiveCommand.Create(PutMoney);
        GetMoneyCommand = ReactiveCommand.Create(GetMoney);
        TakeCreditCommand = ReactiveCommand.Create(TakeCredit);
        RepayCreditCommand = ReactiveCommand.Create(RepayCredit);
        AccrueInterestCommand = ReactiveCommand.Create(AccrueInterest);

        // Обновляем UI при изменении счета
        this.WhenAnyValue(x => x.Account.Balance, x => x.Account.CreditUsed)
            .Subscribe(_ => this.RaisePropertyChanged(nameof(AvailableCredit)));
    }

    private void PutMoney()
    {
        try
        {
            Account.PutMoney(Amount);
            
            // Обновляем UI в главном потоке
            Dispatcher.UIThread.Post(() => 
            {
                StatusMessage = $"Успешно пополнено: {Amount:C}";
                StatusColor = "Green";
            });
        }
        catch (Exception ex)
        {
            Dispatcher.UIThread.Post(() => 
            {
                StatusMessage = $"Ошибка: {ex.Message}";
                StatusColor = "Red";
            });
        }
    }

    private void GetMoney()
    {
        try
        {
            Account.GetMoney(Amount);
            StatusMessage = $"Успешно снято: {Amount:C}";
            StatusColor = "Green";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Ошибка: {ex.Message}";
            StatusColor = "Red";
        }
    }

    private void TakeCredit()
    {
        try
        {
            Account.TakeCredit(Amount);
            StatusMessage = $"Кредит получен: {Amount:C} (комиссия {Account.CommissionRate}%)";
            StatusColor = "Green";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Ошибка: {ex.Message}";
            StatusColor = "Red";
        }
    }

    private void RepayCredit()
    {
        try
        {
            Account.RepayCredit(Amount);
            StatusMessage = $"Кредит погашен: {Amount:C}";
            StatusColor = "Green";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Ошибка: {ex.Message}";
            StatusColor = "Red";
        }
    }

    private void AccrueInterest()
    {
        try
        {
            Account.AccrueInterest();
            StatusMessage = $"Начислены проценты по кредиту ({Account.InterestRate}%)";
            StatusColor = "Orange";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Ошибка: {ex.Message}";
            StatusColor = "Red";
        }
    }
}
