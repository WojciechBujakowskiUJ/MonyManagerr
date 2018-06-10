using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistics.Interfaces
{
    public interface IBarChartDataContainer<TLabel, TValue>
    {
       IList<IBarChartEntry<TLabel,TValue>> BarChartData { get; }
    }
}
