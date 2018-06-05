using System;
using Interfaces;
using Interfaces.Implementation;
using NUnit.Framework;
namespace DatabaseConnect
{
    [TestFixture]
    public class TestTransactionType
    {

        [Test]
        public void TestInsert()
        {
            IDatabaseService databaseService = new DatabaseService();
            databaseService.ConnectionString = DbTestUtils.ConnectionString;
            int cusId = databaseService.TransactionTypeService.Save( new TransactionType() { Name = "test2" ,Description= "sad",Color = "Zielony"});
            var cus = databaseService.TransactionTypeService.GetTransactionTypeById(cusId);
            Assert.AreEqual("test2", cus.Name);
            var customers = databaseService.TransactionTypeService.GetTransactionTypes(new TransactionTypeFilter() {Id = cusId ,Name = "test2" });
            Assert.AreEqual("test2", customers[0].Name);

        }
        [Test]
        public void TestUpdate()
        {
            IDatabaseService databaseService = new DatabaseService();
            databaseService.ConnectionString = DbTestUtils.ConnectionString;
            int cusId = databaseService.TransactionTypeService.Save(new TransactionType() { Name = "test2222", Description = "sad", Color = "Zielony" });
            var cus = databaseService.TransactionTypeService.GetTransactionTypeById(cusId);
            Assert.AreEqual("test2222", cus.Name);
        }

        [Test]
        public void TestSelect()
        {
            IDatabaseService databaseService = new DatabaseService();
            databaseService.ConnectionString = DbTestUtils.ConnectionString;
            int cusId = databaseService.TransactionTypeService.Save(new TransactionType() { Name = "test2222", Description = "sad", Color = "Zielony" });
            var cus = databaseService.TransactionTypeService.GetTransactionTypeById(cusId);
            Assert.AreEqual("test2222", cus.Name);
        }


        [Test]
        public void TestDelete()
        {
            IDatabaseService databaseService = new DatabaseService();
            databaseService.ConnectionString = DbTestUtils.ConnectionString;
            int cusId = databaseService.TransactionTypeService.Save(new TransactionType() { Name = "test2222", Description = "sad", Color = "Zielony" });
            var cus = databaseService.TransactionTypeService.GetTransactionTypeById(cusId);
            Assert.AreEqual("test2222", cus.Name);
            databaseService.TransactionTypeService.Delete(cusId);
            var cusAfterDelete = databaseService.TransactionTypeService.GetTransactionTypeById(cusId);
            Assert.AreEqual(null, cusAfterDelete);
        }

        [TearDown]
        public void TearDown()
        {
            DbTestUtils.ClearTables("TransactionType");
        }

    }
}

