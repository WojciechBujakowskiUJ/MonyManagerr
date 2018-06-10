using Interfaces;
using Statistics.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistics.TransactionStatistics
{

    public class TransactionStatisticsResult : IBarChartDataContainer<DateTime, decimal>
    {
        public IList<IBarChartEntry<DateTime, decimal>> BarChartData { get; }

        public StatisticsResultsHolderBalance Balance { get; }

        public StatisticsResultsHolderDetails Income { get; }

        public StatisticsResultsHolderDetails Expenses { get; }

        public TransactionStatisticsResult()
        {
            BarChartData = new List<IBarChartEntry<DateTime, decimal>>();
            Balance = new StatisticsResultsHolderBalance();
            Income = new StatisticsResultsHolderDetails(true);
            Expenses = new StatisticsResultsHolderDetails(false);
        }

    }
}
