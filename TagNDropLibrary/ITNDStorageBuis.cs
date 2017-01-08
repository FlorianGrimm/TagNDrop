using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagNDropLibrary {
    public interface ITNDStorageBuis {
        ITNDStorageBuis Init(ITNDApplicationBuis applicationBuis, TNDMetaStorage metaStorage);
        Task PreSaveValidationAsync(TNDStoreItem storeItem);
        Task SaveAsync(TNDStoreItem storeItem);
    }
}
