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

        /// <summary>
        /// Список лицевых счетов
        /// </summary>
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

        public Client(string _surName, string _firstName, string _lastName, string _pasport, ObservableCollection<Account<BankInfo>> accounts)
        {
            Surname = _surName;
            FirstName = _firstName;
            LastName = _lastName;
            PasportData = _pasport;
            //this.Account = account;
            if (accounts != null)
            {
                foreach (var account in accounts)
                {
                    if (account.AccountTypeClient.ToString() == "Saving")
                        this.Account.Add(new Deposit (account.AccountNumber, account.Balance.Money, account.AccountTypeClient));
                    if (account.AccountTypeClient.ToString() == "Current")
                        this.Account.Add(new NoneDeposit(account.AccountNumber, account.Balance.Money, account.AccountTypeClient));
                }
            }
        }

        #endregion

        #region Метод

        /// <summary>
        /// Открытие нового л/с существующему клиенту
        /// </summary>
        /// <param name="newAccount"> Новый л/с </param>
        /// <param name="newType"> Тип л/с </param>
        /// <param name="newCurrency"> Валюта л/с </param>
        public void AddAccount(long newAccount, string newType, string newCurrency)
        {
            Account<BankInfo> addNewAccount = new();
            addNewAccount.AccountNumber = newAccount;
            addNewAccount.IsOpen = true;
            addNewAccount.Balance.Money = 0;

            addNewAccount.GetTypeAccountClient(newType);
            addNewAccount.GetCurrencyTypeClient(newCurrency);

            Account.Add(addNewAccount);
        }

        #endregion
    }
}
