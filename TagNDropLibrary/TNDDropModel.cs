using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TagNDropLibrary {
    public class TNDDropModel : TNDNotifyPropertyChanged {
        private string _Status;
        private bool _IsDragging;
        private TNDFavorite _TNDFavorite;
        private ITNDDropBuis _DropBuis;

        public TNDDropModel() {
            this.EmptyFavorite = new TNDFavorite();
            this.Favorite = new BulkObservableCollection<TNDFavorite>();
            this.Drops = new BulkObservableCollection<TNDDropItem>();
        }

        public string Status { get { return this._Status; } set { this._Status = value; this.OnPropertyChanged(nameof(this.Status)); } }

        public bool IsDragging { get { return this._IsDragging; } set { this._IsDragging = value; this.OnPropertyChanged(nameof(this.IsDragging)); } }

        public TNDFavorite DnDTarget {
            get {
                return this._TNDFavorite;
            }
            set {
                if (ReferenceEquals(this._TNDFavorite, value)) {
                    return;
                }
                if (this._TNDFavorite != null) { this._TNDFavorite.IsDragging = false; }
                this._TNDFavorite = value;
                if (this._TNDFavorite != null) { this._TNDFavorite.IsDragging = true; }
                this.OnPropertyChanged(nameof(this.DnDTarget));
            }
        }

        public TNDFavorite EmptyFavorite { get; }

        public BulkObservableCollection<TNDDropItem> Drops { get; }

        public BulkObservableCollection<TNDFavorite> Favorite { get; }

        public void Init(ITNDDropBuis dropBuis) {
            this._DropBuis = dropBuis;
        }

        public void DoDrop(TNDFavorite favorite, System.Windows.IDataObject data) {
            this.GetBuis().DoDrop(favorite, data);
        }

        public void DoGiveFeedback(TNDFavorite favorite, GiveFeedbackEventArgs e) {
            this.GetBuis().DoGiveFeedback(favorite, e);
        }

        public void DoDragEnter(TNDFavorite favorite, IDataObject data) {
            this.GetBuis().DoDragEnter(favorite, data);
        }

        public void DoPaste(TNDFavorite favorite, IDataObject iDataObject) {
            this.GetBuis().DoPaste(favorite, iDataObject);
        }

        private ITNDDropBuis GetBuis() {
            return this._DropBuis ?? (this._DropBuis = PoorMansDI.Instance.Create<ITNDDropBuis>().Init(this));
        }
    }
}
