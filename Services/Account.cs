using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static BankA.Services.Interfaces;

namespace BankA.Services
{
    public enum AccountType: int { Current, Saving }; // тип счёта: текущий, сбережения
    public enum CurrencyType { RUB, USA, EUR}; // тип валюты
    public class Account<T> : IAccount<T>, INotifyPropertyChanged
        where T : BankInfo, new()
    {
        #region Объявление переменных

        private AccountType accountType;
        private CurrencyType currencyType;
        private long accountNumber;
        private bool isOpen;
        private T balance;

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        #region Конструктор
        public Account()
        {
            Balance = new();
            IsOpen = false;
        }

        public Account(long _accountNumber, long money, string currency)
        {
            AccountNumber = _accountNumber;
            Balance.Money = money;
        }

        public Account(long accountNumber, bool isOpen, long money, string type, string currency)
        {
            this.AccountNumber = accountNumber;
            this.IsOpen = isOpen;
            this.Balance.Money = money;

            if (type == "Текущий")
                AccountTypeClient = AccountType.Current;
            if (type == "Сберегающий")
                AccountTypeClient = AccountType.Saving;

            if (currency == "РУБ")
                CurrencyTypeClient = CurrencyType.RUB;
            if (currency == "USA")
                CurrencyTypeClient = CurrencyType.USA;
            if (currency == "EUR")
                CurrencyTypeClient = CurrencyType.EUR;
        }

        #endregion

        #region Свойства

        public bool IsOpen { get { return isOpen; } set { isOpen = value; OnPropertyChanged("IsOpen"); } }

        public long AccountNumber { get { return accountNumber; } set { accountNumber = value; OnPropertyChanged("AccountNumber"); } }
        public T Balance { get { return balance; } set { balance = value; OnPropertyChanged("Balance"); } }

        public AccountType AccountTypeClient { get { return accountType; } set { accountType = value; OnPropertyChanged("AccountType"); } }
        public CurrencyType CurrencyTypeClient { get {  return currencyType; } set { currencyType = value; OnPropertyChanged("CurrencyType"); } }

        #endregion

        #region Методы

        public virtual void GetValue(long amount)
        {
            //if (amount > Balance.Money)
            //    throw new ArgumentException("Превышен лимит!");

            //Balance.Money -= amount;
            //T t = new();
            //t.Money += amount;

            //return t;
        }

        public void SetValue(T value)
        {
            Balance.Money += value.Money;
        }

        public int Count
        { get; }
        public AccountType GetTypeAccountClient(string newAccountType)
        {
            if (newAccountType == "Текущий")
                AccountTypeClient = AccountType.Current;
            if (newAccountType == "Сберегающий")
                AccountTypeClient = AccountType.Saving;

            return AccountTypeClient;
        }

        public CurrencyType GetCurrencyTypeClient(string newCurrencyType)
        {
            if (newCurrencyType == "РУБ")
                CurrencyTypeClient = CurrencyType.RUB;
            if (newCurrencyType == "USA")
                CurrencyTypeClient = CurrencyType.USA;
            if (newCurrencyType == "EUR")
                CurrencyTypeClient = CurrencyType.EUR;

            return CurrencyTypeClient;
        }
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #endregion

    }

    public class Deposit : Account<BankInfo>
    { 
        public Deposit(long _accountNumber, long money, string currency) : base(_accountNumber, money, currency)
        { }

        /// <summary>
        /// Перевод средств
        /// </summary>
        /// <param name="amount"> Сумма перевода</param>
        public override void GetValue(long amount)
        {
            if(amount < 100)
                this.Balance.Money += (amount * (long)0.05);
            if (amount > 100 && amount < 1000)
                this.Balance.Money += (amount * (long)0.25);
            if (amount > 1000)
                this.Balance.Money += (amount * (long)0.1);
        }

    }

    public class NoneDeposit : Account<BankInfo>
    {
        public NoneDeposit(long _accountNumber, long money, string currency) : base(_accountNumber, money, currency)
        { }

        /// <summary>
        /// Перевод средств
        /// </summary>
        /// <param name="amount"> Сумма перевода</param>
        public override void GetValue(long amount)
        {
            this.Balance.Money += amount;
        }

    }
}
