using Statistics.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistics.TransactionStatistics
{
    public class TransactionsBarEntry : IBarChartEntry<DateTime, decimal>
    {
        public DateTime Label { get; set; }

        private decimal _posValue;
        public decimal PosValue
        {
            get
            {
                return _posValue;
            }
            set
            {
                if (value < 0M )
                {
                    throw new ArgumentException("Value must be non-negative");
                }
                _posValue = value;
            }
        }

        private decimal _negValue;
        public decimal NegValue
        {
            get
            {
                return _negValue;
            }
            set
            {
                if (value > 0M)
                {
                    throw new ArgumentException("Value must be non-positive");
                }
                _negValue = value;
            }
        }


        public decimal TotalValue
        {
            get
            {
                return PosValue + NegValue;
            }
        }

        public TransactionsBarEntry(DateTime label, decimal expenses = 0M, decimal income = 0M)
        {
            Label = label;
            PosValue = expenses;
            NegValue = income;
        }

    }
}
