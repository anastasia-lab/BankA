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

namespace BankA.ViewModel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Объявление переменных

        readonly BankInfo bank;

        //Данные выбранного клиента
        private Client SelectedData { get; set; } = new Client();
        public string SelectedButtonText { get; } = string.Empty;
        private ObservableCollection<Client> Clients { get; set; } = new ObservableCollection<Client>();

        #endregion

        #region Конструктор
        public MainWindow()
        {
            InitializeComponent();

            bank = new BankInfo();
            LoadDataInDataView();
        }

        #endregion

        #region Методы

        /// <summary>
        /// Загрузка списка клиентов в DataGrid
        /// </summary>
        private void LoadDataInDataView()
        {
            DataGridListPerson.ItemsSource = bank.GetListClients();
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
                Clients.Add(SelectedData);
                ShowAccountSelectedClientInListView();
            }
        }

        private void CheckBoxChangeData_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        /// <summary>
        /// Вывод лицевых счётов выбранного клиента в ListView
        /// </summary>
        private void ShowAccountSelectedClientInListView()
        {
            ObservableCollection<Client> DataSelectedClient = new()
            {
                SelectedData
            };

            ListViewAccounts.ItemsSource = DataSelectedClient;
        }

        #endregion
    }
}
