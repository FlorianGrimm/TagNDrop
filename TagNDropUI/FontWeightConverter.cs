using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TagNDropUI {
    public class FontWeightConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is double) {
                var weight = 1 + (int)(((double)value) * 999);
                if (weight < 1) { weight = 1; }
                if (weight > 999) { weight = 999; }
                return FontWeight.FromOpenTypeWeight(weight);
            }
            return new FontWeight();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return 0;
        }
    }
}
