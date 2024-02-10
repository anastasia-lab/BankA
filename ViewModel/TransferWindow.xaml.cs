using BankA.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    /// Логика взаимодействия для TransferWindow.xaml
    /// </summary>
    public partial class TransferWindow : Window
    {
        #region Объявление переменных

        // Список всех клиентов
        public ObservableCollection<Client> Clients { get; set; } = new ObservableCollection<Client>();

        //Номер счета
        long AccountNumber { get; set; }

        //Выбранный клиент
        Client SelectedClient { get; set; } = new Client();

        //Название кнопки, чтобы вызвать необходимый метод
        string SelectButtonContetn { get; set; } = string.Empty;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Конструктор для пополнение баланса клиента
        /// </summary>
        /// <param name="clients"> Список клиентов</param>
        /// <param name="client"> Выбранный клиент</param>
        /// <param name="accountNumber"> Номер лицевого счёта для пополнения</param>
        public TransferWindow(ObservableCollection<Client> clients, Client client, long accountNumber, string buttonContent)
        {
            InitializeComponent();

            this.Clients = clients;
            SelectedClient = client;
            AccountNumber = accountNumber;
            this.SelectButtonContetn = buttonContent;

            StackPanelFromWhom.Visibility = Visibility.Hidden;
            StackPanelToWhom.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Конструктор для перевода средств другому клиенту
        /// </summary>
        /// <param name="clients"> Список всех клиентов</param>
        /// <param name="client"> Выбранный клиент</param>
        public TransferWindow(ObservableCollection<Client> clients, Client client, string buttonContent) 
        { 
            InitializeComponent();
            this.Clients = clients;
            SelectedClient = client;
            this.SelectButtonContetn = buttonContent;
            GetDataClients();
        }

        #endregion

        #region Методы

        /// <summary>
        /// Кнопка "Пополнение счёта"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonTransfer_Click(object sender, RoutedEventArgs e)
        {
            if(SelectButtonContetn == "Пополнить баланс")
                AddBalance();
            if (SelectButtonContetn == "Перевести")
                TransferAccount();
        }

        /// <summary>
        /// Пополнение счёта
        /// </summary>
        private void AddBalance()
        {
            try
            {
                (Clients.First(x => x.PasportData == SelectedClient.PasportData).Account).
                    First(x => x.AccountNumber == AccountNumber).GetTransfer(long.Parse(TextBoxSummTransfer.Text));

                MessageBox.Show("Счёт пополнен", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);

                BankInfo.SaveEditData(Clients);
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message, "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning); }
        }

        /// <summary>
        /// Перевод средств другому клиенту
        /// </summary>
        private void TransferAccount()
        {
            try
            {
                var accountOut = new Account<BankInfo>();
                var accountIn = new Account<BankInfo>();

                for (int i = 0; i < Clients.Count; i++)
                {
                    foreach (var client in Clients[i].Account)
                    {
                        if (client.AccountNumber.ToString() == ComboBoxAccountNumberFromWhom.Text)
                            accountOut = client;
                        if (client.AccountNumber.ToString() == ComboBoxAccountNumberToWhom.Text)
                            accountIn = client;
                    }
                }

                if (accountOut.Balance.Money == 0)
                {
                    MessageBox.Show("Недостаточно средст для перевода на другой счёт. Пожалуйста, пополните баланс.", 
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                    Clients.First().Account.First().SetTransaction(accountOut, accountIn, long.Parse(TextBoxSummTransfer.Text));

                BankInfo.SaveEditData(Clients);
                MessageBox.Show("Перевод средств выполнен.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message, "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning); }
        }


        /// <summary>
        /// Вывод информации выбранного клиента и кому можно перевсти средства
        /// </summary>
        private void GetDataClients()
        {
            List<string> FIOClientFromTransfet = new()
            {
                SelectedClient.Surname + " " + SelectedClient.FirstName + " " + SelectedClient.LastName
            };
            ComboBoxFromWhom.ItemsSource = FIOClientFromTransfet;

            List<long> AccountClientFrom = new();
            for (int i = 0; i < SelectedClient.Account.Count; i++)
            {
                if (SelectedClient.Account[i].AccountTypeClient.ToString() == "Current")
                    AccountClientFrom.Add(SelectedClient.Account[i].AccountNumber);
            }    
            ComboBoxAccountNumberFromWhom.ItemsSource = AccountClientFrom;

            List<string> FIOClientToTransfer = new();
            for (int i = 0; i < Clients.Count; i++)
            {
                if (Clients[i].PasportData != SelectedClient.PasportData)
                {
                    FIOClientToTransfer.Add(Clients[i].Surname + " " + Clients[i].FirstName + " " + Clients[i].LastName);
                }
            }
            ComboBoxToWhom.ItemsSource = FIOClientToTransfer;
        }

        /// <summary>
        /// Вывод лицевого счета клиента, которму будут переводить средства
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxToWhom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                object comboBoxSelectedItem = ((ComboBox)sender).SelectedItem;
                string selectedItemString = comboBoxSelectedItem.ToString();

                for (int i = 0; i < Clients.Count; i++)
                {
                    string FIOClientToTransfer = Clients[i].Surname + " " + Clients[i].FirstName + " " + Clients[i].LastName;
                    if (selectedItemString == FIOClientToTransfer)
                    {
                        List<long> listAccount = new();

                        foreach (var account in Clients[i].Account)
                        {
                            if (account.AccountTypeClient.ToString() == "Current")
                                listAccount.Add(account.AccountNumber);
                        }

                        ComboBoxAccountNumberToWhom.ItemsSource = listAccount;
                    }
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
