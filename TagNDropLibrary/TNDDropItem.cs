using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagNDropLibrary {
    public class TNDDropItem : TNDBaseProperty {
        private string _Name;
        private string _DisplayName;
        private byte[] _Content;
        private Exception _LastException;

        public TNDDropItem() {
        }

        public string Name { get { return this._Name; } set { this._Name = value; this.OnPropertyChanged(nameof(this.Name)); } }

        public string DisplayName { get { return this._DisplayName; } set { this._DisplayName = value; this.OnPropertyChanged(nameof(this.DisplayName)); } }

        public Exception LastException { get { return this._LastException; } set { this._LastException = value; this.OnPropertyChanged(nameof(this.LastException)); } }

        public byte[] Content { get { return this._Content; } set { this._Content = value; this.OnPropertyChanged(nameof(this.Content)); } }
    }
}
