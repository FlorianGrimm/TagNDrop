using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace TagNDropUI {
    /// <summary>
    /// Convert between boolean and visibility
    /// </summary>
    [Localizability(LocalizationCategory.NeverLocalize)]
    public sealed class Boolean2VisibilityConverter : IValueConverter {
        public Boolean2VisibilityConverter() {
            this.TrueVisibility = Visibility.Visible;
            this.FalseVisibility = Visibility.Collapsed;
        }

        public Visibility TrueVisibility { get; set; }
        public Visibility FalseVisibility { get; set; }

        /// <summary>
        /// Convert bool or Nullable&lt;bool&gt; to Visibility
        /// </summary>
        /// <param name="value">bool or Nullable&lt;bool&gt;</param>
        /// <param name="targetType">Visibility</param>
        /// <param name="parameter">Visibility or null </param>
        /// <param name="culture">ignored</param>
        /// <returns>Visible or Collapsed/Hidden</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            bool bValue = false;
            if (value is bool) {
                bValue = (bool)value;
            } else if (value is bool?) {
                bool? tmp = (bool?)value;
                bValue = tmp.GetValueOrDefault();
            }
            if (bValue) { return this.TrueVisibility; }
            return this.ConvertVisiblity(parameter);
        }

        /// <summary>
        /// Convert Visibility to boolean
        /// </summary>
        /// <param name="value">Visibility</param>
        /// <param name="targetType">ignored</param>
        /// <param name="parameter">is ignored</param>
        /// <param name="culture">is also ignored</param>
        /// <returns>true if value is Visible.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is Visibility) {
                return (Visibility)value == this.TrueVisibility;
            } else {
                return false;
            }
        }

        private Visibility ConvertVisiblity(object parameter) {
            if (parameter is Visibility) {
                return (Visibility)parameter;
            }
            if (parameter is string) {
                Visibility result;
                if (Enum.TryParse((string)parameter, out result)) {
                    return result;
                }
            }
            return this.FalseVisibility;
        }
    }
}
