using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistics.TransactionStatistics
{
    public class TransactionStatisticsProvider
    {

        public TransactionStatisticsProvider() { }

        public TransactionStatisticsResult Calculate(IList<ITransaction> data, DateTime from, DateTime to, TimeStepType timeStep)
        {
            var result = new TransactionStatisticsResult();

            // TODO

            return result;
        }

        public Task<TransactionStatisticsResult> CalculateAsync(IList<ITransaction> data, DateTime from, DateTime to, TimeStepType timeStep)
        {
            return Task.Factory.StartNew<TransactionStatisticsResult>(() => 
            {
                return Calculate(data, from, to, timeStep);
            });
        }

    }
}
