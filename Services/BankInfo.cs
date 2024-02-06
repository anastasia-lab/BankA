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

        // количество денег на счету клиента
        public long Money { get; set; }

        #endregion

        #region Конструктор
        public BankInfo()
        {
            
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
        /// Проверка нового л/с на совпадение с существующими у клиентов
        /// </summary>
        /// <param name="clientAccountNumber"> Новый счёт клиента </param>
        /// <param name="ClientsList"> Список клиентов </param>
        /// <returns> Возаращает номер счёта клиента </returns>
        /// <exception cref="ArgumentException"></exception>
        public static decimal GetCheckClientAccountNumber(ObservableCollection<Client> ClientsList, long clientAccountNumber)
        {

            for (int i = 0; i < ClientsList.Count; i++)
            {
                for(int j = 0; j < ClientsList[i].Account.Count; j++)
                    if (clientAccountNumber.ToString() == ClientsList[i].Account[j].AccountNumber.ToString())
                        throw new ArgumentException("Ошибка в формировании счёта. Попробуйте еще раз.");
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
