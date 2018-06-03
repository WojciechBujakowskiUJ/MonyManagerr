using System;
using System.Collections.Generic;
using System.Text;

namespace Interfaces
{
    public interface ICustomerFilter
    {
         int? Id { get; set; }
         string Name { get; set; }
         bool? Active { get; set; }
         int? DefaultTransactionTypeId { get; set; }
    }
}
