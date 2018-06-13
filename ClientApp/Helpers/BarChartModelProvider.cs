using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Statistics.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Helpers
{
    public static class BarChartModelProvider
    {

        public static PlotModel GetModel(IBarChartDataContainer<DateTime, decimal> data, Statistics.TimeStepType timeStep)
        {

            OxyPlot.PlotModel plot = new OxyPlot.PlotModel();

            // BarSeries barSeries = new BarSeries { Title = "Balance", StrokeColor = OxyColors.Black};
            //statsRes.BarChartData.ToList().ForEach(x => barSeries.Items.Add(new BarItem { Value = (Math.Abs((double)x.TotalValue))}));

            BarSeries barSeries2 = new BarSeries { Title = "Income", StrokeColor = OxyColors.Black };
            data.BarChartData.ToList().ForEach(x => barSeries2.Items.Add(new BarItem { Value = (double)x.PosValue }));


            BarSeries barSeries3 = new BarSeries { Title = "Outcome", StrokeColor = OxyColors.Black };
            data.BarChartData.OrderBy(x => x.Label).ToList().ForEach(x => barSeries3.Items.Add(new BarItem { Value = -(double)x.NegValue }));


            // plot.Series.Add(barSeries);
            plot.Series.Add(barSeries2);
            plot.Series.Add(barSeries3);
            string dataFormat = "dd.MM.yyyy";
            if (timeStep == Statistics.TimeStepType.Month)
            {
                dataFormat = "MM.yyyy";
            }
            else if (timeStep == Statistics.TimeStepType.Hour)
            {
                dataFormat = "hh dd.MM.yyyy";
            }

            var categoryAxis = new CategoryAxis { Position = AxisPosition.Left };
            data.BarChartData.ToList().ForEach(x => categoryAxis.Labels.Add(x.Label.ToString(dataFormat)));
            plot.Axes.Add(categoryAxis);
            return plot;
        }



    }
}
