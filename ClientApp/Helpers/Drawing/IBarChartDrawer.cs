using Statistics.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Helpers.Drawing
{
    public interface IBarChartDrawer<LabelType, ValueType>
    {
        void Redraw(IBarChartDataContainer<LabelType, ValueType> data);        
    }
}
