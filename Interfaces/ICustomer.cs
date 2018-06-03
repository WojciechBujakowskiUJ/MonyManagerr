using System;
using System.Collections.Generic;
using System.Text;

namespace Interfaces
{
    public interface ICustomer
    {
        int Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        bool Active { get; set; }
        Nullable<int> DefaultTransactionTypeId { get; set; }
    }
}
