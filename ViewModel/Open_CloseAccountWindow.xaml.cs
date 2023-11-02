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
        ObservableCollection<Client> ClientsList;
        Client DataClient = new Client();
        int RecordIndex;

        #endregion

        #region Конструкторы
        public Open_CloseAccountWindow(ObservableCollection<Client> clients)
        {
            InitializeComponent();

            this.ClientsList = clients;
        }

        public Open_CloseAccountWindow(ObservableCollection<Client> clients, Client client, int recordIndex)
        {
            this.ClientsList = clients;
            this.DataClient = client;
            this.RecordIndex = recordIndex;

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
            ComboBoxAccountType.Text = DataClient.AccountType;
            TextBoxCurrency.Text = DataClient.Currency;
            TextBoxAccountNumber.Text = DataClient.AccountNumber.ToString();
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
            string Currency = TextBoxCurrency.Text;
            decimal AccountNumber = NewRandomAccountNumber(ClientsList);

            ComboBoxItem ComboBoxItem = (ComboBoxItem)ComboBoxAccountType.SelectedItem;
            string AccountType = ComboBoxItem.Content.ToString(); //выбранное значение в ComboBox

            Client NewClient = new(Surname,FirstName,LastName,PasportData,AccountType, IsOpen,
                                       AccountStatus, AccountNumber,ValueBalance,Currency);

            try
            {
                BankInfo.AddNewClient(ClientsList, NewClient);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Генерация номера счёта клиента из двух частей
        /// </summary>
        /// <returns></returns>
        private static decimal NewRandomAccountNumber(ObservableCollection<Client> clients)
        {
            Random random = new();
            decimal NewAccountNumber;

            //первая часть номера счёта клиента
            string HalfAccountNumberOne = random.NextInt64(1000000000, 9999999999).ToString();

            //вторая часть номера счёта клиента
            string HalfAccountNumberTwo = random.NextInt64(1000000000, 9999999999).ToString();

            //целый номер счёта клиента
            string WholeNumber = HalfAccountNumberOne+HalfAccountNumberTwo;

            NewAccountNumber = decimal.Parse(WholeNumber);
            BankInfo.GetCheckClientAccountNumber(clients, NewAccountNumber);

            return NewAccountNumber;
        }

        /// <summary>
        /// Кнопка "Открыть счёт"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonOpenAccount_Click(object sender, RoutedEventArgs e)
        {
            OpenAccountForNewClient();
        }

        /// <summary>
        /// Открытие нового счёта существуещему клиенту
        /// </summary>
        private void OpenNewAccountNumderToExistingClient()
        {
            if (ClientsList != null && DataClient != null)
            {
                decimal NewAccountNumber = NewRandomAccountNumber(ClientsList);
            }
        }

        /// <summary>
        /// Закрытие счёта
        /// </summary>
        /// <exception cref="ArgumentException">Обработчик ошибки при закрытии</exception>
        private void CloseAccountNumber()
        {
            if (ClientsList != null && DataClient != null)
            {
                if (ClientsList[RecordIndex].IsOpen == true)
                {
                    ClientsList[RecordIndex].IsOpen = false;
                    if (ClientsList[RecordIndex].ValueBalance != 0)
                        throw new ArgumentException("Для закрытия счёта необходимо иметь нулевой баланс");

                    ClientsList.RemoveAt(RecordIndex);
                    ClientsList.Insert(RecordIndex, DataClient);

                    BankInfo.SaveEditData(ClientsList);
                }
            }
        }

        /// <summary>
        /// Кнопка "Закрыть счёт"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonCloseAccount_Click(object sender, RoutedEventArgs e)
        {
            CloseAccountNumber();
        }

        #endregion
    }
}
