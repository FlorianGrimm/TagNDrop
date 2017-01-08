using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagNDropLibrary
{
    public class TNDTargetModel : TNDNotifyPropertyChanged
    {
        private ITNDApplicationBuis _ApplicationBuis;
        private ITNDDataAccess _DataAccess;
        private ITNDTargetBuis _TargetBuis;
        private TNDDropItem _DropItem;
        private TNDSearchItem _CurrentSearchItem;
        private TNDEntityItem _SelectedEntityItem;
        private TNDEntityItem _SelectedCheckedItem;
        private bool _Searching;
        private int _SearchingCount;
        private bool _CanSave;
        private bool _IsSaving;
        private int _SavingCount;
        private bool _ShowListOrView;

        public TNDTargetModel()
        {
            this.StoreItems = new BulkObservableCollection<TNDStoreItem>();
            this.EntityItems = new BulkObservableCollection<TNDEntityItem>();
            this.CheckedItems = new BulkObservableCollection<TNDEntityItem>();
            this.SearchItems = new BulkObservableCollection<TNDSearchItem>();
            ((INotifyPropertyChanged)this.CheckedItems).PropertyChanged += checkedItemsPropertyChanged;
        }

        public TNDFavorite Favorite { get; set; }

        public TNDDropItem DropItem { get { return this._DropItem; } set { this._DropItem = value; this.OnPropertyChanged(nameof(this.DropItem)); } }

        public BulkObservableCollection<TNDSearchItem> SearchItems { get; }

        public bool IsSearching { get { return this._Searching; } set { this._Searching = value; this.OnPropertyChanged(nameof(this.IsSearching)); } }

        public TNDSearchItem CurrentSearchItem { get { return this._CurrentSearchItem; } set { this._CurrentSearchItem = value; this.OnPropertyChanged(nameof(this.CurrentSearchItem)); } }

        public TNDEntityItem SelectedEntityItem { get { return this._SelectedEntityItem; } set { this._SelectedEntityItem = value; this.OnPropertyChanged(nameof(this.SelectedEntityItem)); } }

        public TNDEntityItem SelectedCheckedItem { get { return this._SelectedCheckedItem; } set { this._SelectedCheckedItem = value; this.OnPropertyChanged(nameof(this.SelectedCheckedItem)); } }

        public bool ShowListOrView { get { return this._ShowListOrView; } set { this._ShowListOrView = value; this.OnPropertyChanged(nameof(this.ShowListOrView)); } }

        public BulkObservableCollection<TNDEntityItem> EntityItems { get; }

        public BulkObservableCollection<TNDEntityItem> CheckedItems { get; }

        public bool CanSave { get { return this._CanSave; } set { this._CanSave = value; this.OnPropertyChanged(nameof(this.CanSave)); } }

        public BulkObservableCollection<TNDStoreItem> StoreItems { get; }

        public bool IsSaving { get { return this._IsSaving; } set { this._IsSaving = value; this.OnPropertyChanged(nameof(this.IsSaving)); } }

        public void Init(ITNDDataAccess dataAccess, ITNDApplicationBuis applicationBuis)
        {
            this._ApplicationBuis = applicationBuis;
            this._DataAccess = dataAccess;
        }
        public void ApplyConfiguration(TNDConfiguration configuration)
        {
            var metaEntities = configuration.MetaEntities.ToArray();
            {
                var item = new TNDSearchItem() { MetaEntityName = ConstsLibrary.SearchAll, Search = string.Empty, MetaEntity = null };
                this.SearchItems.Add(item);
            }
            foreach (var metaEntity in metaEntities)
            {
                var item = new TNDSearchItem() { MetaEntityName = metaEntity.MetaEntityName, Search = string.Empty, MetaEntity = metaEntity };
                this.SearchItems.Add(item);
            }
        }

        public void PrepareSearch()
        {
            var favorite = this.Favorite;
            if (favorite != null)
            {
                var searchItem = this.SearchItems.FirstOrDefault(_ => _.MetaEntityName == favorite.MetaEntityName);
                if (searchItem != null)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var property in favorite.Property)
                    {
                        string valueString = property.ValueString;
                        if (!string.IsNullOrEmpty(valueString))
                        {
                            sb.Append(valueString);
                            sb.Append(" ");
                        }
                    }
                    searchItem.Search = sb.ToString();
                    return;
                }
            }
            var dropItem = this.DropItem;
            if (dropItem != null)
            {
                var displayName = dropItem.DisplayName;
                var searchItem = this.SearchItems.FirstOrDefault();
                if (searchItem != null)
                {
                    searchItem.Search = displayName.Replace(".", " ").Replace("-", " ").Replace("_", " ");
                }
            }
        }

        public void StartSearch()
        {
            this.IsSearching = true;
            this._SearchingCount++;
        }
        public void FinishSearch()
        {
            this._SearchingCount--;
            this.IsSearching = (this._SearchingCount > 0);
        }

        public void AddCheckedItem(TNDEntityItem entityItem)
        {
            if (entityItem == null) { return; }
            entityItem.IsChecked = true;
            var idxCheckedItem = this.GetCheckedItem(entityItem);
            if (idxCheckedItem == null)
            {
                this.CheckedItems.Add(entityItem);
            }
            else
            {
                this.CheckedItems.RemoveAt(idxCheckedItem.Item1);
                this.CheckedItems.Add(entityItem);
            }
        }

        public void RemoveCheckedItem(TNDEntityItem entityItem)
        {
            if (entityItem == null) { return; }
            entityItem.IsChecked = false;
            var idxCheckedItem = this.GetCheckedItem(entityItem);
            if (idxCheckedItem == null)
            {
                // do nothing
            }
            else
            {
                this.CheckedItems.RemoveAt(idxCheckedItem.Item1);
                idxCheckedItem.Item2.IsChecked = false;
            }
        }

        public Tuple<int, TNDEntityItem> GetCheckedItem(TNDEntityItem entityItem)
        {
            for (int idx = 0; idx < this.CheckedItems.Count; idx++)
            {
                TNDEntityItem checkedItem = this.CheckedItems[idx];
                if (checkedItem == null) { continue; }
                if (entityItem.Equals(checkedItem))
                {
                    return Tuple.Create(idx, checkedItem);
                }
            }
            return null;
        }

        public void StartSave()
        {
            this.IsSaving = true;
            this._SavingCount++;
        }
        public void FinishSave()
        {
            this._SavingCount--;
            this.IsSaving = (this._SearchingCount > 0);
        }
        private void checkedItemsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var value = this.CheckedItems.Count > 0;
            if (this.CanSave != value)
            {
                this.CanSave = value;
            }
        }

        [System.Diagnostics.DebuggerStepThrough]
        private ITNDTargetBuis GetBuis()
        {
            return this._TargetBuis ?? (this._TargetBuis = PoorMansDI.Instance.Create<ITNDTargetBuis>().Init(this, this._DataAccess, this._ApplicationBuis));
        }
    }
}
