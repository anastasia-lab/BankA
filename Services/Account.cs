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

        public Account(long _accountNumber,bool isOpen, long money, AccountType type, CurrencyType currencyType)
        {
            Balance = new();
            AccountNumber = _accountNumber;
            Balance.Money = money;
            AccountTypeClient = type;
            IsOpen = isOpen;
            CurrencyTypeClient = currencyType;
        }

        public Account(long accountNumber, bool isOpen, long money, string type, string currency)
        {
            Balance = new();
            this.AccountNumber = accountNumber;
            this.IsOpen = isOpen;
            this.Balance.Money = money;

            if (type == "Текущий")
                AccountTypeClient = AccountType.Current;
            if (type == "Сберегательный")
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

        /// <summary>
        /// Проверка статуса лицевого счёта
        /// </summary>
        public bool IsOpen { get { return isOpen; } set { isOpen = value; OnPropertyChanged("IsOpen"); } }

        /// <summary>
        /// Номер лицевого счёта
        /// </summary>
        public long AccountNumber { get { return accountNumber; } set { accountNumber = value; OnPropertyChanged("AccountNumber"); } }

        /// <summary>
        /// Баланс лицевого счёта
        /// </summary>
        public T Balance { get { return balance; } set { balance = value; OnPropertyChanged("Balance"); } }

        /// <summary>
        /// Тип лицевого счёта
        /// </summary>
        public AccountType AccountTypeClient { get { return accountType; } set { accountType = value; OnPropertyChanged("AccountType"); } }

        /// <summary>
        /// Типа валюты
        /// </summary>
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

        /// <summary>
        /// Перевод средств между клиентами
        /// </summary>
        /// <param name="accountOut"> Клиент, который переводит средства</param>
        /// <param name="accountOut"> Клиент, которому переводят средства</param>
        /// <param name="amount"> Сумма перевода</param>
        public void SetTransaction(Account<BankInfo> accountOut, Account<BankInfo> accountIn, long amount)
        {
            accountOut.Balance.Money = accountOut.Balance.Money - amount;
            accountIn.Balance.Money = accountIn.Balance.Money + amount;
        }

        /// <summary>
        /// Подсчёт количества лицевых счетов клиента
        /// </summary>
        public int Count
        { get; }

        /// <summary>
        /// Получение типа лицевого счёта для нового клиента
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
        /// Получение типа валюты для нового клиента
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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        #endregion

    }

    /// <summary>
    /// Депозитный счет для типа "Сберегательный"
    /// </summary>
    public class Deposit : Account<BankInfo>
    { 
        public Deposit(long _accountNumber,bool isOpen, long money, AccountType type, CurrencyType currencyType) 
            : base(_accountNumber,isOpen, money, type, currencyType)
        { }

        public Deposit() { }

        /// <summary>
        /// Пополнение средств
        /// </summary>
        /// <param name="amount"> Сумма перевода</param>
        public override long GetTransfer(long amount)
        {
            if (amount < 100)
                Balance.Money += (amount * 5);
            if (amount > 100 && amount < 1000)
                Balance.Money += (amount * 7);
            if (amount > 1000)
                Balance.Money += (amount * 10);

            return Balance.Money;
        }

    }

    /// <summary>
    /// Недепозитный счёт для типа "Текущий"
    /// </summary>
    public class NoneDeposit : Account<BankInfo>
    {
        public NoneDeposit(long _accountNumber, bool isOpen, long money, AccountType type, CurrencyType currencyType) 
            : base(_accountNumber, isOpen, money, type, currencyType)
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
