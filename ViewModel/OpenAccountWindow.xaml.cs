using BankA.Services;
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
        
        public ObservableCollection<Client> ClientsList { get; set; } = new ObservableCollection<Client>();
        public Client DataClient { get; set; } = new Client();
        readonly int RecordIndex;

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
        }

        /// <summary>
        /// Конструкто для отрытия лицевого счёта существующего клиента
        /// </summary>
        /// <param name="clients">Список клиентов</param>
        /// <param name="client">Данные выбранного клиента из списка</param>
        /// <param name="recordIndex">Индекс выбранного клиента из списка</param>
        public OpenAccountWindow(ObservableCollection<Client> clients, Client client, int recordIndex)
        {
            InitializeComponent();

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
            string Currency = ComboBoxCurrency.Text;
            string AccountType = ComboBoxAccountType.Text; //выбранное значение в ComboBox

            Client NewClient = new(Surname, FirstName, LastName, PasportData, AccountType,
                                   IsOpen, AccountStatus, NewRandomAccountNumber(ClientsList), ValueBalance, Currency);

            try
            {
                BankInfo.AddNewClient(ClientsList, NewClient);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }

        /// <summary>
        /// Генерация номера счёта клиента из двух частей
        /// </summary>
        /// <param name="clients">Список клиентов</param>
        /// <returns></returns>
        private static ObservableCollection<long> NewRandomAccountNumber(ObservableCollection<Client> clients)
        {
            Random random = new();
            ObservableCollection<long> NewAccountNumber = new();

            try
            {
                //Сгенерированый рандомный номер лицевого счёта
                long accountNumber = random.NextInt64();

                BankInfo.GetCheckClientAccountNumber(clients, accountNumber);
            }
            catch { }

            return NewAccountNumber;
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
            else OpenNewAccountNumberToExistingClient();
        }

        /// <summary>
        /// Открытие нового счёта существуещему клиенту
        /// </summary>
        private void OpenNewAccountNumberToExistingClient()
        {
            if (ClientsList != null && DataClient != null)
            {
                for (int i = 0; i < ClientsList.Count; i++)
                {
                    if (ClientsList[i].AccountsNumber == ClientsList[RecordIndex].AccountsNumber)
                    {
                        DataClient.AccountType = ComboBoxAccountType.Text;
                        //ClientsList[i].IsOpen = true;
                        //ClientsList[i].AccountStatus = "Открыт";
                        //ClientsList[i].AccountsNumber = NewRandomAccountNumber(ClientsList);
                        //ClientsList[i].ValueBalance = 0;
                        //ClientsList[i].Currency = ComboBoxCurrency.Text;

                        //ClientsList.RemoveAt(RecordIndex);
                        ClientsList.Insert(RecordIndex, DataClient);
                        break;
                        //BankInfo.SaveEditData(ClientsList);

                    }
                }
            }
        }

        #endregion
    }
}