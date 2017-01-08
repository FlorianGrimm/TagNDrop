using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagNDropLibrary {
    [JsonObject(MemberSerialization.OptIn)]
    public class TNDEntityItem : TNDBaseProperty {
        private string _MetaEntityName;
        private bool _IsChecked;
        private double _MatchPercent;

        public TNDEntityItem() {
        }

        [JsonProperty]
        public string MetaEntityName { get { return this._MetaEntityName; } set { this._MetaEntityName = value; this.OnPropertyChanged(nameof(this.MetaEntityName)); } }

        [JsonIgnore]
        public bool IsChecked { get { return this._IsChecked; } set { this._IsChecked = value; this.OnPropertyChanged(nameof(this.IsChecked)); } }

        [JsonIgnore]
        public int Level { get { return (this.MetaEntity?.MetaEntityLevel).GetValueOrDefault(0); } }

        [JsonIgnore]
        public double MatchPercent { get { return this._MatchPercent; } set { this._MatchPercent = value; this.OnPropertyChanged(nameof(this.MatchPercent)); } }

        [JsonIgnore]
        public TNDMetaEntity MetaEntity { get; set; }

        [JsonIgnore]
        public string Name {
            get {
                var propertyName = this.MetaEntity?.MetaEntityNamePropertyName;
                if (string.IsNullOrEmpty(propertyName)) { propertyName = this.MetaEntity.MetaEntityName; }
                if (string.IsNullOrEmpty(propertyName)) { return this.ToString(); }
                var property = this.GetProperty(propertyName);
                if (property != null) { return property.ValueDisplay; }
                //if (property != null) { return $"{property.ValueDisplay}-{this.GetKeyValue()}-{this.GetParentKeyValue()}"; }
                return base.ToString();
            }
        }

        public override string ToString() {
            try {
                var propertyName = this.MetaEntity?.MetaEntityNamePropertyName;
                if (string.IsNullOrEmpty(propertyName)) { propertyName = this.MetaEntity.MetaEntityName; }
                if (string.IsNullOrEmpty(propertyName)) { return this.ToString(); }
                var property = this.GetProperty(propertyName);
                if (property != null) { return property.ValueDisplay; }
                return (this.MetaEntityName ?? string.Empty) + " - " + this.InstanceId.ToString();
            } catch {
                return (this.MetaEntityName ?? string.Empty) + " - " + this.InstanceId.ToString();
            }
        }

        public override int GetHashCode() {
            if (this.MetaEntity == null) {
                return base.GetHashCode();
            } else {
                var propertyName = this.MetaEntity.GetIdOrNamePropertyName();
                object key = this.GetProperty(propertyName);
                var result = this.MetaEntity.MetaEntityName.GetHashCode();
                if (key != null) {
                    result ^= key.GetHashCode();
                }
                return result;
            }
        }
        public override bool Equals(object obj) {
            if (this.MetaEntity == null) {
                return base.Equals(obj);
            } else {
                if (ReferenceEquals(this, obj)) { return true; }
                var other = obj as TNDEntityItem;
                if (ReferenceEquals(other, null)) { return false; }
                if (!ReferenceEquals(this.MetaEntity, other.MetaEntity)) { return false; }
                var propertyName = this.MetaEntity.GetIdOrNamePropertyName();
                object thisKey = this.GetProperty(propertyName);
                object otherKey = other.GetProperty(propertyName);
                if (ReferenceEquals(thisKey, otherKey)) { return true; }
                if (ReferenceEquals(thisKey, null)) { return false; }
                return thisKey.Equals(otherKey);
            }
        }

        public TNDProperty GetKeyProperty() {
            var propertyName = this.MetaEntity.GetIdOrNamePropertyName();
            if (string.IsNullOrEmpty(propertyName)) { return null; }
            return this.GetProperty(propertyName);
        }
        public object GetKeyValue() {
            var propertyName = this.MetaEntity.GetIdOrNamePropertyName();
            if (string.IsNullOrEmpty(propertyName)) { return null; }
            return this.GetProperty(propertyName)?.Value;
        }

        public TNDProperty GetParentKeyProperty() {
            var propertyName = this.MetaEntity.GetParentIdOrNamePropertyName();
            if (string.IsNullOrEmpty(propertyName)) { return null; }
            return this.GetProperty(propertyName);
        }
        public object GetParentKeyValue() {
            var propertyName = this.MetaEntity.GetParentIdOrNamePropertyName();
            if (string.IsNullOrEmpty(propertyName)) { return null; }
            return this.GetProperty(propertyName)?.Value;
        }
    }
}
