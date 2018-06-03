using System;
using System.Collections.Generic;
using System.Text;

namespace Interfaces
{
    public interface ITransactionFilter
    {
        int? Id { get; set; }
        string Name { get; set; }
        decimal? Value { get; set; }
        decimal? ValueMax { get; set; }
        decimal? ValueMin { get; set; }
        int? TransactionTypeId { get; set; }
        int? CustomerId { get; set; }
        DateTime? Date { get; set; }
        DateTime? DateMax { get; set; }
        DateTime? DateMin { get; set; }
    }
}
