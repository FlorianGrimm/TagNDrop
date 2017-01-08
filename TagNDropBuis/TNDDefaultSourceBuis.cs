using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagNDropLibrary;

namespace TagNDropBuis {
    /// <summary>Does nothing usefull</summary>
    public class TNDDefaultSourceBuis : ITNDSourceBuis {
        private ITNDApplicationBuis _ApplicationBuis;
        private TNDMetaSource _MetaSource;

        /// <summary>Initialize this.</summary>
        /// <param name="applicationBuis">application buissness</param>
        /// <param name="metaSource">the configuration.</param>
        /// <returns>this or alternate to use.</returns>
        public ITNDSourceBuis Init(ITNDApplicationBuis applicationBuis, TNDMetaSource metaSource) {
            this._ApplicationBuis = applicationBuis;
            this._MetaSource = metaSource;
            return this;
        }

        /// <summary>
        /// Search for the term.
        /// </summary>
        /// <param name="metaEntity">default metaEntity.</param>
        /// <param name="searchTerm">the terms to search.</param>
        /// <param name="emails">the emails</param>
        /// <param name="emailDomains">the emails domains</param>
        /// <returns>the search result</returns>
        public Task<TNDEntityItem[]> SearchTerm(TNDMetaEntity metaEntity, string searchTerm, string emails, string emailDomains) {
            return Task.FromResult(new TNDEntityItem[0]);
        }

        /// <summary>
        /// Query for entities by id
        /// </summary>
        /// <param name="metaEntity">default metaEntity.</param>
        /// <param name="ids">the ids</param>
        /// <param name="returnParent">true to return also parent.</param>
        /// <param name="returnThis">true to return this entity type.</param>
        /// <param name="returnChildren">true to return children.</param>
        /// <returns>the search result</returns>
        public Task<TNDEntityItem[]> Query(TNDMetaEntity metaEntity, string ids, bool returnParent, bool returnThis, bool returnChildren) {
            return Task.FromResult(new TNDEntityItem[0]);
        }
    }
}
