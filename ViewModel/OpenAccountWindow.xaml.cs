﻿using BankA.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BankA.ViewModel
{
    /// <summary>
    /// Логика взаимодействия для Open_CloseAccountWindow.xaml
    /// </summary>
    public partial class OpenAccountWindow : Window
    {
        #region Объявление переменных
         
        // Список всех клиентов
        public ObservableCollection<Client> ClientsList { get; set; } = new ObservableCollection<Client>();

        // Данные выбранного клиента
        public Client DataClient { get; set; } = new Client();

        #endregion

        #region Конструкторы

        /// <summary>
        /// Конструктор для добавления нового клиента
        /// </summary>
        /// <param name="clients">Список всех клиентов</param>
        public OpenAccountWindow(ObservableCollection<Client> clients)
        {
            InitializeComponent();

            this.ClientsList = clients;
            checkBoxNewClient.IsChecked = true;
        }

        /// <summary>
        /// Конструкто для отрытия лицевого счёта существующего клиента
        /// </summary>
        /// <param name="clients">Список клиентов</param>
        /// <param name="client">Данные выбранного клиента из списка</param>
        /// <param name="recordIndex">Индекс выбранного клиента из списка</param>
        public OpenAccountWindow(ObservableCollection<Client> clients, Client client)
        {
            InitializeComponent();

            this.ClientsList = clients;
            this.DataClient = client;
            checkBoxNewClient.IsChecked = false;

            GetShowSelectData();
        }

        #endregion

        #region Методы


        /// <summary>
        /// Загрузка данных
        /// </summary>
        private void GetShowSelectData()
        {
            textBoxSurnameAccount.Text = DataClient.Surname;
            textBoxFirstNameAccount.Text = DataClient.FirstName;
            textBoxLastNameAccount.Text = DataClient.LastName;
            textBoxPasportAccount.Text = DataClient.PasportData;
        }

        /// <summary>
        /// Кнопка "Открыть счёт"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonOpenAccount_Click(object sender, RoutedEventArgs e)
        {
            if (checkBoxNewClient.IsChecked == true)
                OpenAccountForNewClient();
            if (checkBoxNewClient.IsChecked == false)
                OpenNewAccountNumberToExistingClient();
        }

        /// <summary>
        /// Открытие лицевого счёта для нового клиента
        /// </summary>
        private void OpenAccountForNewClient()
        {
            try
            {
                //Для добавления лицевого счёта новому клиента
                ObservableCollection<Account<BankInfo>> AccountClient = new();

                Account<BankInfo> newAccount = new ();
                string Surname = textBoxSurnameAccount.Text;
                string FirstName = textBoxFirstNameAccount.Text;
                string LastName = textBoxLastNameAccount.Text;
                string PasportData = textBoxPasportAccount.Text;

                for (int i = 0; i < ClientsList.Count; i++)
                {
                    if (PasportData == ClientsList[i].PasportData)
                        MessageBox.Show("Проверьте корректность паспортных данных", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                newAccount.Balance.Money = 0;
                newAccount.IsOpen = true;

                newAccount.AccountNumber = NewRandomAccountNumber(ClientsList);
                newAccount.GetTypeAccountClient(ComboBoxAccountType.Text);
                newAccount.GetCurrencyTypeClient(ComboBoxCurrency.Text);

                AccountClient.Add(newAccount);

                Client NewClient = new(Surname, FirstName, LastName, PasportData, AccountClient);
                ClientsList.Add(NewClient);

                BankInfo.SaveEditData(ClientsList);
                MessageBox.Show("Новый клиент добавлен.", "Информация!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Генерация нового номера лицевого счёта клиента
        /// </summary>
        /// <param name="clients">Список клиентов</param>
        /// <returns></returns>
        private static long NewRandomAccountNumber(ObservableCollection<Client> clients)
        {
            Random random = new();
            long NewAccountNumber = 0;

            try
            {
                //Сгенерированый рандомный номер лицевого счёта
                NewAccountNumber = random.NextInt64();

                
                BankInfo.GetCheckClientAccountNumber(clients, NewAccountNumber);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            return NewAccountNumber;
        }

        /// <summary>
        /// Открытие нового счёта существуещему клиенту
        /// </summary>
        private void OpenNewAccountNumberToExistingClient()
        {
            try
            {
                if (ClientsList != null && DataClient != null)
                {
                    var newClientAccount = NewRandomAccountNumber(ClientsList);
                    var newCurrency = ComboBoxCurrency.Text;
                    var newType = ComboBoxAccountType.Text;

                    ClientsList.First(x => x.PasportData == DataClient.PasportData)
                        .AddAccount(newClientAccount, newType, newCurrency);

                    BankInfo.SaveEditData(ClientsList);
                    MessageBox.Show("Новый лицевой счёт добавлен.", "Информация!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        #endregion
    }
}