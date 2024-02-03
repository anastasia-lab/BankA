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
using System.Xml.Serialization;
using static BankA.Services.Interfaces;

namespace BankA.Services
{
    public enum AccountType  { Current, Saving }; // тип счёта: текущий, сберегательный
    public enum CurrencyType { RUB, USA, EUR}; // тип валюты

    /// <summary>
    /// Лицевой счёт клиента
    /// </summary>
    /// <typeparam name="T"></typeparam>

    [XmlInclude(typeof(Deposit))]
    [XmlInclude(typeof(NoneDeposit))]

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
            IsOpen = true;
        }

        public Account(long _accountNumber, long money, AccountType type)
        {
            Balance = new();
            AccountNumber = _accountNumber;
            Balance.Money = money;
            AccountTypeClient = type;
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

        /// <summary>
        /// Пополнение баланса счёта
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public virtual long GetTransfer(long amount)
        {
            return amount;
        }

        public void SetValue(T accountIn, Account<BankInfo> accountOut, long amount)
        {
            accountIn.Money = Balance.Money + amount;
            accountOut.Balance.Money = Balance.Money - amount;
        }

        public int Count
        { get; }

        /// <summary>
        /// Получение типа лицевого счёта клиента
        /// </summary>
        /// <param name="newAccountType"></param>
        /// <returns> Возврат типа аккаунта</returns>
        public AccountType GetTypeAccountClient(string newAccountType)
        {
            if (newAccountType == "Текущий")
                AccountTypeClient = AccountType.Current;
            if (newAccountType == "Сберегательный")
                AccountTypeClient = AccountType.Saving;

            return AccountTypeClient;
        }


        /// <summary>
        /// Получение типа валюты клиента
        /// </summary>
        /// <param name="newCurrencyType"></param>
        /// <returns> Возврат типа валюты </returns>
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

    /// <summary>
    /// Депозитный счет для типа "Сберегательный"
    /// </summary>
    public class Deposit : Account<BankInfo>
    { 
        public Deposit(long _accountNumber, long money, AccountType type) : base(_accountNumber, money, type)
        { }

        public Deposit() { }

        /// <summary>
        /// Пополнение средств
        /// </summary>
        /// <param name="amount"> Сумма перевода</param>
        public override long GetTransfer(long amount)
        {
            if (amount < 100)
                Balance.Money += (amount * (long)5);
            if (amount > 100 && amount < 1000)
                Balance.Money += (amount * (long)7);
            if (amount > 1000)
                Balance.Money += (amount * (long)10);

            return Balance.Money;
        }

    }

    /// <summary>
    /// Недепозитный счёт для типа "Текущий"
    /// </summary>
    public class NoneDeposit : Account<BankInfo>
    {
        public NoneDeposit(long _accountNumber, long money, AccountType type) : base(_accountNumber, money, type)
        { }

        public NoneDeposit() { }

        /// <summary>
        /// Пополнение средств
        /// </summary>
        /// <param name="amount"> Сумма перевода</param>
        public override long GetTransfer(long amount)
        {
            this.Balance.Money += amount;

            return this.Balance.Money;
        }

    }
}
