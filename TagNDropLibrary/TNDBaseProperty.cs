using System;
using System.Linq;
using Newtonsoft.Json;

namespace TagNDropLibrary {
    [JsonObject(MemberSerialization.OptIn)]
    public class TNDBaseProperty : TNDNotifyPropertyChanged {
        private static int _InstanceId;

        public TNDBaseProperty() {
            this.InstanceId = ++_InstanceId;
            this.Property = new BulkObservableCollection<TNDProperty>();
        }

        [JsonIgnore]
        public int InstanceId { get; }

        [JsonProperty]
        public BulkObservableCollection<TNDProperty> Property { get; }

        public void SetProperty<T>(string name, T value) {
            TNDProperty property = this.GetProperty(name);
            if (property == null) {
                this.Property.Add(TNDProperty.Create<T>(name, value));
            } else {
                property.Value = value;
            }
        }

        public void SetProperty(TNDProperty propertyToSet) {
            for (int idx = 0; idx < this.Property.Count; idx++) {
                TNDProperty property = this.Property[idx];
                if (string.Equals(property.Name, propertyToSet.Name, StringComparison.Ordinal)) {
                    this.Property[idx] = propertyToSet;
                    return;
                }
            }
            this.Property.Add(propertyToSet);
        }

        public void SetProperties(System.Collections.Generic.IEnumerable<TNDProperty> propertiesToSet) {
            foreach (var propertyToSet in propertiesToSet) {
                this.SetProperty(propertyToSet);
            }
        }

        public TNDProperty GetProperty(string name) {
            return this.Property.FirstOrDefault(_ => string.Equals(_.Name, name, StringComparison.Ordinal));
        }

        public override string ToString() {
            return base.ToString() + " - " + this.InstanceId.ToString();
        }

        public override int GetHashCode() {
            unchecked {
                int result = 0;
                for (int idx = this.Property.Count - 1; idx >= 0; idx--) {
                    var property = this.Property[idx];
                    if (property != null) {
                        result = result ^ property.GetHashCode();
                    }
                }
                return result;
            }
        }
        public override bool Equals(object obj) {
            if (ReferenceEquals(this, obj)) { return true; }
            var other = obj as TNDBaseProperty;
            if (ReferenceEquals(other, null)) { return false; }
            if (this.Property.Count != other.Property.Count) { return false; }
            for (int idx = this.Property.Count - 1; idx >= 0; idx--) {
                var thisProperty = this.Property[idx];
                var otherProperty = other.Property[idx];
                if (ReferenceEquals(thisProperty, otherProperty)) { continue; }
                if (ReferenceEquals(thisProperty, null)) { return false; }
                if (!thisProperty.Equals(other)) { return false; }
            }
            return true;
        }
    }
}
