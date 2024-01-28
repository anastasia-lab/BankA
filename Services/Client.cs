using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static BankA.Services.Interfaces;
using System.Windows;
using System.Windows.Documents;

namespace BankA.Services
{
    public class Client
    {
        private Account<BankInfo> accountNumber;

        #region Свойства

        public string Surname { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PasportData { get; set; } = string.Empty;
        public Account<BankInfo> Account 
        { 
            get { return accountNumber; }
            set { accountNumber = value; }
        } 

        #endregion

        #region Конструкторы

        public Client() { }

        public Client(string _surName, string _firstName, string _lastName, string _pasport)
        {
            Surname = _surName;
            FirstName = _firstName;
            LastName = _lastName;
            PasportData = _pasport;
        }

        public Client(string _surName, string _firstName, string _lastName, string _pasport, Account<BankInfo> account)
        {
            Surname = _surName;
            FirstName = _firstName;
            LastName = _lastName;
            PasportData = _pasport;
            this.Account = account;
        }

        #endregion

        //public void AddAccount(long account, string Type, string NewCurrency)
        //{
        //    this.AccountsNumber.Add(account);
        //    Currency = NewCurrency;
        //    AccountType = Type;
        //    IsOpen = true;
        //    AccountStatus = "Открыт";
        //    ValueBalance = 3000;
        //}
    }
}
