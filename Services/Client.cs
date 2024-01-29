﻿using System;
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
        #region Свойства

        public string Surname { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PasportData { get; set; } = string.Empty;
        public ObservableCollection<Account<BankInfo>> Account { get; set; } = new ObservableCollection<Account<BankInfo>>();

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

        public Client(string _surName, string _firstName, string _lastName, string _pasport, ObservableCollection<Account<BankInfo>> account)
        {
            Surname = _surName;
            FirstName = _firstName;
            LastName = _lastName;
            PasportData = _pasport;
            this.Account = account;
        }

        #endregion

        #region Метод

        public void AddAccount(long account, string type, string newCurrency)
        {
            Account<BankInfo> addNewAccount = new();
            addNewAccount.AccountNumber = account;
            addNewAccount.IsOpen = true;
            addNewAccount.Balance.Money = 0;

            addNewAccount.GetTypeAccountClient(type);
            addNewAccount.GetCurrencyTypeClient(newCurrency);

            Account.Add(addNewAccount);
        }

        #endregion
    }
}
