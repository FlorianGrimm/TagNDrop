using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagNDropLibrary;

namespace TagNDropBuis {
    public class TNDTargetBuis : ITNDTargetBuis {
        private ITNDApplicationBuis _ApplicationBuis;
        private ITNDDataAccess _DataAccess;

        public TNDTargetBuis() { }

        public ITNDTargetBuis Init(TNDTargetModel targetModel, ITNDDataAccess dataAccess, ITNDApplicationBuis applicationBuis) {
            this._ApplicationBuis = applicationBuis;
            this._DataAccess = dataAccess;
            return this;
        }

    }
}
