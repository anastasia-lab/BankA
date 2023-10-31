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
    public class Account<T> : IAccount<T>
        where T : BankInfo, new()
    {
        private AccountType AccountType;

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

        public bool IsOpen { get; set; }

        public decimal AccountNumber { get; set; }
        public T Balance { get; }

        public AccountType Type { get { return AccountType; } set { AccountType = value; } }

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

        //public void Transfer(long amount, object toTransfer, object adressesTransfer)
        //{
        //    (toTransfer as IAccount<Bank>).SetValue((adressesTransfer as IAccount<Bank>).GetValue(amount));
        //}
    }
}
