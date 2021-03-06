﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Interfaces
{
    public interface ITransaction
    {
        int Id { get; set; }
        string Name { get; set; }
        decimal Value { get; set; }
        int TransactionTypeId { get; set; }
        ITransactionType TransactionType { get; set; }
        string Description { get; set; }
        int? CustomerId { get; set; }
        ICustomer Customer { get; set; }
        DateTime Date { get; set; }
    }
}
