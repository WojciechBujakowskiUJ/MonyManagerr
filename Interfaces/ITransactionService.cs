using System;
using System.Collections.Generic;
using System.Text;

namespace Interfaces
{
   public interface ITransactionService
    {
        int Save(ITransaction transaction);
        ITransaction GetTransactionById(int id);
        List<ITransaction> GetTransactions(ITransactionFilter filter);
        void Delete(int id);
    }
}
