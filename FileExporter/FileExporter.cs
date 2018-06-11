using Interfaces;
using Interfaces.Implementation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExporter
{
    public static class FileExporter
    {

        private static T GetObjectFromFile<T>(string filePath)
        {
            string json = File.ReadAllText(filePath);
            T result = JsonConvert.DeserializeObject<T>(json);
            return result;
        }


        public static List<Customer> ImportCustomer(string filePath)
        {
            return GetObjectFromFile<List<Customer>>(filePath);
        }
        public static void ExportCustomer(string filePath, List<ICustomer> Customers)
        {
            File.WriteAllText(filePath, JsonConvert.SerializeObject(Customers));
        }

        public static List<Transaction> ImportTransaction(string filePath)
        {
            return GetObjectFromFile<List<Transaction>>(filePath);
        }
        public static void ExportTransaction(string filePath, List<Transaction> Transactions)
        {
            File.WriteAllText(filePath, JsonConvert.SerializeObject(Transactions));
        }

        public static List<TransactionType> ImportTransactionType(string filePath)
        {
            return GetObjectFromFile<List<TransactionType>>(filePath);
        }
        public static void ExportTransactionType(string filePath, List<TransactionType> TransactionTypes)
        {
            File.WriteAllText(filePath, JsonConvert.SerializeObject(TransactionTypes));
        }
    }
}
