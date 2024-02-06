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
                AccountsInfoOfSelectedClient();
            }
        }


        /// <summary>
        /// Выбор поля для изменения личных данных клиента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                if (ClientsList != null && !string.IsNullOrEmpty(SelectedData.ToString()))
                {
                    OpenAccountWindow OpenNewAccount = new(ClientsList, SelectedData);
                    OpenNewAccount.ShowDialog();
                }
                else { MessageBox.Show("Выберите клиента для открытия нового счёта", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Information); }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message, "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning); }
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
            try
            {
                if (ClientsList != null && !string.IsNullOrEmpty(SelectedData.ToString()))
                {
                    for (int i = 0; i < SelectedData.Account.Count; i++)
                    {
                        if (SelectedData.Account[i].Balance.Money != 0)
                        {
                            MessageBox.Show("Для закрытия счёта необходимо иметь нулевой баланс",
                                "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        if (SelectedData.Account[i].IsOpen == false)
                        {
                            MessageBox.Show("Счёт уже закрыт",
                                "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        if (ComboBoxAccounts.Text == "")
                        {
                            MessageBox.Show("Выберите лицевой счёт",
                                    "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        else
                        {
                            //Индекс выбранного клиента в общем списке
                            int RecordIndex = ClientsList.IndexOf(SelectedData);

                            ClientsList[RecordIndex].Account[i].IsOpen = false;

                            ClientsList.RemoveAt(RecordIndex);
                            ClientsList.Insert(RecordIndex, SelectedData);

                            BankInfo.SaveEditData(ClientsList);

                            MessageBox.Show("Счёт закрыт", "Информация", MessageBoxButton.OK,
                                MessageBoxImage.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message, "Внимание!", MessageBoxButton.OK, MessageBoxImage.Information); 
            }
        }

        /// <summary>
        /// Информация о лицевых счетах выбранного клиента
        /// </summary>
        private void AccountsInfoOfSelectedClient()
        {

            ObservableCollection<long> accountsOfSelectedClient = new ();

            for (int i = 0; i < SelectedData.Account.Count; i++)
                accountsOfSelectedClient.Add(SelectedData.Account[i].AccountNumber);

            ComboBoxAccounts.ItemsSource = accountsOfSelectedClient;
            ComboBoxAccounts.SelectedIndex = -1;
            textBlockType.Text = string.Empty;
            textBlockBalance.Text = string.Empty;
            textBlockCurrencyOfAccount.Text = string.Empty;
            textBlockStatusOfAccount.Text = string.Empty;
        }

        /// <summary>
        /// Вывод информации выбранного счета клиента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxAccounts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (ComboBoxAccounts.SelectedItem != null)
                {
                    object _comboBoxItem = ((ComboBox)sender).SelectedItem;
                    string choiseComboBoxAccount = _comboBoxItem.ToString(); //выбранное значение в ComboBox

                    for (int i = 0; i < SelectedData.Account.Count; i++)
                    {
                        if (choiseComboBoxAccount == SelectedData.Account[i].AccountNumber.ToString())
                        {
                            textBlockBalance.Text = SelectedData.Account[i].Balance.Money.ToString();
                            textBlockCurrencyOfAccount.Text = SelectedData.Account[i].CurrencyTypeClient.ToString();
                            //textBlockType.Text = SelectedData.Account[i].AccountTypeClient.ToString();

                            if (SelectedData.Account[i].AccountTypeClient == AccountType.Saving)
                                textBlockType.Text = "Сберегательный";
                            if (SelectedData.Account[i].AccountTypeClient == AccountType.Current)
                                textBlockType.Text = "Текущий";

                            if (SelectedData.Account[i].IsOpen == true)
                                textBlockStatusOfAccount.Text = "Открыт";
                            if (SelectedData.Account[i].IsOpen == false)
                                textBlockStatusOfAccount.Text = "Закрыт";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Кнопка "Пополнить баланс"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonReplenish_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ComboBoxAccounts.Text == "")
                {
                    MessageBox.Show("Выберите лицевой счёт",
                            "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (ClientsList != null && !string.IsNullOrEmpty(SelectedData.ToString()))
                {
                    TransferWindow transfer = new(ClientsList, SelectedData, long.Parse(ComboBoxAccounts.Text),
                        ButtonReplenish.Content.ToString());
                    transfer.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Кнопка "Перевести"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonTransfer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ClientsList != null && !string.IsNullOrEmpty(SelectedData.ToString()))
                {
                    for (int i = 0; i < SelectedData.Account.Count; i++)
                    {
                        if (ComboBoxAccounts.Text == "")
                        {
                            MessageBox.Show("Выберите лицевой счёт",
                                    "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        if (SelectedData.Account[i].AccountTypeClient.ToString() == "Current" && SelectedData.Account[i].IsOpen == true
                            && SelectedData.Account[i].Balance.Money != 0)
                        {
                            TransferWindow transfer = new(ClientsList, SelectedData, ButtonTransfer.Content.ToString());
                            transfer.ShowDialog();
                        }
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
