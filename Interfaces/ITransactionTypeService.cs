using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface ITransactionTypeService
    {
        int Save(ITransactionType transactionType);
        ITransactionType GetTransactionTypeById(int id);
        IList<ITransactionType> GetTransactionTypes();
        IList<ITransactionType> GetTransactionTypes(ITransactionTypeFilter filter);
        void Delete(int id);

        Task<int> SaveAsync(ITransactionType transactionType);
        Task<ITransactionType> GetTransactionTypeByIdAsync(int id);
        Task<IList<ITransactionType>> GetTransactionTypesAsync();
        Task<IList<ITransactionType>> GetTransactionTypesAsync(ITransactionTypeFilter filter);
        Task DeleteAsync(int id);
    }
}
