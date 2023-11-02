using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankA.DataBases;


namespace BankA.Services
{
    public class BankInfo
    {
        #region Объявление переменных и свойств

        // получение значения на счету клиента
        public long Money { get; set; }

        // коллекция для хранения списка клиентов
        //public ObservableCollection<Client> ClientBankAccount { get; set; }

        #endregion

        #region Конструктор
        public BankInfo()
        {
            //ClientBankAccount = new ObservableCollection<Client>();
        }
        #endregion

        #region Методы

        /// <summary>
        /// Получение списка клиентов
        /// </summary>
        /// <returns> Возвращает список клиентов из файла </returns>
        public static ObservableCollection<Client> GetListClients(ObservableCollection<Client> ClientsList)
        {
            DataBase.ReadXmlFile("clients.xml", ClientsList);
            return ClientsList;
        }

        /// <summary>
        /// Добавление в коллекцию нового клиента
        /// </summary>
        /// <param name="client"> Новый клиент </param>
        /// <param name="ClientsList"> Список клиентов </param>
        public static void AddNewClient(ObservableCollection<Client> ClientsList,Client client)
        {
            ClientsList.Add(client);
            DataBase.SaveXmlFile(ClientsList);
        }

        /// <summary>
        /// Проверка нового счёта на совпадение с существующими
        /// </summary>
        /// <param name="clientAccountNumber"> Новый счёт клиента </param>
        /// <param name="ClientsList"> Список клиентов </param>
        /// <returns> Возаращает номер счёта клиента </returns>
        /// <exception cref="ArgumentException"></exception>
        public static decimal GetCheckClientAccountNumber(ObservableCollection<Client> ClientsList, decimal clientAccountNumber)
        {
            GetListClients(ClientsList);

            for (int i = 0; i < ClientsList.Count; i++)
            {
                if (clientAccountNumber == ClientsList[i].AccountNumber)
                    throw new ArgumentException("Кажется, такой номер счёта уже есть.");
            }

            return clientAccountNumber;
        }

        /// <summary>
        /// Сохранение изменненых данных
        /// </summary>
        /// <param name="isOpen"> Проверка на открытие счёта в банке</param>
        /// <param name="ClientBankAccount"> Список клиентов банка </param>
        /// <param name="index">Индекс клиента в списке </param>
        public static void SaveEditData(ObservableCollection<Client> Clients)
        {
            DataBase.SaveXmlFile(Clients);
        }

        #endregion
    }
}
