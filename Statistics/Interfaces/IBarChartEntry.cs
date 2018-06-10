using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistics.Interfaces
{
    public interface IBarChartEntry<LabelType, ValueType>
    {
        LabelType Label { get; set; }

        ValueType PosValue { get; set; }
        ValueType NegValue { get; set; }
        ValueType TotalValue { get; }
    }
}
