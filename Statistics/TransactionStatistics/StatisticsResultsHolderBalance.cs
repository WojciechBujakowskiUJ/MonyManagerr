using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistics.TransactionStatistics
{
    public class StatisticsResultsHolderBalance : StatisticsResultsHolderBase
    {
        public override decimal MaxBarValue
        {
            get
            {
                return MaxBar.TotalValue;
            }
        }
        public override decimal MinBarValue
        {
            get
            {
                return MinBar.TotalValue;
            }
        }

    }
}
