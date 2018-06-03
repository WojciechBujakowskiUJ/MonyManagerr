using System;
using System.Collections.Generic;
using System.Text;

namespace Interfaces
{
    public interface ITransactionTypeFilter
    {
        int? Id { get; set; }
        string Name { get; set; }
        string Color { get; set; }
        bool? Income { get; set; }

    }
}
