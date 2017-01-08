using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagNDropLibrary;
namespace TagNDropBuis
{
    public class TNDDefaultStorageBuis : ITNDStorageBuis
    {
        private ITNDApplicationBuis _ApplicationBuis;
        private TNDMetaStorage _MetaStorage;

        public static void x(TNDStoreItem storeItem)
        {
#warning here            storeItem.Name = "";
        }

        public static TNDFavorite ConvertToFavorit(TNDStoreItem storeItem)
        {
            var result = new TNDFavorite();
            var checkedItem = storeItem.CheckedItem;
            var metaEntity = checkedItem.MetaEntity;
            result.MetaEntityName = checkedItem.MetaEntityName;
            result.Property.AddRange(checkedItem.Property);
            result.Id = checkedItem.GetKeyValue()?.ToString();
            if (result.Id == null) { return null; }
            result.Name = checkedItem.Name;
            result.LastUsed = System.DateTime.Now;
            result.Used = 1;
            return result;
        }

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

        public async Task SaveAsync(TNDStoreItem storeItem)
        {
            storeItem.ProcessText = "Saving...";
            await Task.Delay(1000).ConfigureAwait(true);
            storeItem.ProcessText = "OK - Nothing Done";
            storeItem.Done = true;
        }

    }
}
