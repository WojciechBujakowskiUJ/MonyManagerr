using System;
using System.Collections.Generic;
using System.Text;

namespace Interfaces
{
    public interface ITransactionType
    {
        int Id { get; set; }
        string Name { get; set; }
        string Color { get; set; }
        string Description { get; set; }
        bool Income { get; set; }
    }
}
