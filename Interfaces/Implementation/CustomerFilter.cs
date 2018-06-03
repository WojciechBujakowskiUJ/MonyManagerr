using System;
using System.Collections.Generic;
using System.Text;

namespace Interfaces.Implementation
{
    public class CustomerFilter : ICustomerFilter
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public bool? Active { get; set; }
        public int? DefaultTransactionTypeId { get; set; }


    }
}
