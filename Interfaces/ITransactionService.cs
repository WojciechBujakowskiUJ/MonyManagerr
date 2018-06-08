using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
   public interface ITransactionService
    {
        int Save(ITransaction transaction);
        ITransaction GetTransactionById(int id);
        IList<ITransaction> GetTransactions();
        IList<ITransaction> GetTransactions(ITransactionFilter filter);
        void Delete(int id);

        Task<int> SaveAsync(ITransaction transaction);
        Task<ITransaction> GetTransactionByIdAsync(int id);
        Task<IList<ITransaction>> GetTransactionsAsync();
        Task<IList<ITransaction>> GetTransactionsAsync(ITransactionFilter filter);
        Task DeleteAsync(int id);
    }
}
