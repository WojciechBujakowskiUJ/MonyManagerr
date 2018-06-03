using System;
using System.Collections.Generic;
using System.Text;

namespace Interfaces
{
    public class TransactionType: ITransactionType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string Description { get; set; }
        public bool Income { get; set; }
    }
}
