using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ClientApp
{
    public static class ColorsDictionary
    {

        private static readonly IDictionary<string, Brush> _colors;
        private static readonly IDictionary<string, Brush> _colorsWithNull;

        static ColorsDictionary()
        {
            _colors = new Dictionary<string, Brush>();

            _colors.Add("Blue", Brushes.Blue);
            _colors.Add("Light Blue", Brushes.LightBlue);
            _colors.Add("Dark Blue", Brushes.DarkBlue);
            _colors.Add("Green", Brushes.Green);
            _colors.Add("Light Green", Brushes.LightGreen);
            _colors.Add("Dark Green", Brushes.DarkGreen);
            _colors.Add("Red", Brushes.Red);
            _colors.Add("Dark Red", Brushes.DarkRed);
            _colors.Add("Yellow", Brushes.Yellow);
            _colors.Add("Orange", Brushes.Orange);
            _colors.Add("Cyan", Brushes.Cyan);
            _colors.Add("Pink", Brushes.Pink);
            _colors.Add("Magenta", Brushes.Magenta);
            _colors.Add("Purple", Brushes.Purple);
            _colors.Add("Dark Gray", Brushes.DarkGray);
            _colors.Add("Brown", Brushes.Brown);

            _colorsWithNull = new Dictionary<string, Brush>();

            _colorsWithNull.Add("None", null);
            foreach (var color in _colors)
            {
                _colorsWithNull.Add(color.Key, color.Value);
            }
        }

        public static IDictionary<string,Brush> GetDictionary()
        {
            return _colors;
        }

        public static IDictionary<string, string> GetHexValuesDictionary()
        {
            return _colors.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString());
        }

        public static IDictionary<string, Brush> GetDictionaryWithNull()
        {
            return _colorsWithNull;
        }

        public static IDictionary<string, string> GetHexValuesDictionaryWithNull()
        {
            return _colorsWithNull.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString());
        }

        public static Brush Get(string name)
        {
            if (_colors.ContainsKey(name))
            {
                return _colors[name];
            }
            else
            {
                throw new IndexOutOfRangeException("No color found with name: " + name);
            }
        }

        public static string GetHexValue(string name)
        {
            return Get(name).ToString();
        }

        public static IList<Brush> GetBrushesList()
        {
            return _colors.Values.ToList();
        }

        public static IList<string> GetHexValuesList()
        {
            return _colors.Values.Select(b => b.ToString()).ToList();
        }

        public static string GetNameFromHex(string hex)
        {
            return _colors.Where(kvp => kvp.Value.ToString() == hex).Select(kvp => kvp.Key).FirstOrDefault();
        }

        public static Brush GetBrushFromHex(string hex)
        {
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex));
        }

    }
}
