using System;
using System.Collections.Generic;
using System.Text;

namespace Interfaces
{
    public interface ITransactionTypeService
    {

        int Save(ITransactionType transactionType);
        ITransactionType GetTransactionTypeById(int id);
        IList<ITransactionType> GetTransactionTypes(ITransactionTypeFilter filter);
        void Delete(int id);
    }
}
