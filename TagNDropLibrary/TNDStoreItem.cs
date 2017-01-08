using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagNDropLibrary {
    public class TNDStoreItem : TNDBaseProperty {
        public TNDDropItem DropItem;
        public TNDEntityItem CheckedItem;
        private string _Name;
        private string _MetaEntityName;
        private string _ProcessText;
        private bool _Done;

        public TNDStoreItem() {
        }

        public string Name { get { return this._Name; } set { this._Name = value; this.OnPropertyChanged(nameof(this.Name)); } }

        public string MetaEntityName { get { return this._MetaEntityName; } set { this._MetaEntityName = value; this.OnPropertyChanged(nameof(this.MetaEntityName)); } }

        public string ProcessText { get { return this._ProcessText; } set { this._ProcessText = value; this.OnPropertyChanged(nameof(this.ProcessText)); } }

        public bool Done { get { return this._Done; } set { this._Done = value; this.OnPropertyChanged(nameof(this.Done)); } }
    }
}
