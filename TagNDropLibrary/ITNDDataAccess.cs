using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagNDropLibrary {
    public interface ITNDDataAccess {
        ITNDDataAccess Init(ITNDApplicationBuis applicationBuis);

        TNDFavorite[] LoadFavorites();

        Task SaveFavorites(TNDFavorite[] items);

        Task InitalSearch(TNDFavorite favorite, TNDDropItem dropItem);

        Task<TNDEntityItem[]> SearchTerm(TNDMetaEntity metaEntity, string searchTerm, string emails, string emailDomains);

        /// <summary>Query for entities by id</summary>
        /// <param name="metaEntity">default metaEntity.</param>
        /// <param name="ids">the ids</param>
        /// <param name="returnParent">true to return also parent.</param>
        /// <param name="returnThis">true to return this entity type.</param>
        /// <param name="returnChildren">true to return children.</param>
        /// <returns>the search result</returns>
        Task<TNDEntityItem[]> Query(TNDMetaEntity metaEntity, string ids, bool returnParent, bool returnThis, bool returnChildren);
    }
}
