using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using static BankA.Services.Interfaces;

namespace BankA.Services
{
    public enum AccountType { Current, Savings }; // тип счёта: текущий, сбережения
    public enum CurrencyType { RUB, USA, EUR}; // тип валюты
    public class Account<T> : IAccount<T>
        where T : BankInfo, new()
    {
        #region Объявление переменных

        private AccountType accountType;
        private CurrencyType currencyType;
        private long accountNumber;
        private bool isOpen;
        private T balance;

        #endregion

        #region Конструктор
        public Account()
        {
            Balance = new();
            IsOpen = false;
        }

        public Account(T inputBalance)
        {
            Balance = inputBalance;
            IsOpen = true;
        }

        #endregion

        #region Свойства

        public bool IsOpen { get { return isOpen; } set { isOpen = value; } }

        public long AccountNumber { get { return accountNumber; } set { accountNumber = value; } }
        public T Balance { get { return balance; } set { balance = value; } }

        public AccountType Type { get { return accountType; } set { accountType = value; } }
        public CurrencyType CurrencyType { get {  return currencyType; } set { currencyType = value; } }

        #endregion

        #region Методы

        public T GetValue(long amount)
        {
            if (amount > Balance.Money)
                throw new ArgumentException("Превышен лимит!");

            Balance.Money -= amount;
            T t = new();
            t.Money += amount;

            return t;
        }

        public void SetValue(T value)
        {
            Balance.Money += value.Money;
        }

        public int Count
        { get; }

        #endregion

    }
}
