using Interfaces;
using Statistics.Interfaces;
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

        public TransactionStatisticsResult Calculate(IList<ITransaction> data, DateTime from, DateTime to, TimeStepType timeStep, bool fillBlankBars = false)
        {
            var result = new TransactionStatisticsResult();


            var income = data.Where(x => x.Date >= from && x.Date <= to && x.Value > 0);
            var expenses = data.Where(x => x.Date >= from && x.Date <= to && x.Value < 0);
            var balance = data.Where(x => x.Date >= from && x.Date <= to);



            if (income.Count() == 0)
            {
                result.Balance.Average = 0;
                result.Balance.Sum = 0;
                result.Balance.Median = 0;
            }
            else
            {
                result.Balance.Average = balance.Average(x => x.Value);
                result.Balance.Sum = balance.Sum(x => x.Value);
                result.Balance.Median = (balance.OrderBy(x => x.Value).ElementAt(balance.Count() / 2).Value
                                        + balance.OrderBy(x => x.Value).ElementAt((balance.Count() - 1) / 2).Value) / 2;
                result.Balance.StdDeviation = (decimal)Math.Sqrt((double)balance.Average(x => x.Value * x.Value) - (double)result.Balance.Average * (double)result.Balance.Average);
            }
            if (expenses.Count() == 0)
            {
                result.Expenses.Average = 0;
                result.Expenses.Sum = 0;
                result.Expenses.Median = 0;
            }
            else
            {
                result.Expenses.Average = expenses.Average(x => x.Value);
                result.Expenses.Sum = expenses.Sum(x => x.Value);
                result.Expenses.Median = (expenses.OrderBy(x => x.Value).ElementAt(expenses.Count() / 2).Value
                                        + expenses.OrderBy(x => x.Value).ElementAt((expenses.Count() - 1) / 2).Value) / 2;
                result.Expenses.StdDeviation = (decimal)Math.Sqrt((double)expenses.Average(x => x.Value * x.Value) - (double)result.Expenses.Average * (double)result.Expenses.Average);
            }

            if (income.Count() == 0)
            {
                result.Income.Average = 0;
                result.Income.Sum = 0;
                result.Income.Median = 0;
            }
            else
            {
                result.Income.Average = income.Average(x => x.Value);
                result.Income.Sum = income.Sum(x => x.Value);
                result.Income.Median = (income.OrderBy(x => x.Value).ElementAt(income.Count() / 2).Value
                                        + income.OrderBy(x => x.Value).ElementAt((income.Count() - 1) / 2).Value) / 2;
                result.Income.StdDeviation = (decimal)Math.Sqrt((double)income.Average(x => x.Value * x.Value) - (double)result.Income.Average * (double)result.Income.Average);

            }



            String dateFormat = "yyyyMMdd";
            if (timeStep == TimeStepType.Month)
            {
                dateFormat = "yyyyMM";
            }
            else if (timeStep == TimeStepType.Hour)
            {
                dateFormat = "yyyyMMddhh";
            }
            else
            {
                dateFormat = "yyyyMMdd";
            }

            if (fillBlankBars)
            {
                FillBlankPeriods(data, from, to, timeStep);
            }

            if (timeStep == TimeStepType.Week)
            {
                dateFormat = "yyyyMMdd";
                data.OrderBy(x => x.Date).Where(x => x.Date >= from && x.Date <= to).GroupBy(i => i.Date.AddDays(-(int)i.Date.DayOfWeek).ToString(dateFormat)).ToList()
                                .ForEach(y =>
                                result.BarChartData.Add(new TransactionsBarEntry(
                                    DateTime.ParseExact(y.First().Date.ToString(dateFormat), dateFormat, System.Globalization.CultureInfo.InvariantCulture),
                                    y.Where(x => x.Value > 0).Sum(x => x.Value),
                                    y.Where(x => x.Value < 0).Sum(x => x.Value))));
            }
            else
            {
                data.OrderBy(x => x.Date).Where(x => x.Date >= from && x.Date <= to).GroupBy(i => i.Date.ToString(dateFormat)).ToList()
                    .ForEach(y =>
                    result.BarChartData.Add(new TransactionsBarEntry(
                        DateTime.ParseExact(y.First().Date.ToString(dateFormat), dateFormat, System.Globalization.CultureInfo.InvariantCulture),
                        y.Where(x => x.Value > 0).Sum(x => x.Value),
                        y.Where(x => x.Value < 0).Sum(x => x.Value))));
            }

            if (result.BarChartData != null && result.BarChartData.Count > 0)
            {
                result.Balance.MaxBar = (TransactionsBarEntry)result.BarChartData.Aggregate((i1, i2) => i1.TotalValue > i2.TotalValue ? i1 : i2);
                result.Balance.MinBar = (TransactionsBarEntry)result.BarChartData.Aggregate((i1, i2) => i1.TotalValue < i2.TotalValue ? i1 : i2);

                result.Expenses.MaxBar = (TransactionsBarEntry)result.BarChartData.Aggregate((i1, i2) => i1.PosValue > i2.PosValue ? i1 : i2);
                result.Expenses.MinBar = (TransactionsBarEntry)result.BarChartData.Aggregate((i1, i2) => i1.PosValue < i2.PosValue ? i1 : i2);

                result.Income.MaxBar = (TransactionsBarEntry)result.BarChartData.Aggregate((i1, i2) => i1.NegValue > i2.NegValue ? i1 : i2);
                result.Income.MinBar = (TransactionsBarEntry)result.BarChartData.Aggregate((i1, i2) => i1.NegValue < i2.NegValue ? i1 : i2);
            }

            return result;
        }

        public Task<TransactionStatisticsResult> CalculateAsync(IList<ITransaction> data, DateTime from, DateTime to, TimeStepType timeStep, bool fillBlankBars = false)
        {
            return Task.Factory.StartNew<TransactionStatisticsResult>(() =>
            {
                return Calculate(data, from, to, timeStep, fillBlankBars);
            });
        }

        #region Helpers

        private void FillBlankPeriods(IList<ITransaction> data, DateTime from, DateTime to, TimeStepType timeStep)
        {
            DateTime dt = from;

            while (dt < to)
            {
                data.Add(new Transaction() { Date = dt, Value = 0M });

                switch(timeStep)
                {
                case TimeStepType.Hour:
                    dt = dt.AddHours(1);
                    break;

                case TimeStepType.Week:
                    dt = dt.AddDays(7);
                    break;

                case TimeStepType.Month:
                    dt = dt.AddDays(28);
                    break;

                default:
                    dt = dt.AddDays(1);
                    break;
                }
            }

            data.Add(new Transaction() { Date = to, Value = 0M });
        }

        #endregion

    }
}
