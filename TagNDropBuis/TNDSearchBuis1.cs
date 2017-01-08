using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagNDropLibrary;

namespace TagNDropBuis {
    public class TNDSearchBuis1 : ITNDSearchBuis {
        protected ITNDApplicationBuis _ApplicationBuis;
        protected TNDSearchModel _SearchModel;

        public TNDSearchBuis1() { }

        public virtual ITNDSearchBuis Init(TNDSearchModel searchModel) {
            this._SearchModel = searchModel;
            return this;
        }

        public virtual async Task Search(TNDTargetModel targetModel, TNDSearchItem searchItem) {
            if (searchItem == null) {
                //
                //this._ApplicationBuis.GetDataAccess().InitalSearch()
            } else {
                //
                targetModel.StartSearch();
                try {
                    //
                    var entityItems = targetModel.EntityItems;
                    var checkedItems = targetModel.CheckedItems;
                    searchItem.ParseSearch();
                    var taskSearchResult = this._ApplicationBuis.GetDataAccess().SearchTerm(
                        searchItem.MetaEntity,
                        searchItem.SearchTerm,
                        searchItem.Emails,
                        searchItem.EmailDomains);
                    TNDEntityItem[] arrSearchResults = null;
                    try {
                        arrSearchResults = await taskSearchResult.ConfigureAwait(true);
                    } catch (Exception exception) {
                        throw;
#warning exception
                    }
                    replaceEntityItems(entityItems, checkedItems, arrSearchResults);
                } finally {
                    targetModel.FinishSearch();
                }
            }
        }

        /// <summary>Query for entities by id</summary>
        /// <param name="targetModel">the taget</param>
        /// <param name="metaEntity">default metaEntity.</param>
        /// <param name="addOrReplace">true to add; false to replace</param>
        /// <param name="ids">the ids</param>
        /// <param name="returnParent">true to return also parent.</param>
        /// <param name="returnThis">true to return this entity type.</param>
        /// <param name="returnChildren">true to return children.</param>
        /// <returns>the search result</returns>
        public virtual async Task Query(TNDTargetModel targetModel, TNDMetaEntity metaEntity, bool addOrReplace, System.Collections.IEnumerable ids, bool returnParent, bool returnThis, bool returnChildren) {
            string idsText = getIds(ids);
            idsText = idsText.Trim();
            var entityItems = targetModel.EntityItems;
            var checkedItems = targetModel.CheckedItems;
            var taskSearchResult = this._ApplicationBuis.GetDataAccess().Query(
                    metaEntity,
                    idsText,
                    true,
                    true,
                    true);
            TNDEntityItem[] arrSearchResults = null;
            try {
                arrSearchResults = await taskSearchResult;
            } catch (Exception exception) {
                throw;
#warning exception
            }
            if (addOrReplace) {
                addEntityItems(entityItems, checkedItems, arrSearchResults);
            } else {
                replaceEntityItems(entityItems, checkedItems, arrSearchResults);
            }
#if false
            if (arrSearchResults == null || !arrSearchResults.Any()) {
                entityItems.Clear();
            } else {
                entityItems.Clear();
                var dictCheckedItems = checkedItems.ToDictionary(_ => _);
                //
                var newCheckedItems = new List<TNDEntityItem>();
                // add the found ones - uses the found.
                foreach (var searchResult in arrSearchResults) {
                    if (dictCheckedItems.Remove(searchResult)) {
                        searchResult.IsChecked = true;
                        newCheckedItems.Add(searchResult);
                    }
                }
                // add the not found ones - reuse the old.
                newCheckedItems.AddRange(dictCheckedItems.Keys);
                //
                var sortedNewCheckedItems
                    = newCheckedItems
                    .OrderBy(_ => _.MetaEntity.MetaEntityLevel)
                    .ThenBy(_ => _.MetaEntityName)
                    .ThenBy(_ => _.Name)
                    .ToArray();
                checkedItems.Replace(newCheckedItems);
                entityItems.Replace(arrSearchResults);
            }
#endif
        }

        public virtual void ClearDrops() {
            foreach (var target in this._SearchModel.Targets) {
                if (!target.IsSaving) {
                    this.RemoveTarget(target);
                }
            }
            //if (this._SearchModel.Targets.Count == 0) {
            //    this._ApplicationBuis.ShowDropWindow();
            //}
        }

        public virtual void ClearSearchItems() {
            var selectedTarget = this._SearchModel.SelectedTarget;
            if (selectedTarget != null) {
                foreach (var searchItem in selectedTarget.SearchItems) {
                    searchItem.Search = string.Empty;
                }
            }
        }

        public virtual void ClearCheckedItems() {
            var selectedTarget = this._SearchModel.SelectedTarget;
            if (selectedTarget != null) {
                foreach (var checkedItem in selectedTarget.CheckedItems.ToArray()) {
                    selectedTarget.RemoveCheckedItem(checkedItem);
                }
            }
            this.RemoveTarget(selectedTarget);
        }

        public virtual async Task<TNDStoreItem[]> PreSaveValidation() {
            await Task.Delay(1);
            var selectedTarget = this._SearchModel.SelectedTarget;
            if (selectedTarget != null) {
                var result = new List<TNDStoreItem>();
                var lstTasks = new List<Task>();
                var checkedItems = selectedTarget.CheckedItems;
                lock (checkedItems) {
                    foreach (var checkedItem in checkedItems.ToArray()) {
                        var storeItem = this.GetOrCreateStoreItem(selectedTarget, selectedTarget.DropItem, checkedItem);
                        try {
                            var metaStorage = checkedItem.MetaEntity.MetaStorage;
                            var taskPreSaveValidation = metaStorage.StorageBuis.PreSaveValidationAsync(storeItem);
                            lstTasks.Add(taskPreSaveValidation);
                            result.Add(storeItem);
                        } catch (Exception exception) {
                            storeItem.ProcessText = "Exception: " + exception.Message;
                            storeItem.Done = true;
                        }
                    }
                }
                {
                    var taskWhenAll = Task.WhenAll(lstTasks).ConfigureAwait(true);
                    try {
                        await taskWhenAll;
                    } catch (Exception exception) {
                    }
                }
                return result.ToArray();
            }
            return new TNDStoreItem[0];
        }

        public virtual async Task<TNDStoreItem[]> Save() {
            var selectedTarget = this._SearchModel.SelectedTarget;
            bool removeTarget = true;
            if (selectedTarget != null) {
                try {
                    selectedTarget.StartSave();
                    var result = new List<TNDStoreItem>();
                    var lstTasks = new List<Task>();
                    var checkedItems = selectedTarget.CheckedItems;
                    lock (checkedItems) {
                        foreach (var checkedItem in checkedItems.ToArray()) {
                            var metaStorage = checkedItem.MetaEntity.MetaStorage;
                            var storeItem = this.GetOrCreateStoreItem(selectedTarget, selectedTarget.DropItem, checkedItem);
                            if (storeItem.Done) { continue; }
                            try {
                                var taskSave = metaStorage.StorageBuis.SaveAsync(storeItem);
                                lstTasks.Add(taskSave);
                                result.Add(storeItem);
                            } catch (Exception exception) {
                                storeItem.ProcessText = "Exception: " + exception.Message;
                                storeItem.Done = true;
                            }
                            selectedTarget.RemoveCheckedItem(checkedItem);
                        }
                    }
                    {
                        var taskWhenAll = Task.WhenAll(lstTasks).ConfigureAwait(true);
                        try {
                            await taskWhenAll;
                        } catch (Exception exception) {
                            removeTarget = false;
                        }
                    }
                    {
                        var favorites = result
                            .Select(storeItem => TNDDefaultStorageBuis.ConvertToFavorit(storeItem))
                            .Where(_ => _ != null)
                            .ToArray();
                        this._ApplicationBuis.AddToFavorites(favorites);
                        var taskSaveFavorites = this._ApplicationBuis.SaveFavoritesAsync();
                        try {
                            await taskSaveFavorites;
                        } catch (Exception exception) {
                            removeTarget = false;
                        }
                    }
                    return result.ToArray();
                } finally {
                    selectedTarget.FinishSave();
                    if (removeTarget) {
                        this.RemoveTarget(selectedTarget);
                    }
                }
            }
            return new TNDStoreItem[0];
        }

        private TNDStoreItem GetOrCreateStoreItem(TNDTargetModel selectedTarget, TNDDropItem dropItem, TNDEntityItem checkedItem) {
            TNDStoreItem storeItem = null;
            var storeItems = selectedTarget.StoreItems;
            lock (storeItems) {
                storeItem = storeItems.FirstOrDefault(si => ReferenceEquals(si.CheckedItem, checkedItem));
                if (storeItem == null) {
                    var metaStorage = checkedItem.MetaEntity.MetaStorage;
                    storeItem = new TNDStoreItem();
                    storeItem.DropItem = dropItem;
                    storeItem.CheckedItem = checkedItem;
                    storeItems.Add(storeItem);
                }
            }
            return storeItem;
        }

        public bool IsSaving() {
            foreach (var target in this._SearchModel.Targets.ToArray()) {
                if (target.IsSaving) {
                    return true;
                }
            }
            return false;
        }

        private static string getIds(System.Collections.IEnumerable ids) {
            var e = ids.GetEnumerator();
            var lstIds = new List<string>();
            using (var d = e as IDisposable) {
                while (e.MoveNext()) {
                    var id = e.Current;
                    if (id == null) { continue; }
                    if (id is TNDProperty) { id = ((TNDProperty)id).Value; }
                    if (id is string) {
                        lstIds.Add((string)id);
                    } else if (id is int) {
                        lstIds.Add(((int)id).ToString());
                    } else if (id is Guid) {
                        lstIds.Add(((Guid)id).ToString("D"));
                    } else {
                        lstIds.Add(id.ToString());
                    }
                }
            }
            var idsText = string.Join(",", lstIds.Select(_ => _.Replace("\\", "\\!").Replace(",", "\\;")));
            return idsText;
        }

        private static void addEntityItems(BulkObservableCollection<TNDEntityItem> entityItems, BulkObservableCollection<TNDEntityItem> checkedItems, TNDEntityItem[] arrSearchResults) {
            if (arrSearchResults == null || !arrSearchResults.Any()) {
                //entityItems.Clear();
            } else {
                //entityItems.Clear();
                var dictCheckedItems = checkedItems.ToDictionary(_ => _);
                var dictEntityItems = new Dictionary<TNDEntityItem, int>();
                {
                    int idx = 0;
                    foreach (var entityItem in entityItems) {
                        dictEntityItems[entityItem] = idx;
                        idx++;
                    }
                }
                //
                var oldEntityItems = new List<TNDEntityItem>(entityItems);
                var newCheckedItems = new List<TNDEntityItem>();
                // add the found ones - uses the found.
                foreach (var searchResult in arrSearchResults) {
                    int idx;
                    if (dictEntityItems.TryGetValue(searchResult, out idx)) {
                        var oldMatch = oldEntityItems[idx]?.GetProperty("Match")?.GetValueAsInt();
                        var currentMatch = searchResult.GetProperty("Match")?.GetValueAsInt();
                        oldEntityItems[idx] = null;
                        if (oldMatch.HasValue && (oldMatch.GetValueOrDefault() > currentMatch.GetValueOrDefault())) {
                            searchResult.SetProperty<int>("Match", oldMatch.GetValueOrDefault());
                        }
                    }
                    if (dictCheckedItems.Remove(searchResult)) {
                        searchResult.IsChecked = true;
                        newCheckedItems.Add(searchResult);
                    }
                }
                // add the not found ones - reuse the old.
                newCheckedItems.AddRange(dictCheckedItems.Keys);
                var lstSearchResults = new List<TNDEntityItem>(oldEntityItems.Count + arrSearchResults.Length);
                lstSearchResults.AddRange(oldEntityItems.Where(_ => _ != null));
                lstSearchResults.AddRange(arrSearchResults);
                arrSearchResults = SortHelper.Sort(lstSearchResults.ToArray());
                //
                var sortedNewCheckedItems
                    = newCheckedItems
                    .OrderBy(_ => _.MetaEntity.MetaEntityLevel)
                    .ThenBy(_ => _.MetaEntityName)
                    .ThenBy(_ => _.Name)
                    .ToArray();
                lock (entityItems) {
                    checkedItems.Replace(newCheckedItems);
                    entityItems.Replace(arrSearchResults);
                }
            }
        }

        private static void replaceEntityItems(BulkObservableCollection<TNDEntityItem> entityItems, BulkObservableCollection<TNDEntityItem> checkedItems, TNDEntityItem[] arrSearchResults) {
            if (arrSearchResults == null || !arrSearchResults.Any()) {
                entityItems.Clear();
            } else {
                entityItems.Clear();
                var dictCheckedItems = checkedItems.ToDictionary(_ => _);
                //
                var newCheckedItems = new List<TNDEntityItem>();
                // add the found ones - uses the found.
                foreach (var searchResult in arrSearchResults) {
                    if (dictCheckedItems.Remove(searchResult)) {
                        searchResult.IsChecked = true;
                        newCheckedItems.Add(searchResult);
                    }
                }
                // add the not found ones - reuse the old.
                newCheckedItems.AddRange(dictCheckedItems.Keys);
                //
                var sortedNewCheckedItems
                    = newCheckedItems
                    .OrderBy(_ => _.MetaEntity.MetaEntityLevel)
                    .ThenBy(_ => _.MetaEntityName)
                    .ThenBy(_ => _.Name)
                    .ToArray();
                lock (entityItems) {
                    checkedItems.Replace(newCheckedItems);
                    entityItems.Replace(arrSearchResults);
                }
            }
        }

        private void RemoveTarget(TNDTargetModel selectedTarget) {
            if (ReferenceEquals(this._SearchModel.SelectedTarget, selectedTarget)) {
                this._SearchModel.SelectedTarget = this._SearchModel.Targets.FirstOrDefault(t => !ReferenceEquals(t, selectedTarget));
            }
            this._SearchModel.Targets.Remove(selectedTarget);
            if (this._SearchModel.Targets.Count == 0) {
                this._ApplicationBuis.ShowDropWindow();
            }
        }

    }
}
