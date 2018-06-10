using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ClientApp.Helpers.Drawing
{
    public class BarChartDrawerFactory
    {
        public IBarChartDrawer<DateTime, decimal> CreateForTransactions(Grid drawingGrid)
        {
            if (drawingGrid == null)
            {
                throw new ArgumentNullException("Parameter '" + nameof(drawingGrid) + "' cannot be null.");
            }
            else
            {
                // add more possibilities if time permits
                return new BarChartDrawer(drawingGrid);
            }
        }

    }
}
