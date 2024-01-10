﻿using BankA.Services;
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

        //readonly BankInfo bank;

        //Данные выбранного клиента
        private Client SelectedData { get; set; } = new Client();
        public string SelectedButtonText { get; } = string.Empty;
        private ObservableCollection<Client> ClientsList { get; set; } = new ObservableCollection<Client>();
        private ObservableCollection<Client> DataSelectedClient { get; set; } = new ObservableCollection<Client>();
        #endregion

        #region Конструктор
        public MainWindow()
        {
            InitializeComponent();

            //bank = new BankInfo();
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
            LableInfo.Content = "Количество клиентов: " + DataGridListPerson.Items.Count;
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


        private void ButtonOpenNewAccount_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Кнопка "Добавить клиента" на вкладке "Информация"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonAddNewClient_Click(object sender, RoutedEventArgs e)
        {
            Open_CloseAccountWindow OpenToCreateNewClient = new(ClientsList);
            OpenToCreateNewClient.ShowDialog();
        }

        /// <summary>
        /// Кнопка "Закрыть счёт" на вкладке "Информация"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonCloseAccount_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsList != null && SelectedData != null)
            {
                int RecordIndex = ClientsList.IndexOf(SelectedData);
                Open_CloseAccountWindow CloseAccount = new(ClientsList, SelectedData, RecordIndex);
                CloseAccount.ShowDialog();
            }
        }

        /// <summary>
        /// Информация о лицевых счетах выбранного клиента
        /// </summary>
        private void AccountsInfoOfSelectedClient()
        {
            ComboBoxAccounts.ItemsSource = SelectedData.AccountsNumber;
            textBlockType.Text = string.Empty;
            textBlockBalance.Text = string.Empty;
            textBlockCurrencyOfAccount.Text= string.Empty;
            textBlockStatusOfAccount.Text= string.Empty;
        }


        private void ComboBoxAccounts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            if (cmb == null)
                return;

            textBlockType.Text = SelectedData.AccountType;
            textBlockBalance.Text = SelectedData.ValueBalance.ToString();
            textBlockCurrencyOfAccount.Text = SelectedData.Currency;
            textBlockStatusOfAccount.Text = SelectedData.AccountStatus;
        }

        #endregion
    }
}
