using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    public class PieChartEntry
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public string FillColor { get; set; }

        public PieChartEntry(string name, decimal value, string color)
        {
            Name = name;
            Value = value;
            FillColor = color;
        }
    }
}
