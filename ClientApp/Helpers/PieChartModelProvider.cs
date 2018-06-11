using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    public static class PieChartModelProvider
    {

        public static PlotModel GetModel(IList<PieChartEntry> entries)
        {
            PlotModel result = new PlotModel();

            var series = new PieSeries();

            SetSlices(series.Slices, entries);

            series.InnerDiameter = 0;
            series.Stroke = OxyColors.White;
            series.StrokeThickness = 0.5;
            series.InsideLabelPosition = 0.6;
            series.AngleSpan = 360;
            series.StartAngle = 0;
            series.Selectable = false;

            result.Series.Add(series);

            return result;
        }

        private static void SetSlices(IList<PieSlice> slices, IList<PieChartEntry> entries)
        {
            decimal total = entries.Sum(e => e.Value);

            foreach (var entry in entries)
            {
                double perc = Math.Abs((double)(entry.Value / total));
                slices.Add(new PieSlice(entry.Name, perc) { Fill = OxyColor.Parse(entry.FillColor), IsExploded = false } );
            }

        }


    }
}
