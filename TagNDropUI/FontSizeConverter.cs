using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TagNDropUI {
    public class FontSizeConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is double) {
                var size = ((double)value * 12.0d) + 12.0d;
                if (size < 12.0d) { size = 12.0d; }
                if (size > 24.0d) { size = 24.0d; }
                return size;
            }
            return 12.0d;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return 0.0d;
        }
    }
}
