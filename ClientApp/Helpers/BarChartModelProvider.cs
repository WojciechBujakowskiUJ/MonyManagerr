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

            //ColumnSeries barSeries = new ColumnSeries { Title = "Balance", StrokeColor = OxyColors.Black, FillColor = OxyColors.White, StrokeThickness = 1};
            //data.BarChartData.ToList().ForEach(x => barSeries.Items.Add(new ColumnItem { Value = ((double)x.TotalValue)}));

            ColumnSeries barSeries2 = new ColumnSeries { Title = "Income", StrokeColor = OxyColors.Black, FillColor = OxyColors.Green.ChangeSaturation(0.4) };
            data.BarChartData.ToList().ForEach(x => barSeries2.Items.Add(new ColumnItem { Value = (double)x.PosValue }));

            ColumnSeries barSeries3 = new ColumnSeries { Title = "Expenses", StrokeColor = OxyColors.Black, FillColor = OxyColors.Red.ChangeSaturation(0.4) };
            data.BarChartData.OrderBy(x => x.Label).ToList().ForEach(x => barSeries3.Items.Add(new ColumnItem { Value = -(double)x.NegValue }));


            plot.Series.Add(barSeries2);
            plot.Series.Add(barSeries3);
            //plot.Series.Add(barSeries);

            string dataFormat = "dd.MM.yyyy";
            if (timeStep == Statistics.TimeStepType.Month)
            {
                dataFormat = "MM.yyyy";
            }
            else if (timeStep == Statistics.TimeStepType.Hour)
            {
                dataFormat = "hh dd.MM.yyyy";
            }

            var categoryAxis = new CategoryAxis { Position = AxisPosition.Bottom, GapWidth = 0, TickStyle = TickStyle.Outside };

            int barCount = data.BarChartData.Count;
            int labelShift = barCount / 10 + 1;

            for (int i = 0; i < barCount; ++i)
            {
                categoryAxis.Labels.Add(i % labelShift == 0 ? data.BarChartData[i].Label.ToString(dataFormat) : string.Empty);
            }

            //data.BarChartData.ToList().ForEach(x => categoryAxis.Labels.Add(x.Label.ToString(dataFormat)));




            plot.Axes.Add(categoryAxis);

            plot.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Solid, MaximumPadding = 0.2 });

            return plot;
        }



    }
}
