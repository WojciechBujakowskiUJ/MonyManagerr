using Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseConnect
{
    class TransactionService : ITransactionService
    {
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ITransaction GetTransactionById(int id)
        {
            throw new NotImplementedException();
        }

        public List<ITransaction> GetTransactions(ITransactionFilter filter)
        {
            throw new NotImplementedException();
        }

        public int Save(ITransaction transaction)
        {
            throw new NotImplementedException();
        }
    }
}
