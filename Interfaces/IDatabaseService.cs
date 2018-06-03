using System;
using System.Collections.Generic;
using System.Text;

namespace Interfaces
{
    public interface IDatabaseService
    {
        string ConnectionString{ get; set; }
        ITransactionTypeService TransactionTypeService { get; }
        ITransactionService TransactionService { get; }
        ICustomerService CustomerService { get; }
    }
}
