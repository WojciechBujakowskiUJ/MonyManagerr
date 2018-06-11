using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistics.Interfaces
{
    public interface IBarChartEntry<TLabel, TValue>
    {
        TLabel Label { get; set; }

        TValue PosValue { get; set; }
        TValue NegValue { get; set; }
        TValue TotalValue { get; }
    }
}
