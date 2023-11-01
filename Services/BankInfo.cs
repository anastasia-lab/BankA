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
        public ObservableCollection<Client> ClientBankAccount { get; set; }

        #endregion

        #region Конструктор
        public BankInfo()
        {
            ClientBankAccount = new ObservableCollection<Client>();
        }
        #endregion

        #region Методы

        /// <summary>
        /// Получение списка клиентов
        /// </summary>
        /// <returns> Возвращает список клиентов из файла </returns>
        public ObservableCollection<Client> GetListClients()
        {
            DataBase.ReadXmlFile("clients.xml", ClientBankAccount);
            return ClientBankAccount;
        }

        /// <summary>
        /// Добавление в коллекцию нового клиента
        /// </summary>
        /// <param name="client"> Новый клиент </param>
        public void AddNewClient(ObservableCollection<Client> Clients,Client client)
        {
            Clients.Add(client);
            DataBase.SaveXmlFile(ClientBankAccount);
        }

        /// <summary>
        /// Проверка на совпадение нового счёта
        /// </summary>
        /// <param name="clientAccountNumber"> Новый счёт клиента </param>
        /// <returns> Возаращает номер счёта клиента </returns>
        /// <exception cref="ArgumentException"></exception>
        public long GetCheckClientAccountNumber(long clientAccountNumber)
        {
            GetListClients();

            for (int i = 0; i < ClientBankAccount.Count; i++)
            {
                if (clientAccountNumber == ClientBankAccount[i].AccountNumber)
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
        public static void SaveEditData(bool isOpen, ObservableCollection<Client> ClientBankAccount, int index)
        {
            ClientBankAccount[index].IsOpen = isOpen;
            DataBase.SaveXmlFile(ClientBankAccount);
        }

        #endregion
    }
}
