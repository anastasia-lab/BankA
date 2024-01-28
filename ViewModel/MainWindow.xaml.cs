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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static BankA.Services.Interfaces;

namespace BankA.ViewModel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Объявление переменных

        //Данные выбранного клиента
        public Client SelectedData { get; set; } = new Client();
        public string SelectedButtonContent { get; } = string.Empty;

        //Список всех клиентов
        public ObservableCollection<Client> ClientsList { get; set; } = new ObservableCollection<Client>();
        
        #endregion

        #region Конструктор
        public MainWindow()
        {
            InitializeComponent();

            LoadDataInDataView();
        }

        #endregion

        #region Методы

        /// <summary>
        /// Загрузка списка клиентов в DataGrid
        /// </summary>
        private void LoadDataInDataView()
        {
            DataGridListPerson.ItemsSource = BankInfo.GetListClients(ClientsList);
            //LableInfo.Content = "Количество клиентов: " + ClientsList.Count;
        }

        /// <summary>
        /// Выбор клиента из списка DataGrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridListPerson_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridListPerson.SelectedItem is Client selected)
            {
                SelectedData = selected;
            }

            AccountsInfoOfSelectedClient();
        }

        private void CheckBoxChangeData_Checked(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Кнопка "Открыть счёт"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonOpenNewAccount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ClientsList != null && SelectedData != null)
                {   
                    //Индекс выбранного клиента в общем списке
                    int RecordIndex = ClientsList.IndexOf(SelectedData);

                    OpenAccountWindow OpenNewAccount = new(ClientsList, SelectedData, RecordIndex);
                    OpenNewAccount.ShowDialog();
                    this.Close();
                }
            }
            catch { }
        }

        /// <summary>
        /// Кнопка "Добавить клиента"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonAddNewClient_Click(object sender, RoutedEventArgs e)
        {
            OpenAccountWindow OpenToCreateNewClient = new(ClientsList);
            OpenToCreateNewClient.ShowDialog();
        }

        /// <summary>
        /// Кнопка "Закрыть счёт"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonCloseAccount_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    if (ClientsList != null && SelectedData != null)
            //    {
            //        if (SelectedData.ValueBalance != 0)
            //        {
            //            MessageBox.Show("Для закрытия счёта необходимо иметь нулевой баланс",
            //                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            //            return;
            //        }
            //        if (SelectedData.IsOpen == false)
            //        {
            //            MessageBox.Show("Счёт уже закрыт",
            //                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            //            return;
            //        }
            //        if (ComboBoxAccounts.Text == "")
            //        {
            //            MessageBox.Show("Выберите лицевой счёт",
            //                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            //            return;
            //        }
            //        else
            //        {
            //            //Индекс выбранного клиента в общем списке
            //            int RecordIndex = ClientsList.IndexOf(SelectedData);

            //            try
            //            {
            //                if (ClientsList != null && SelectedData != null)
            //                {
            //                    for (int i = 0; i < ClientsList.Count; i++)
            //                    {
            //                        if (ClientsList[i].AccountsNumber == ClientsList[RecordIndex].AccountsNumber)
            //                        {
            //                            ClientsList[RecordIndex].IsOpen = false;
            //                            ClientsList[RecordIndex].AccountStatus = "Закрыт";

            //                            ClientsList.RemoveAt(RecordIndex);
            //                            ClientsList.Insert(RecordIndex, SelectedData);

            //                            BankInfo.SaveEditData(ClientsList);
            //                        }
            //                    }

            //                    MessageBox.Show("Счёт закрыт", "Информация", MessageBoxButton.OK, 
            //                        MessageBoxImage.Information);
            //                    return;
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                MessageBox.Show(ex.Message, "Внимание!", MessageBoxButton.OK, MessageBoxImage.Information);
            //            }
            //        }
            //    }
            //}
            //catch { }
        }

        /// <summary>
        /// Информация о лицевых счетах выбранного клиента
        /// </summary>
        private void AccountsInfoOfSelectedClient()
        {
            List<long> accounts = new List<long>
            {
                SelectedData.Account.AccountNumber
            };
            ComboBoxAccounts.ItemsSource = accounts;
            textBlockType.Text = string.Empty;
            textBlockBalance.Text = string.Empty;
            textBlockCurrencyOfAccount.Text = string.Empty;
        }

        /// <summary>
        /// Вывод информации выбранного счета клиента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxAccounts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            if (cmb == null)
                return;

            textBlockType.Text = SelectedData.Account.Type.ToString();
            textBlockBalance.Text = SelectedData.Account.Balance.Money.ToString();
            textBlockCurrencyOfAccount.Text = SelectedData.Account.CurrencyType.ToString();
        }

        #endregion
    }
}
