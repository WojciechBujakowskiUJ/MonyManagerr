using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Interfaces.Implementation;
using NUnit.Framework;
namespace DatabaseConnect
{
    [TestFixture]
    public abstract class TestITransaction
    {
        public abstract IDatabaseService GetDatabaseServiceInstance();

        private int CreateTestCustomer()
        {
            IDatabaseService databaseService = GetDatabaseServiceInstance();
            return databaseService.CustomerService.Save(new Customer() { Name = "Test", Description = "Przypadek testowy", Active = true });
        }

        private int CreateTestTransactionType()
        {
            IDatabaseService databaseService = GetDatabaseServiceInstance();
            return databaseService.TransactionTypeService.Save(new TransactionType() { Name = "Test", Description = "Przypadek testowy", Income = true,Color = "Red" });
        }

        [Test]
        public void TestTransactionInsert()
        {
            IDatabaseService databaseService = GetDatabaseServiceInstance();
            int CustomerId = CreateTestCustomer();
            int TransactionTypeId = CreateTestTransactionType();
            int TransactionId = databaseService.TransactionService.Save(new Transaction() { Name = "Test", Description = "Przypadek testowy",   CustomerId = CustomerId, Date = DateTime.Now ,Value=10,TransactionTypeId= TransactionTypeId });
            var Transaction = databaseService.TransactionService.GetTransactionById(TransactionId);
            Assert.AreEqual("Test", Transaction.Name);
            Assert.AreEqual("Przypadek testowy", Transaction.Description);
            Assert.AreEqual(CustomerId, Transaction.CustomerId);
            Assert.AreEqual(TransactionTypeId, Transaction.TransactionTypeId);
            Assert.AreEqual(10, Transaction.Value);
        }
        [Test]
        public void TestTransactionUpdate()
        {
            IDatabaseService databaseService = GetDatabaseServiceInstance();
            int CustomerId = CreateTestCustomer();
            int TransactionTypeId = CreateTestTransactionType();
            int TransactionId = databaseService.TransactionService.Save(new Transaction() { Name = "Test", Description = "Przypadek testowy", CustomerId = CustomerId, Date = DateTime.Now, Value = 10, TransactionTypeId = TransactionTypeId });
            var Transaction = databaseService.TransactionService.GetTransactionById(TransactionId);
            Transaction.Name = "Test Updated";
            Transaction.Description += " Updated";
            Transaction.Value = 20;
            databaseService.TransactionService.Save(Transaction);
            Transaction = databaseService.TransactionService.GetTransactionById(TransactionId);
            Assert.AreEqual("Test Updated", Transaction.Name);
            Assert.AreEqual("Przypadek testowy Updated", Transaction.Description);
            Assert.AreEqual(20, Transaction.Value);
        }

        [Test]
        public void TestTransactionDelete()
        {
            IDatabaseService databaseService = GetDatabaseServiceInstance();
            int CustomerId = CreateTestCustomer();
            int TransactionTypeId = CreateTestTransactionType();
            int TransactionId = databaseService.TransactionService.Save(new Transaction() { Name = "Test", Description = "Przypadek testowy", CustomerId = CustomerId, Date = DateTime.Now, Value = 10, TransactionTypeId = TransactionTypeId });
            var Transaction = databaseService.TransactionService.GetTransactionById(TransactionId);
            Assert.AreNotEqual(null, Transaction);
            databaseService.TransactionService.Delete(TransactionId);
            Transaction = databaseService.TransactionService.GetTransactionById(TransactionId);
            Assert.AreEqual(null, Transaction);
        }

        [Test]
        public void TestTransactionFilterDate()
        {
            IDatabaseService databaseService = GetDatabaseServiceInstance();
            int CustomerId = CreateTestCustomer();
            int TransactionTypeId = CreateTestTransactionType();
            int TransactionId = databaseService.TransactionService.Save(new Transaction() { Name = "Test", Description = "Przypadek testowy", CustomerId = CustomerId, Date = DateTime.Now, Value = 10, TransactionTypeId = TransactionTypeId });
            var Transactions = databaseService.TransactionService.GetTransactions(new TransactionFilter() { DateMax = DateTime.Now.AddHours(1) });
            Assert.IsTrue(Transactions.Any(x => x.Id == TransactionId));
            Transactions = databaseService.TransactionService.GetTransactions(new TransactionFilter() { DateMin = DateTime.Now.AddHours(-1) });
            Assert.IsTrue(Transactions.Any(x => x.Id == TransactionId));
            Transactions = databaseService.TransactionService.GetTransactions(new TransactionFilter() { DateMax = DateTime.Now.AddMinutes(1), DateMin = DateTime.Now.AddMinutes(-1) });
            Assert.IsTrue(Transactions.Any(x => x.Id == TransactionId));
            Transactions = databaseService.TransactionService.GetTransactions(new TransactionFilter() { DateMax = DateTime.Now.AddMinutes(-1) });
            Assert.IsFalse(Transactions.Any(x => x.Id == TransactionId));
           
        }
        [Test]
        public void TestTransactionFilterValue()
        {
            IDatabaseService databaseService = GetDatabaseServiceInstance();
            int CustomerId = CreateTestCustomer();
            int TransactionTypeId = CreateTestTransactionType();
            int TransactionId = databaseService.TransactionService.Save(new Transaction() { Name = "Test", Description = "Przypadek testowy", CustomerId = CustomerId, Date = DateTime.Now, Value = 10, TransactionTypeId = TransactionTypeId });
            var Transactions = databaseService.TransactionService.GetTransactions(new TransactionFilter() { ValueMax = 11 });
            Assert.IsTrue(Transactions.Any(x => x.Id == TransactionId));
            Transactions = databaseService.TransactionService.GetTransactions(new TransactionFilter() { ValueMin = 9 });
            Assert.IsTrue(Transactions.Any(x => x.Id == TransactionId));
            Transactions = databaseService.TransactionService.GetTransactions(new TransactionFilter() { ValueMax = 13, ValueMin =1 });
            Assert.IsTrue(Transactions.Any(x => x.Id == TransactionId));
            Transactions = databaseService.TransactionService.GetTransactions(new TransactionFilter() { ValueMax = 1 });
            Assert.IsFalse(Transactions.Any(x => x.Id == TransactionId));

        }

        [TearDown]
        public void TearDown()
        {
            IDatabaseService databaseService = GetDatabaseServiceInstance();
            databaseService.TransactionService.ClearTables();
            databaseService.TransactionTypeService.ClearTables();
            databaseService.CustomerService.ClearTables();

        }
    }
}

