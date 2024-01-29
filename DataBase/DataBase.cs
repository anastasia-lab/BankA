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
                            clients[i] = new();
                            clients[i].Surname = client.Surname;
                            clients[i].FirstName = client.FirstName;
                            clients[i].LastName = client.LastName;
                            clients[i].PasportData = client.PasportData;
                            clients[i].Account = client.Account;

                            clientsList.Add(clients[i]);
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning); }

            return clientsList;
        }

        /// <summary>
        /// Сохранение списка клиентов в xml-файл
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
