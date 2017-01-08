using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagNDropLibrary {

    public class TNDSearchModel : TNDNotifyPropertyChanged {
        public ITNDApplicationBuis ApplicationBuis;
        public TNDConfiguration Configuration;
        //
        private string _Status;
        private TNDTargetModel _SelectedTarget;
        private ITNDSearchBuis _SearchBuis;

        public TNDSearchModel() {
            this.Targets = new BulkObservableCollection<TNDTargetModel>();
        }

        public string Status { get { return this._Status; } set { this._Status = value; this.OnPropertyChanged(nameof(this.Status)); } }

        public TNDTargetModel SelectedTarget { get { return this._SelectedTarget; } set { this._SelectedTarget = value; this.OnPropertyChanged(nameof(this.SelectedTarget)); } }

        public BulkObservableCollection<TNDTargetModel> Targets { get; }

        public void Init(ITNDApplicationBuis applicationBuis) {
            this.ApplicationBuis = applicationBuis;
        }

        public void ApplyConfiguration(TNDConfiguration configuration) {
            this.Configuration = configuration;
            if (this.Targets.Any()) {
                foreach (var target in this.Targets.ToArray()) {
                    target.ApplyConfiguration(configuration);
                }
            }
        }

        public void AddTargets(TNDTargetModel[] targets) {
            if (targets != null && targets.Length > 0) {
                foreach (var target in targets) {
                    target.ApplyConfiguration(this.Configuration);
                }
                this.Targets.AddRange(targets);
            }
            if (this.Targets.Count > 0 && this.SelectedTarget == null) {
                this.SelectedTarget = this.Targets.FirstOrDefault();
            }
            foreach (var target in targets) {
                target.PrepareSearch();
            }
        }

        public bool IsSaving() {
            return this.GetBuis().IsSaving();
        }

        public async Task Search(TNDTargetModel targetModel, TNDSearchItem searchItem) {
            var taskSearch = this.GetBuis().Search(targetModel, searchItem);
            try {
                await taskSearch;
            } catch (Exception exception) {
                throw;
            }
        }

        public async Task Query(TNDTargetModel targetModel, TNDEntityItem entityItem, bool returnParent, bool returnThis, bool returnChildren) {
            var taskSearch = this.GetBuis().Query(targetModel, entityItem.MetaEntity, true, new object[] { entityItem.GetKeyValue() }, returnParent, returnThis, returnChildren);
            try {
                await taskSearch;
            } catch (Exception exception) {
                throw;
            }
        }

        public void AddCheckedItem(TNDEntityItem entityItem) {
            var selectedTarget = this.SelectedTarget;
            if (selectedTarget != null) {
                selectedTarget.AddCheckedItem(entityItem);
            }
        }

        public void RemoveCheckedItem(TNDEntityItem entityItem) {
            var selectedTarget = this.SelectedTarget;
            if (selectedTarget != null) {
                selectedTarget.RemoveCheckedItem(entityItem);
            }
        }

        public void ClearDrops() {
            this.GetBuis().ClearDrops();
        }

        public void ClearSearchItems() {
            this.GetBuis().ClearSearchItems();
        }

        public void ClearCheckedItems() {
            this.GetBuis().ClearCheckedItems();
        }

        public Task<TNDStoreItem[]> Save() {
            return this.GetBuis().Save();
        }

        [System.Diagnostics.DebuggerStepThrough]
        private ITNDSearchBuis GetBuis() {
            return this._SearchBuis ?? (this._SearchBuis = PoorMansDI.Instance.Create<ITNDSearchBuis>().Init(this));
        }
    }
}
