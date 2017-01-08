using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagNDropLibrary;

namespace TagNDropBuis {
    public class TNDPropertyCache {
        private readonly Dictionary<TNDProperty, TNDProperty> _Cache;
        public TNDPropertyCache() {
            this._Cache = new Dictionary<TNDProperty, TNDProperty>();
        }

        public TNDProperty GetOrCreate(TNDProperty property) {
            TNDProperty result;
            this._Cache.TryGetValue(property, out result);
            if (result == null) {
                this._Cache[property] = property;
                return property;
            } else {
                return result;
            }
        }
    }
}
