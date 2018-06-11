using DatabaseConnect;
using Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnectTests.DatabaseConnectTest
{
    [TestFixture]
    class TestTransactionType : TestITransactionType
    {
        public override IDatabaseService GetDatabaseServiceInstance()
        {
            return new DatabaseService() { ConnectionString = @"Data Source=SYLWIA\SQLEXPRESS;Initial Catalog=Test;Integrated Security=SSPI;" };
        }
    }
}
