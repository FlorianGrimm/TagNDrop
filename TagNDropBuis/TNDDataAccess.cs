using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagNDropLibrary;

namespace TagNDropBuis {
    public class TNDDataAccess : ITNDDataAccess {
        private ITNDApplicationBuis _ApplicationBuis;
        private UTF8Encoding _UTF8NoBOM;

        public TNDDataAccess() { }

        public ITNDDataAccess Init(ITNDApplicationBuis applicationBuis) {
            this._ApplicationBuis = applicationBuis;
            return this;
        }

        public TNDFavorite[] LoadFavorites() {
            string favoritesPath = this.GetFavoritesPath();
            if (System.IO.File.Exists(favoritesPath)) {
                var favoritesContent = System.IO.File.ReadAllText(favoritesPath);
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<TNDFavorite[]>(favoritesContent) ?? new TNDFavorite[0];
                return result;
            }
            // fallback
            return new TNDFavorite[0];
        }

        public async Task SaveFavorites(TNDFavorite[] items) {
            string favoritesPath = this.GetFavoritesPath();
            var dirName = System.IO.Path.GetDirectoryName(favoritesPath);
            if (!System.IO.Directory.Exists(dirName)) {
                System.IO.Directory.CreateDirectory(dirName);
            }
            var jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(items);
            using (var sw = new System.IO.StreamWriter(favoritesPath, false, this._UTF8NoBOM ?? (this._UTF8NoBOM = new UTF8Encoding(false, true)))) {
                try {
                    await sw.WriteAsync(jsonContent);
                } catch (Exception exception) {
                    return;
                }
            }
        }

        public async Task InitalSearch(TNDFavorite favorite, TNDDropItem dropItem) {
            //TNDMetaEntity metaEntity = null;
            if (favorite != null) {
                //metaEntity = favorite.MetaEntity;
                //this.Search(favorite, dropItem);
            } else {
            }
            await Task.Delay(0);
            //if (metaEntity != null) { }
            // dropItem.GetProperty("Subject")
            // TODO
        }

        public async Task<TNDEntityItem[]> SearchTerm(TNDMetaEntity metaEntity, string searchTerm, string emails, string emailDomains) {
            var configuration = this._ApplicationBuis.LoadConfiguration(false);
            var metaSource = metaEntity?.MetaSource;
            if (metaSource == null) {
                var tasksSearchTerm = new List<Task<TNDEntityItem[]>>();
                foreach (var metaEnt in configuration.MetaEntities) {
                    var sourceBuis = metaEnt?.MetaSource.SourceBuis;
                    if (sourceBuis != null) {
                        var taskSearchTerm = sourceBuis.SearchTerm(metaEnt, searchTerm, emails, emailDomains);
                        tasksSearchTerm.Add(taskSearchTerm);
                    }
                }
                if (tasksSearchTerm.Any()) {
                    var taskWhenAll = Task.WhenAll(tasksSearchTerm);
                    TNDEntityItem[][] results = null;
                    try {
                        results = await taskWhenAll;
                    } catch (Exception exception) {
                        throw;
                    }
                    // flatten
                    var result = new List<TNDEntityItem>();
                    foreach (var r in results) {
                        if (r != null && r.Any()) {
                            result.AddRange(r);
                        }
                    }
                    return SortHelper.Sort(result.ToArray());
                }
                return new TNDEntityItem[0];
            } else {
                var sourceBuis = metaSource.SourceBuis;
                if (sourceBuis != null) {
                    var taskSearchTerm = sourceBuis.SearchTerm(metaEntity, searchTerm, emails, emailDomains);
                    TNDEntityItem[] result = null;
                    try {
                        result = await taskSearchTerm;
                    } catch (Exception exception) {
                        throw;
                    }
                    return result;
                    //metaSource.Configuration;
                }
            }
            return null;
        }

        /// <summary>Query for entities by id</summary>
        /// <param name="metaEntity">default metaEntity.</param>
        /// <param name="ids">the ids</param>
        /// <param name="returnParent">true to return also parent.</param>
        /// <param name="returnThis">true to return this entity type.</param>
        /// <param name="returnChildren">true to return children.</param>
        /// <returns>the search result</returns>
        public virtual async Task<TNDEntityItem[]> Query(TNDMetaEntity metaEntity, string ids, bool returnParent, bool returnThis, bool returnChildren) {
            var configuration = this._ApplicationBuis.LoadConfiguration(false);
            var metaSource = metaEntity?.MetaSource;
            var sourceBuis = metaSource.SourceBuis;
            if (sourceBuis != null) {
                var taskQuery = sourceBuis.Query(metaEntity, ids, returnParent, returnThis, returnChildren);
                TNDEntityItem[] result = null;
                try {
                    result = await taskQuery;
                } catch (Exception exception) {
                    throw;
                }
                //SortHelper.Sort(result);
                return result;
            }
            return null;
        }
        private string GetFavoritesPath() {
            string favoritesPath = this._ApplicationBuis.ApplicationModel.FavoritesPath;
            if (string.IsNullOrEmpty(favoritesPath)) {
                var appdata = System.Environment.ExpandEnvironmentVariables("%APPDATA%");
                var fn = System.IO.Path.Combine(appdata, ConstsLibrary.Company, ConstsLibrary.TagNDropFavoritesJson);
                favoritesPath = fn;
            }
            return favoritesPath;
        }
    }
}
