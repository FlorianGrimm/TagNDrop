using System.Threading.Tasks;

namespace TagNDropLibrary {
    /// <summary>Provide search.</summary>
    public interface ITNDSearchBuis {
        /// <summary>Initialize this instance.</summary>
        /// <param name="searchModel">the owner</param>
        /// <returns>return buis to use.</returns>
        ITNDSearchBuis Init(TNDSearchModel searchModel);

        Task Search(TNDTargetModel targetModel, TNDSearchItem searchItem);

        /// <summary>Query for entities by id</summary>
        /// <param name="targetModel">the taget</param>
        /// <param name="metaEntity">default metaEntity.</param>
        /// <param name="addOrReplace">true to add; false to replace</param>
        /// <param name="ids">the ids</param>
        /// <param name="returnParent">true to return also parent.</param>
        /// <param name="returnThis">true to return this entity type.</param>
        /// <param name="returnChildren">true to return children.</param>
        /// <returns>the search result</returns>
        Task Query(TNDTargetModel targetModel, TNDMetaEntity metaEntity, bool addOrReplace, System.Collections.IEnumerable ids, bool returnParent, bool returnThis, bool returnChildren);

        void ClearDrops();

        void ClearSearchItems();

        void ClearCheckedItems();

        bool IsSaving();

        Task<TNDStoreItem[]> PreSaveValidation();

        Task<TNDStoreItem[]> Save();
    }
}
