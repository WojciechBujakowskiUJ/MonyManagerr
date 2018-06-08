using System;
using System.Collections.Generic;
using System.Text;

namespace Interfaces
{
    public class Transaction : ITransaction
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public int TransactionTypeId { get; set; }
        public ITransactionType TransactionType { get; set; }
        public string Description { get; set; }
        public int? CustomerId { get; set; }
        public ICustomer Customer { get; set; }
        public DateTime Date { get; set; }
    }
}
