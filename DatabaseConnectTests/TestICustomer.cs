using System;
using Interfaces;
using Interfaces.Implementation;
using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace DatabaseConnect
{
    [TestFixture]
    public abstract class TestICustomer
    {

        public abstract IDatabaseService GetDatabaseServiceInstance();

        [Test]
        public void TestCustomerInsert()
        {

            IDatabaseService databaseService = GetDatabaseServiceInstance();
            int cusId = databaseService.CustomerService.Save( new Customer() { Name = "Test" ,Description= "Przypadek testowy",Active = true});
            var Customer = databaseService.CustomerService.GetCustomerById(cusId);
            Assert.AreEqual("Test", Customer.Name);
            Assert.AreEqual("Przypadek testowy", Customer.Description);
            Assert.AreEqual(true, Customer.Active);

        }
        [Test]
        public void TestCustomerUpdate()
        {
            IDatabaseService databaseService = new DatabaseService();
            int cusId = databaseService.CustomerService.Save(new Customer() { Name = "Test", Description = "Przypadek testowy", Active = true });
            ICustomer Customer = databaseService.CustomerService.GetCustomerById(cusId);
            Customer.Name = "Test Updated";
            Customer.Description += " Updated";
            Customer.Active = false;
            databaseService.CustomerService.Save(Customer);
            ICustomer CustomerUpdated = databaseService.CustomerService.GetCustomerById(cusId);
            Assert.AreEqual("Test Updated", CustomerUpdated.Name);
            Assert.AreEqual("Przypadek testowy Updated", CustomerUpdated.Description);
            Assert.AreEqual(false, CustomerUpdated.Active);
        }

        [Test]
        public void TestCustomerDelete()
        {
            IDatabaseService databaseService = GetDatabaseServiceInstance();
            int cusId = databaseService.CustomerService.Save(new Customer() { Name = "Test", Description = "Przypadek testowy", Active = true });
            var Customer = databaseService.CustomerService.GetCustomerById(cusId);
            Assert.AreNotEqual(null, Customer);
            databaseService.CustomerService.Delete(cusId);
            Customer = databaseService.CustomerService.GetCustomerById(cusId);
            Assert.AreEqual(null, Customer);
        }

        [Test]
        public void TestCustomerFilterActive()
        {
            IDatabaseService databaseService = GetDatabaseServiceInstance();
            int cusId = databaseService.CustomerService.Save(new Customer() { Name = "Test", Description = "Przypadek testowy", Active = true });
            IList<ICustomer> Customers = databaseService.CustomerService.GetCustomers(new CustomerFilter() { Active =true});
            Assert.IsTrue(Customers.Any(x => x.Id == cusId));
            var Customer = databaseService.CustomerService.GetCustomerById(cusId);
            Customer.Active = false;
            cusId= databaseService.CustomerService.Save(Customer);
            Customers = databaseService.CustomerService.GetCustomers(new CustomerFilter() { Active = true });
            Assert.IsFalse(Customers.Any(x => x.Id == cusId));
        }
        [Test]
        public void TestCustomerFilterName()
        {
            IDatabaseService databaseService = GetDatabaseServiceInstance();
            int cusId = databaseService.CustomerService.Save(new Customer() { Name = "Test", Description = "Przypadek testowy", Active = true });
            IList<ICustomer> Customers = databaseService.CustomerService.GetCustomers(new CustomerFilter() { Name ="Test" });
            Assert.IsTrue(Customers.Any(x => x.Id == cusId));
            var Customer = databaseService.CustomerService.GetCustomerById(cusId);
            Customer.Name = "Testtt";
            cusId = databaseService.CustomerService.Save(Customer);
            Customers = databaseService.CustomerService.GetCustomers(new CustomerFilter() { Name = "Test" });
            Assert.IsFalse(Customers.Any(x => x.Id == cusId));
        }

        [TearDown]
        public void TearDown()
        {
            IDatabaseService databaseService = GetDatabaseServiceInstance();
            databaseService.CustomerService.ClearTables();
        }

    }
}

