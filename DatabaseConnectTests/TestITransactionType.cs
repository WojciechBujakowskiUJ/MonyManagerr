using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Interfaces.Implementation;
using NUnit.Framework;
namespace DatabaseConnect
{
    [TestFixture]
    public abstract class TestITransactionType
    {
        public abstract IDatabaseService GetDatabaseServiceInstance();

        [Test]
        public void TestTransactionTypeInsert()
        {

            IDatabaseService databaseService = GetDatabaseServiceInstance();
            int TransactionTypeId = databaseService.TransactionTypeService.Save(new TransactionType() { Name = "Test", Description = "Przypadek testowy", Color ="Red" ,Income = true});
            var TransactionType = databaseService.TransactionTypeService.GetTransactionTypeById(TransactionTypeId);
            Assert.AreEqual("Test", TransactionType.Name);
            Assert.AreEqual("Przypadek testowy", TransactionType.Description);
            Assert.AreEqual("Red", TransactionType.Color);
            Assert.AreEqual(true, TransactionType.Income);
        }
        [Test]
        public void TestTransactionTypeUpdate()
        {
            IDatabaseService databaseService = GetDatabaseServiceInstance();
            int TransactionTypeId = databaseService.TransactionTypeService.Save(new TransactionType() { Name = "Test", Description = "Przypadek testowy", Color = "Red", Income = true });
            var TransactionType = databaseService.TransactionTypeService.GetTransactionTypeById(TransactionTypeId);
            TransactionType.Name = "Test Updated";
            TransactionType.Description += " Updated";
            TransactionType.Color = "Green";
            TransactionType.Income = false;
            databaseService.TransactionTypeService.Save(TransactionType);
            TransactionType = databaseService.TransactionTypeService.GetTransactionTypeById(TransactionTypeId);
            Assert.AreEqual("Test Updated", TransactionType.Name);
            Assert.AreEqual("Przypadek testowy Updated", TransactionType.Description);
            Assert.AreEqual("Green", TransactionType.Color);
            Assert.AreEqual(false, TransactionType.Income);

        }

        [Test]
        public void TestTransactionTypeDelete()
        {
            IDatabaseService databaseService = GetDatabaseServiceInstance();
            int TransactionTypeId = databaseService.TransactionTypeService.Save(new TransactionType() { Name = "Test", Description = "Przypadek testowy", Color = "Red", Income = true });
            var TransactionType = databaseService.TransactionTypeService.GetTransactionTypeById(TransactionTypeId);
            Assert.AreNotEqual(null, TransactionType);
            databaseService.TransactionTypeService.Delete(TransactionTypeId);
            TransactionType = databaseService.TransactionTypeService.GetTransactionTypeById(TransactionTypeId);
            Assert.AreEqual(null, TransactionType);
        }

        [Test]
        public void TestTransactionTypeFilterIncome()
        {
            IDatabaseService databaseService = GetDatabaseServiceInstance();
            int TransactionTypeId = databaseService.TransactionTypeService.Save(new TransactionType() { Name = "Test", Description = "Przypadek testowy", Color = "Red", Income = true });
            var TransactionTypes = databaseService.TransactionTypeService.GetTransactionTypes(new TransactionTypeFilter() { Income = true });
            Assert.IsTrue(TransactionTypes.Any(x => x.Id == TransactionTypeId));
            var TransactionType = databaseService.TransactionTypeService.GetTransactionTypeById(TransactionTypeId);
            TransactionType.Income = false;
            TransactionTypeId = databaseService.TransactionTypeService.Save(TransactionType);
            TransactionTypes = databaseService.TransactionTypeService.GetTransactionTypes(new TransactionTypeFilter() { Income = true });
            Assert.IsFalse(TransactionTypes.Any(x => x.Id == TransactionTypeId));
        }
        [Test]
        public void TestTransactionTypeFilterName()
        {
            IDatabaseService databaseService = GetDatabaseServiceInstance();
            int TransactionTypeId = databaseService.TransactionTypeService.Save(new TransactionType() { Name = "Test", Description = "Przypadek testowy", Color = "Red", Income = true });
            var TransactionTypes = databaseService.TransactionTypeService.GetTransactionTypes(new TransactionTypeFilter() { Name = "Test" });
            Assert.IsTrue(TransactionTypes.Any(x => x.Id == TransactionTypeId));
            var TransactionType = databaseService.TransactionTypeService.GetTransactionTypeById(TransactionTypeId);
            TransactionType.Name = "TestUpdated";
            TransactionTypeId = databaseService.TransactionTypeService.Save(TransactionType);
            TransactionTypes = databaseService.TransactionTypeService.GetTransactionTypes(new TransactionTypeFilter() { Name = "Test" });
            Assert.IsFalse(TransactionTypes.Any(x => x.Id == TransactionTypeId));
        }
        [Test]
        public void TestTransactionTypeFilterColor()
        {
            IDatabaseService databaseService = GetDatabaseServiceInstance();
            int TransactionTypeId = databaseService.TransactionTypeService.Save(new TransactionType() { Name = "Test", Description = "Przypadek testowy", Color = "Red", Income = true });
            var TransactionTypes = databaseService.TransactionTypeService.GetTransactionTypes(new TransactionTypeFilter() { Color = "Red" });
            Assert.IsTrue(TransactionTypes.Any(x => x.Id == TransactionTypeId));
            var TransactionType = databaseService.TransactionTypeService.GetTransactionTypeById(TransactionTypeId);
            TransactionType.Color = "Black";
            TransactionTypeId = databaseService.TransactionTypeService.Save(TransactionType);
            TransactionTypes = databaseService.TransactionTypeService.GetTransactionTypes(new TransactionTypeFilter() { Color = "Red" });
            Assert.IsFalse(TransactionTypes.Any(x => x.Id == TransactionTypeId));
        }

        [TearDown]
        public void TearDown()
        {
            IDatabaseService databaseService = GetDatabaseServiceInstance();
            databaseService.TransactionTypeService.ClearTables();
        }
    }
}

