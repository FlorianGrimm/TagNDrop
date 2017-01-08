using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace TagNDropUI {
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class DoubleMultiplyConverter : IValueConverter {
        public double Factor { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value == null) { return value; }
            if (parameter == null && this.Factor == 0.0) { return value; }
            var dValue = System.Convert.ToDouble(value);
            var dParameter = (parameter == null) ? this.Factor : System.Convert.ToDouble(parameter);
            return dValue * dParameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value == null) { return value; }
            if (parameter == null && this.Factor == 0.0) { return value; }
            var dValue = System.Convert.ToDouble(value);
            var dParameter = (parameter == null) ? this.Factor : System.Convert.ToDouble(parameter);
            if (dParameter == 0.0) { return 0.0; }
            return dValue / dParameter;
        }
    }
}
