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
    /// Логика взаимодействия для Open_CloseAccountWindow.xaml
    /// </summary>
    public partial class Open_CloseAccountWindow : Window
    {
        #region Объявление переменных
        ObservableCollection<Client> Clients = new();
        string ButtonText = string.Empty;
        Client Client = new();
        BankInfo Bank = new();

        #endregion

        #region Конструкторы
        public Open_CloseAccountWindow(ObservableCollection<Client> clients)
        {
            InitializeComponent();

            Clients = clients;
        }


        /// <summary>
        /// Открытие лицевого счёта для нового клиента
        /// </summary>
        private void OpenAccountForNewClient()
        { 
            string Surname = textBoxSurnameAccount.Text;
            string FirstName = textBoxFirstNameAccount.Text;
            string LastName = textBoxLastNameAccount.Text;
            string PasportData = textBoxPasportAccount.Text;
            string AccountStatus = "Открыт";
            bool IsOpen = true;
            long ValueBalance = 0;
            string Currency = "RUB";

            ComboBoxItem ComboBoxItem = (ComboBoxItem)ComboBoxAccountType.SelectedItem;
            string AccountType = ComboBoxItem.Content.ToString(); //выбранное значение в ComboBox

            Random random = new Random();
            decimal AccountNumber = random.NextInt64(100000000000000000, 999999999999999999);

            Client NewClient = new Client(Surname,FirstName,LastName,PasportData,AccountType, IsOpen,
                                       AccountStatus, AccountNumber,ValueBalance,Currency);

            try
            {
                Bank.AddNewClient(Clients, NewClient);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ButtonOpenAccount_Click(object sender, RoutedEventArgs e)
        {
            OpenAccountForNewClient();
        }

        #endregion
    }
}
