using System;
using System.Collections.Generic;
using System.Text;

namespace Interfaces.Implementation
{
    public class Customer : ICustomer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public Nullable<int> DefaultTransactionTypeId { get; set; }
    }
}
