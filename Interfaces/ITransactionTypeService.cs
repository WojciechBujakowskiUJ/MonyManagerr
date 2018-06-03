using System;
using System.Collections.Generic;
using System.Text;

namespace Interfaces
{
    public interface ITransactionTypeService
    {

        int Save(ITransactionType transactionType);
        ITransactionType GetTransactionTypeById(int id);
        List<ITransactionType> GetTransactionTypes(TransactionTypeFilter filter);
        void Delete(int id);
    }
}
