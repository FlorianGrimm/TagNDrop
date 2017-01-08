using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagNDropLibrary {
    /// <summary>Source of Search.</summary>
    public interface ITNDSourceBuis {
        /// <summary>Initialize this.</summary>
        /// <param name="applicationBuis">application buissness</param>
        /// <param name="metaSource">the configuration.</param>
        /// <returns>this or alternate to use.</returns>
        ITNDSourceBuis Init(ITNDApplicationBuis applicationBuis, TNDMetaSource metaSource);

        /// <summary>
        /// Search for the term.
        /// </summary>
        /// <param name="metaEntity">default metaEntity.</param>
        /// <param name="searchTerm">the terms to search.</param>
        /// <param name="emails">the emails</param>
        /// <param name="emailDomains">the emails domains</param>
        /// <returns>the search result</returns>
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
