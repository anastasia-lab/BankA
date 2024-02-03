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

        public ObservableCollection<Client> Clients { get; set; } = new ObservableCollection<Client>();
        long AccountNumber { get; set; }
        Client SelectedClient { get; set; } = new Client();

        #endregion

        #region Конструкторы
        public TransferWindow(ObservableCollection<Client> clients, Client client, long accountNumber)
        {
            InitializeComponent();

            this.Clients = clients;
            SelectedClient = client;
            AccountNumber = accountNumber;

            StackPanelFromWhom.Visibility = Visibility.Hidden;
            StackPanelToWhom.Visibility = Visibility.Hidden;
        }

        public TransferWindow(ObservableCollection<Client> clients) 
        { 
            InitializeComponent();
            this.Clients = clients;

            GetAccount();
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

        private void TransferAccount()
        {

        }
        private void GetAccount()
        { 
            ObservableCollection<string> accs = new ObservableCollection<string>();
            for (int i = 0; i < Clients.Count; i++)
            {
                accs.Add(Clients[i].Surname + " " + Clients[i].FirstName + " " + Clients[i].LastName);
            }

            ComboBoxFromWhom.ItemsSource = accs;
            ComboBoxToWhom.ItemsSource = accs;
        }

        /// <summary>
        /// Кнопка "Пополнение счёта"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonTransfer_Click(object sender, RoutedEventArgs e)
        {
            AddBalance();
        }




        #endregion

        private void ComboBoxAccountNumberFromWhom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}
