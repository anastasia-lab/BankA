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
        #region Объявление переменных и свойств
        public string Surname { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PasportData { get; set; } = string.Empty;
        public string AccountStatus { get; set; } = string.Empty;
        public long ValueBalance { get; set; }
        public string AccountType { get; set; } = string.Empty;
        public bool IsOpen { get; set; }

        public ObservableCollection<long> accounts = new();
        public ObservableCollection<long> AccountsNumber { get => accounts; set { accounts = value; } }

        //Название валюты
        public string Currency { get; set; } = string.Empty;
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

        public Client(string _surName, string _firstName, string _lastName, string _pasport, string _accountType, bool _isOpen,
            string _accountStatus, ObservableCollection<long> _accountsNumber, long _accountBalance, string _currency)
        {
            Surname = _surName;
            FirstName = _firstName;
            LastName = _lastName;
            PasportData = _pasport;
            AccountType = _accountType;
            IsOpen = _isOpen;
            AccountStatus = _accountStatus;
            AccountsNumber = _accountsNumber;
            ValueBalance = _accountBalance;
            Currency = _currency;
        }

        #endregion

        public static TTargetType ParseTo<TTargetType>(string target)
        {
            return (TTargetType)System.Convert.ChangeType(target, typeof(TTargetType));
        }
    }
}
