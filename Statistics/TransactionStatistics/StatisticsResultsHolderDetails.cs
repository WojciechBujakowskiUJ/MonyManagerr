using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistics.TransactionStatistics
{
    public class StatisticsResultsHolderDetails : StatisticsResultsHolderBase
    {
        private readonly bool isIncome;

        public StatisticsResultsHolderDetails(bool isIncome) : base()
        {
            this.isIncome = isIncome;
        }

        public ITransaction MinTransaction { get; set; }
        public ITransaction MaxTransaction { get; set; }

        public decimal MaxValue
        {
            get
            {
                return MaxTransaction.Value;
            }
        }
        public decimal MinValue
        {
            get
            {
                return MinTransaction.Value;
            }
        }
        public override decimal MaxBarValue
        {
            get
            {
                return isIncome ? MaxBar.PosValue : MaxBar.NegValue;
            }
        }
        public override decimal MinBarValue
        {
            get
            {
                return isIncome ? MaxBar.PosValue : MaxBar.NegValue;
            }
        }

    }
}
