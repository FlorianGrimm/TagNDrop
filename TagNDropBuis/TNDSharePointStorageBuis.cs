using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagNDropLibrary;
namespace TagNDropBuis
{
    public class TNDSharePointStorageBuis : ITNDStorageBuis
    {
        private ITNDApplicationBuis _ApplicationBuis;
        private TNDMetaStorage _MetaStorage;

        public ITNDStorageBuis Init(ITNDApplicationBuis applicationBuis, TNDMetaStorage metaStorage)
        {
            this._ApplicationBuis = applicationBuis;
            this._MetaStorage = metaStorage;
            return this;
        }

        public async Task PreSaveValidationAsync(TNDStoreItem storeItem)
        {
            await Task.Delay(1);
        }

        public Task SaveAsync(TNDStoreItem storeItem)
        {
#warning here
            storeItem.ProcessText = "Nothing Done";
            storeItem.Done = true;
            return Task.FromResult(0);
        }
    }
}
