using DatabaseConnect;
using Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnectTests.DatabaseConnectTest
{
    [TestFixture]
    class TestCustomer : TestICustomer
    {

        public override IDatabaseService GetDatabaseServiceInstance()
        {
            return new DatabaseService() { ConnectionString = ConfigurationManager.ConnectionStrings["TestEntities"].ConnectionString };
        }
    }
}
