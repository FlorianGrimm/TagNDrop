using System.Collections.ObjectModel;
using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace TagNDropLibrary {
    [JsonObject(MemberSerialization.OptIn)]
    public class TNDFavorite : TNDBaseProperty {
        public TNDMetaEntity MetaEntity;
        public int Order;
        //
        private string _Id;
        private string _Name;
        private int _Used;
        private string _MetaEntityName;
        private double _OrderPercent;
        private bool _IsDragging;
        private DateTime _LastUsed;
        private double _LastUsedPercent;

        public TNDFavorite() {
        }

        public TNDFavorite(string name, params TNDProperty[] properties) {
            this.Name = name;
            if (properties != null && properties.Length > 0) {
                this.Property.AddRange(properties);
            }
        }

        [JsonProperty]
        public string Id { get { return this._Id; } set { this._Id = value; this.OnPropertyChanged(nameof(this.Id)); this.OnPropertyChanged(nameof(this.IdOrName)); } }

        [JsonProperty]
        public string Name { get { return this._Name; } set { this._Name = value; this.OnPropertyChanged(nameof(this.Name)); this.OnPropertyChanged(nameof(this.IdOrName)); } }

        public string IdOrName { get { return this._Id ?? this._Name; } }

        [JsonProperty]
        public int Used { get { return this._Used; } set { this._Used = value; this.OnPropertyChanged(nameof(this.Used)); } }

        [JsonProperty]
        public string MetaEntityName { get { return this._MetaEntityName; } set { this._MetaEntityName = value; this.OnPropertyChanged(nameof(this.MetaEntityName)); } }

        [JsonProperty]
        public DateTime LastUsed { get { return this._LastUsed; } set { this._LastUsed = value; this.OnPropertyChanged(nameof(this.LastUsed)); } }

        [JsonIgnore]
        public double OrderPercent { get { return this._OrderPercent; } set { this._OrderPercent = value; this.OnPropertyChanged(nameof(this.OrderPercent)); } }

        [JsonIgnore]
        public double LastUsedPercent { get { return this._LastUsedPercent; } set { this._LastUsedPercent = value; this.OnPropertyChanged(nameof(this.LastUsed)); } }

        public bool IsDragging { get { return this._IsDragging; } set { this._IsDragging = value; this.OnPropertyChanged(nameof(this.IsDragging)); } }

        public void Merge(TNDFavorite fav) {
            this.LastUsed = (this.LastUsed > fav.LastUsed) ? this.LastUsed : fav.LastUsed;
            this.SetProperties(fav.Property);
            this.Used += System.Math.Min(1, fav.Used);
        }
    }
}
