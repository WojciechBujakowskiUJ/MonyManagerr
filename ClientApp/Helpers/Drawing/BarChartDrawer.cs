using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ClientApp.Helpers.Drawing
{
    internal class BarChartDrawer : IBarChartDrawer
    {

        #region Local Fields

        private readonly Grid drawingGrid = null;

        #endregion

        #region Constructor

        public BarChartDrawer(Grid grid)
        {
            drawingGrid = grid;
        }

        #endregion

        #region Interface Methods

        public void Redraw(object data)
        {
            drawingGrid.Children.Clear();
        } 

        #endregion

    }
}
