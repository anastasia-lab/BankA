using BankA.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace BankA.DataBases
{
    internal class DataBase
    {
        /// <summary>
        /// Чтение данных из xml-файла
        /// </summary>
        /// <param name="path"> Путь к файлу </param>
        /// <param name="clientsList"> Список клиентов </param>
        /// <returns></returns>
        public static ObservableCollection<Client> ReadXmlFile(string path, ObservableCollection<Client> clientsList)
        {
            try
            {
                XmlSerializer xmlSerializer = new(typeof(Client[]));
                using FileStream reader = new(path, FileMode.OpenOrCreate);
                if (xmlSerializer.Deserialize(reader) is Client[] clients)
                {
                    foreach (Client client in clients)
                    {
                        for (int i = 0; i < 1; i++)
                        {
                            clients[i] = new Client
                            {
                                Surname = client.Surname,
                                FirstName = client.FirstName,
                                LastName = client.LastName,
                                PasportData = client.PasportData,
                                AccountType = client.AccountType,
                                IsOpen = client.IsOpen,
                                AccountStatus = client.AccountStatus,
                                AccountNumber = client.AccountNumber,
                                ValueBalance = client.ValueBalance,
                                Currency = client.Currency,
                            };

                            clientsList.Add(clients[i]);
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning); }

            return clientsList;
        }

        /// <summary>
        /// Сохранение данных в xml-файл
        /// </summary>
        /// <param name="clientsList"> Список клиентов </param>
        public static void SaveXmlFile(ObservableCollection<Client> clientsList)
        {
            try
            {
                XmlSerializer xmlSerializer = new(typeof(Client[]));
                Client[] clients = clientsList.ToArray();


                using FileStream fs = new("clients.xml", FileMode.OpenOrCreate);
                xmlSerializer.Serialize(fs, clients);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}
