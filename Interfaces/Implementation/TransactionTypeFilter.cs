using System;
using System.Collections.Generic;
using System.Text;

namespace Interfaces
{
    public class TransactionTypeFilter : ITransactionTypeFilter
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public bool? Income { get; set; }

    }
}
