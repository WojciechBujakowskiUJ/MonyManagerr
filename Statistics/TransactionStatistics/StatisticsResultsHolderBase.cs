using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistics.TransactionStatistics
{
    public abstract class StatisticsResultsHolderBase
    {
        public TransactionsBarEntry MinBar { get; set; }
        public TransactionsBarEntry MaxBar { get; set; }

        public decimal Sum { get; set; }
        public decimal Average { get; set; }
        public decimal Median { get; set; }
        public decimal StdDeviation { get; set; }

        public abstract decimal MaxBarValue { get; }
        public abstract decimal MinBarValue { get; }
    }
}
