using Interfaces;
using NUnit.Framework;
using Statistics;
using Statistics.TransactionStatistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticsTests
{
    [TestFixture]
    public class TransactionsStatisticsProviderTests
    {
 
        TransactionStatisticsProvider tsp = new TransactionStatisticsProvider();
        List<ITransaction> data = new List<ITransaction>();

        [Test]
        public void SimpleTest()
        {
            data = new List<ITransaction>();
            ITransaction transaction = new Transaction();
            transaction.Value = 100;
            transaction.Date = new DateTime(2001, 1, 1);
            data.Add(transaction);

            transaction = new Transaction();
            transaction.Value = -100;
            transaction.Date = new DateTime(2001, 2, 1);
            data.Add(transaction);

            transaction = new Transaction();
            transaction.Value = 100;
            transaction.Date = new DateTime(2002, 6, 6);
            data.Add(transaction);

            transaction = new Transaction();
            transaction.Value = 200;
            transaction.Date = new DateTime(2001, 1, 3);
            data.Add(transaction);

            transaction = new Transaction();
            transaction.Value = -300;
            transaction.Date = new DateTime(2001, 6, 5);
            data.Add(transaction);

            transaction = new Transaction();
            transaction.Value = 100;
            transaction.Date = new DateTime(2001, 4, 1);
            data.Add(transaction);


            transaction = new Transaction();
            transaction.Value = 10000;
            transaction.Date = new DateTime(4000, 8, 8);
            data.Add(transaction);

            transaction = new Transaction();
            transaction.Value = 10000;
            transaction.Date = new DateTime(400, 8, 8);
            data.Add(transaction);

            var result = tsp.Calculate(data, new DateTime(1000, 1, 1), new DateTime(3000, 2, 3), TimeStepType.Month);

            Assert.AreEqual(100, result.Balance.Sum);
            Assert.AreEqual(-400, result.Expenses.Sum);
            Assert.AreEqual(500, result.Income.Sum);

            Assert.AreEqual(100 / 6.0, result.Balance.Average);
            Assert.AreEqual(-400 / 2, result.Expenses.Average);
            Assert.AreEqual(500 / 4, result.Income.Average);

            Assert.AreEqual(100, result.Balance.Median);
            Assert.AreEqual(-200, result.Expenses.Median);
            Assert.AreEqual(100, result.Income.Median);

            Assert.AreEqual(300, result.BarChartData.ElementAt(0).TotalValue);

        }

        [Test]
        public void StartEndDateTest()
        {
            data = new List<ITransaction>();
            ITransaction transaction = new Transaction();

            transaction = new Transaction();
            transaction.Value = 100;
            transaction.Date = new DateTime(2001, 1, 1);
            data.Add(transaction);

            transaction = new Transaction();
            transaction.Value = -50;
            transaction.Date = new DateTime(2001, 2, 3);
            data.Add(transaction);


            var result = tsp.Calculate(data, new DateTime(2001, 1, 1), new DateTime(2001, 2, 3), TimeStepType.Month);

            Assert.AreEqual(50, result.Balance.Sum);

        }


        [Test]
        public void BetweenDateTest()
        {
            data = new List<ITransaction>();
            ITransaction transaction = new Transaction();
            transaction.Value = 100;
            transaction.Date = new DateTime(2001, 1, 1);
            data.Add(transaction);

            transaction = new Transaction();
            transaction.Value = -100;
            transaction.Date = new DateTime(2001, 2, 1);
            data.Add(transaction);

            transaction = new Transaction();
            transaction.Value = 100;
            transaction.Date = new DateTime(2002, 6, 6);
            data.Add(transaction);

            transaction = new Transaction();
            transaction.Value = 200;
            transaction.Date = new DateTime(2001, 1, 3);
            data.Add(transaction);

            transaction = new Transaction();
            transaction.Value = -300;
            transaction.Date = new DateTime(2001, 6, 5);
            data.Add(transaction);

            transaction = new Transaction();
            transaction.Value = 100;
            transaction.Date = new DateTime(2001, 4, 1);
            data.Add(transaction);


            transaction = new Transaction();
            transaction.Value = 10000;
            transaction.Date = new DateTime(4000, 8, 8);
            data.Add(transaction);

            transaction = new Transaction();
            transaction.Value = 10000;
            transaction.Date = new DateTime(400, 8, 8);
            data.Add(transaction);


            var result = tsp.Calculate(data, new DateTime(2001, 1, 2), new DateTime(2001, 4, 20), TimeStepType.Month);

            Assert.AreEqual(200, result.Balance.Sum);
            Assert.AreEqual(-100, result.Expenses.Sum);
            Assert.AreEqual(300, result.Income.Sum);
        }

        [Test]
        public void MedianOddEvenTest()
        {
            data = new List<ITransaction>();
            ITransaction transaction = new Transaction();

            transaction = new Transaction();
            transaction.Value = 100;
            transaction.Date = new DateTime(2001, 1, 1);
            data.Add(transaction);

            transaction = new Transaction();
            transaction.Value = -50;
            transaction.Date = new DateTime(2001, 1, 2);
            data.Add(transaction);

            transaction = new Transaction();
            transaction.Value = 200;
            transaction.Date = new DateTime(2001, 1, 3);
            data.Add(transaction);

            transaction = new Transaction();
            transaction.Value = -150;
            transaction.Date = new DateTime(2001, 2, 3);
            data.Add(transaction);

            var result = tsp.Calculate(data, new DateTime(2000, 1, 1), new DateTime(2002, 4, 2), TimeStepType.Month);

            Assert.AreEqual(25, result.Balance.Median);
            Assert.AreEqual(-100, result.Expenses.Median);
            Assert.AreEqual(150, result.Income.Median);

            result = tsp.Calculate(data, new DateTime(2001, 1, 1), new DateTime(2001, 1, 2), TimeStepType.Month);
            Assert.AreEqual(25, result.Balance.Median);
            Assert.AreEqual(-50, result.Expenses.Median);
            Assert.AreEqual(100, result.Income.Median);

            result = tsp.Calculate(data, new DateTime(2001, 1, 1), new DateTime(2001, 1, 3), TimeStepType.Month);
            Assert.AreEqual(100, result.Balance.Median);
        }

        [Test]
        public void OneTransactionTest()
        {
            data = new List<ITransaction>();
            ITransaction transaction = new Transaction();
            transaction.Value = 100;
            transaction.Date = new DateTime(2001, 1, 1);
            data.Add(transaction);

            var result = tsp.Calculate(data, new DateTime(2001, 1, 1), new DateTime(2001, 1, 1), TimeStepType.Month);

            Assert.AreEqual(100, result.Balance.Sum);
            Assert.AreEqual(0, result.Expenses.Sum);
            Assert.AreEqual(100, result.Income.Sum);

            Assert.AreEqual(100, result.Balance.Average);
            Assert.AreEqual(0, result.Expenses.Average);
            Assert.AreEqual(100, result.Income.Average);

            Assert.AreEqual(100, result.Balance.Median);
            Assert.AreEqual(0, result.Expenses.Median);
            Assert.AreEqual(100, result.Income.Median);

            Assert.AreEqual(100, result.Balance.MaxBar.TotalValue);
            Assert.AreEqual(100, result.BarChartData.First().TotalValue);
            Assert.AreEqual(100, result.BarChartData.ElementAt(0).TotalValue);
            Assert.AreEqual(0, result.BarChartData.ElementAt(0).NegValue);
            Assert.AreEqual(100, result.BarChartData.ElementAt(0).PosValue);
        }

        [Test]
        public void MonthTimeStepTest()
        {
            data = new List<ITransaction>();
            ITransaction transaction = new Transaction();

            transaction = new Transaction();
            transaction.Value = 100;
            transaction.Date = new DateTime(2001, 2, 21);
            data.Add(transaction);

            transaction = new Transaction();
            transaction.Value = -50;
            transaction.Date = new DateTime(2001, 2, 12);
            data.Add(transaction);

            transaction = new Transaction();
            transaction.Value = 50;
            transaction.Date = new DateTime(2002, 8, 22);
            data.Add(transaction);

            transaction = new Transaction();
            transaction.Value = -100;
            transaction.Date = new DateTime(2002, 8, 11);
            data.Add(transaction);

            var result = tsp.Calculate(data, new DateTime(2001, 1, 1), new DateTime(2003, 4, 20), TimeStepType.Month);

            Assert.AreEqual(50, result.Balance.MaxBar.TotalValue);
            Assert.AreEqual(50, result.BarChartData.First().TotalValue);
            Assert.AreEqual(-50, result.BarChartData.ElementAt(1).TotalValue);
            Assert.AreEqual(-100, result.BarChartData.ElementAt(1).NegValue);
            Assert.AreEqual(50, result.BarChartData.ElementAt(1).PosValue);

        }

        [Test]
        public void DayTimeStepTest()
        {
            data = new List<ITransaction>();
            ITransaction transaction = new Transaction();

            transaction = new Transaction();
            transaction.Value = 100;
            transaction.Date = new DateTime(2001, 2, 21, 9, 2, 2);
            data.Add(transaction);

            transaction = new Transaction();
            transaction.Value = -50;
            transaction.Date = new DateTime(2001, 2, 21, 7, 2, 2);
            data.Add(transaction);

            transaction = new Transaction();
            transaction.Value = 50;
            transaction.Date = new DateTime(2001, 2, 25, 5, 7, 2);
            data.Add(transaction);

            transaction = new Transaction();
            transaction.Value = -100;
            transaction.Date = new DateTime(2001, 2, 25, 8, 2, 2);
            data.Add(transaction);



            var result = tsp.Calculate(data, new DateTime(2001, 1, 1), new DateTime(2003, 4, 20), TimeStepType.Day);

            Assert.AreEqual(50, result.Balance.MaxBar.TotalValue);
            Assert.AreEqual(50, result.BarChartData.First().TotalValue);
            Assert.AreEqual(-50, result.BarChartData.ElementAt(1).TotalValue);
            Assert.AreEqual(-100, result.BarChartData.ElementAt(1).NegValue);
            Assert.AreEqual(50, result.BarChartData.ElementAt(1).PosValue);
        }

        [Test]
        public void HourTimeStepTest()
        {
            data = new List<ITransaction>();
            ITransaction transaction = new Transaction();

            transaction = new Transaction();
            transaction.Value = 100;
            transaction.Date = new DateTime(2001, 2, 21, 9, 4, 2);
            data.Add(transaction);

            transaction = new Transaction();
            transaction.Value = -50;
            transaction.Date = new DateTime(2001, 2, 21, 9, 8, 9);
            data.Add(transaction);

            transaction = new Transaction();
            transaction.Value = 50;
            transaction.Date = new DateTime(2001, 2, 25, 5, 7, 2);
            data.Add(transaction);

            transaction = new Transaction();
            transaction.Value = -100;
            transaction.Date = new DateTime(2001, 2, 25, 5, 2, 3);
            data.Add(transaction);

            var result = tsp.Calculate(data, new DateTime(2001, 1, 1), new DateTime(2003, 4, 20), TimeStepType.Hour);

            Assert.AreEqual(50, result.Balance.MaxBar.TotalValue);
            Assert.AreEqual(50, result.BarChartData.First().TotalValue);
            Assert.AreEqual(-50, result.BarChartData.ElementAt(1).TotalValue);
            Assert.AreEqual(-100, result.BarChartData.ElementAt(1).NegValue);
            Assert.AreEqual(50, result.BarChartData.ElementAt(1).PosValue);


        }
        [Test]
        public void WeekTimeStepTest()
        {
            data = new List<ITransaction>();
            ITransaction transaction = new Transaction();

            transaction = new Transaction();
            transaction.Value = 100;
            transaction.Date = new DateTime(2018, 6, 3);
            data.Add(transaction);

            transaction = new Transaction();
            transaction.Value = -50;
            transaction.Date = new DateTime(2018, 6, 9);
            data.Add(transaction);

            transaction = new Transaction();
            transaction.Value = 50;
            transaction.Date = new DateTime(2018, 6, 11);
            data.Add(transaction);

            transaction = new Transaction();
            transaction.Value = -100;
            transaction.Date = new DateTime(2018, 6, 14);
            data.Add(transaction);


            var result = tsp.Calculate(data, new DateTime(2001, 1, 1), new DateTime(2020, 4, 20), TimeStepType.Week);
            Assert.AreEqual(50, result.Balance.MaxBar.TotalValue);
            Assert.AreEqual(50, result.BarChartData.First().TotalValue);
            Assert.AreEqual(-50, result.BarChartData.ElementAt(1).TotalValue);
            Assert.AreEqual(-100, result.BarChartData.ElementAt(1).NegValue);
            Assert.AreEqual(50, result.BarChartData.ElementAt(1).PosValue);
        }

        [Test]
        public void StdDeviationTest()
        {
            data = new List<ITransaction>();
            ITransaction transaction = new Transaction();

            transaction = new Transaction();
            transaction.Value = 100;
            transaction.Date = new DateTime(2018, 6, 3);
            data.Add(transaction);

            transaction = new Transaction();
            transaction.Value = 200;
            transaction.Date = new DateTime(2018, 6, 9);
            data.Add(transaction);

            transaction = new Transaction();
            transaction.Value = 150;
            transaction.Date = new DateTime(2018, 6, 11);
            data.Add(transaction);

            transaction = new Transaction();
            transaction.Value = -50;
            transaction.Date = new DateTime(2018, 6, 14);
            data.Add(transaction);


            var result = tsp.Calculate(data, new DateTime(2001, 1, 1), new DateTime(2020, 4, 20), TimeStepType.Month);
            Assert.AreEqual(93.54, (double)result.Balance.StdDeviation, 0.01);
            Assert.AreEqual(0, (double)result.Expenses.StdDeviation, 0.01);

        }


    }
}