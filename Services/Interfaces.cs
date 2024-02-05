using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankA.Services
{
    class Interfaces
    {
        public interface IAccount<T> : ICovariance<T>, IContrvariance<Account<BankInfo>, Account<BankInfo>>
        {
            T Balance { get; }
            bool IsOpen { get; set; }
        }

        /// <summary>
        /// Пополнение баланса
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public interface ICovariance<out T>
        {
           long GetTransfer(long amount);
        }

        /// <summary>
        /// Перевод между клиентами
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        public interface IContrvariance<in T1, in T2> where T1: Account<BankInfo> where T2:Account<BankInfo> 
        {
            void SetTransaction(Account<BankInfo> accountOut, Account<BankInfo> accountIn, long amount);
        }
    }
}
