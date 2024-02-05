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

        #endregion

        #region Конструкторы

        /// <summary>
        /// Конструктор для пополнение баланса клиента
        /// </summary>
        /// <param name="clients"> Список клиентов</param>
        /// <param name="client"> Выбранный клиент</param>
        /// <param name="accountNumber"> Номер лицевого счёта для пополнения</param>
        public TransferWindow(ObservableCollection<Client> clients, Client client, long accountNumber)
        {
            InitializeComponent();

            this.Clients = clients;
            SelectedClient = client;
            AccountNumber = accountNumber;

            StackPanelFromWhom.Visibility = Visibility.Hidden;
            StackPanelToWhom.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Конструктор для перевода средств другому клиенту
        /// </summary>
        /// <param name="clients"> Список всех клиентов</param>
        /// <param name="client"> Выбранный клиент</param>
        public TransferWindow(ObservableCollection<Client> clients, Client client) 
        { 
            InitializeComponent();
            this.Clients = clients;
            SelectedClient = client;
            GetDataClients();
        }

        #endregion

        #region Методы

        /// <summary>
        /// Пополнение счёта
        /// </summary>
        private void AddBalance()
        {
            try
            {
                (Clients.First(x => x.PasportData == SelectedClient.PasportData).Account).
                    First(x => x.AccountNumber == AccountNumber).GetTransfer(long.Parse(TextBoxSummTransfer.Text));

                MessageBox.Show("Счёт пополнен", "Информация!", MessageBoxButton.OK, MessageBoxImage.Information);

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
            //var acc1 = Clients.First(x => x == ComboBoxFromWhom.SelectedItem).Account.
            //First(x => x.AccountNumber.ToString() == ComboBoxAccountNumberFromWhom.Text);
            //var acc2 = (Account<BankInfo>)Clients.First(x => x == (Client)ComboBoxToWhom.SelectedItem).Account.
            //    First(x => x.AccountNumber.ToString() == ComboBoxAccountNumberToWhom.Text);

            //for (int i = 0; i < Clients.Count; i++)
            //{
            //    foreach (var client in Clients[i].Account)
            //    {
            //        client.SetValue(acc1, acc2, long.Parse(TextBlockSummTransfer.Text));
            //    }
            //}
        }

        /// <summary>
        /// Кнопка "Пополнение счёта"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonTransfer_Click(object sender, RoutedEventArgs e)
        {
            //AddBalance();
            TransferAccount();
        }

        /// <summary>
        /// Вывод информации выбранного клиента и кому можно перевсти средства
        /// </summary>
        private void GetDataClients()
        {
            List<string> FIOClientFrom = new()
            {
                SelectedClient.Surname + " " + SelectedClient.FirstName + " " + SelectedClient.LastName
            };
            ComboBoxFromWhom.ItemsSource = FIOClientFrom;

            List<long> AccountClientFrom = new List<long>();
            for (int i = 0; i < SelectedClient.Account.Count; i++)
                AccountClientFrom.Add(SelectedClient.Account[i].AccountNumber);
            ComboBoxAccountNumberFromWhom.ItemsSource = AccountClientFrom;

            List<string> strings = new List<string>();
            for (int i = 0; i < Clients.Count; i++)
            {
                if (Clients[i].PasportData != SelectedClient.PasportData)
                {
                    strings.Add(Clients[i].Surname + " " + Clients[i].FirstName + " " + Clients[i].LastName);
                }
            }
            ComboBoxToWhom.ItemsSource = strings;
        }

        /// <summary>
        /// Вывод лицевого счета клиента, которму будут переводить средства
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxToWhom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object cm = ((ComboBox)sender).SelectedItem;
            string st = cm.ToString();

            for (int i = 0; i < Clients.Count; i++)
            {
                string info = Clients[i].Surname + " " + Clients[i].FirstName + " " + Clients[i].LastName;
                if (st == info)
                {
                    List<long> list = new List<long>();
                    foreach (var acc in Clients[i].Account)
                    {
                            list.Add(acc.AccountNumber);
                    }
                    ComboBoxAccountNumberToWhom.ItemsSource = list;
                }
            }
        }

        #endregion

    }
}
