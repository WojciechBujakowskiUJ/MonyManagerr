using System;
using Interfaces;
using Interfaces.Implementation;
using NUnit.Framework;
namespace DatabaseConnect
{
    [TestFixture]
    public class TestCustomer
    {
        [Test]
        public void TestInsert()
        {
            IDatabaseService databaseService = new DatabaseService();
            databaseService.ConnectionString = @"Data Source=SYLWIA\SQLEXPRESS;Initial Catalog=Test;Integrated Security=SSPI;";
            int cusId = databaseService.CustomerService.Save( new Customer() { Name = "test2" ,Description= "sad"});
            var cus = databaseService.CustomerService.GetCustomerById(cusId);
            Assert.AreEqual("test2", cus.Name);
            var customers = databaseService.CustomerService.GetCustomers(new CustomerFilter() {Id = cusId ,Name = "test2" });
            Assert.AreEqual("test2", customers[0].Name);

        }
        [Test]
        public void TestUpdate()
        {
            IDatabaseService databaseService = new DatabaseService();
            databaseService.ConnectionString = @"Data Source=SYLWIA\SQLEXPRESS;Initial Catalog=Test;Integrated Security=SSPI;";
            int cusId = databaseService.CustomerService.Save(new Customer() {Id=12, Name = "test2222", Description = "sad" });
            var cus = databaseService.CustomerService.GetCustomerById(cusId);
            Assert.AreEqual("test2222", cus.Name);
        }

        [Test]
        public void TestSelect()
        {
            IDatabaseService databaseService = new DatabaseService();
            databaseService.ConnectionString = @"Data Source=SYLWIA\SQLEXPRESS;Initial Catalog=Test;Integrated Security=SSPI;";
            int cusId = databaseService.CustomerService.Save(new Customer() { Id = 12, Name = "test2222", Description = "sad" });
            var cus = databaseService.CustomerService.GetCustomerById(cusId);
            Assert.AreEqual("test2222", cus.Name);
        }


        [Test]
        public void TestDelete()
        {
            IDatabaseService databaseService = new DatabaseService();
            databaseService.ConnectionString = @"Data Source=SYLWIA\SQLEXPRESS;Initial Catalog=Test;Integrated Security=SSPI;";
            int cusId = databaseService.CustomerService.Save(new Customer() { Id = 12, Name = "test2222", Description = "sad" });
            var cus = databaseService.CustomerService.GetCustomerById(cusId);
            Assert.AreEqual("test2222", cus.Name);
            databaseService.CustomerService.Delete(cusId);
            var cusAfterDelete = databaseService.CustomerService.GetCustomerById(cusId);
            Assert.AreEqual(null, cusAfterDelete);
        }
    }
}

