using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagNDropLibrary {
    public static class Extensions {
        public static T[] AddTo<T>(this T[] that, T item) {
            var l = that.Length;
            var result = new T[l + 1];
            if (l > 0) {
                Array.Copy(that, result, l);
            }
            result[l] = item;
            return result;
        }

        public static bool IsEmptyOrDefaultString(this string that) {
            return string.IsNullOrEmpty(that)
                || string.Equals(that, ConstsLibrary.Default, StringComparison.OrdinalIgnoreCase);
        }
    }
}
