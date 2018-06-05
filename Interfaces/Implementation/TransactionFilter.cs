using System;
using System.Collections.Generic;
using System.Text;

namespace Interfaces
{
    public class TransactionFilter : ITransactionFilter
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public decimal? Value { get; set; }
        public decimal? ValueMax { get; set; }
        public decimal? ValueMin { get; set; }
        public int? TransactionTypeId { get; set; }
        public int? CustomerId { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? DateMax { get; set; }
        public DateTime? DateMin { get; set; }
    }
}
