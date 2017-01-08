using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace TagNDropLibrary {

    [JsonObject]
    public class TNDProperty : TNDNotifyPropertyChanged {
        private string _Name;
        private Type _Type;
        private object _Value;

        public TNDProperty() {
            this._Type = typeof(object);
        }

        public TNDProperty(string name, Type type, object value) {
            if (name == null) { throw new ArgumentNullException(nameof(name)); }
            if (type == null) { throw new ArgumentNullException(nameof(type)); }
            //
            this._Name = name;
            this._Type = type;
            if (value == null || type.IsAssignableFrom(value.GetType())) {
                this._Value = value;
            }
        }

        [JsonProperty(Order = 1)]
        public string Name {
            get {
                return this._Name;
            }
            set {
                this._Name = value; this.OnPropertyChanged(nameof(this.Name));
            }
        }

        [JsonIgnore]
        public Type Type {
            get {
                return this._Type;
            }
            set {
                if (this._Type == value) { return; }
                if (value == null) { throw new ArgumentNullException(nameof(this.Type)); }
                if (this._Type != typeof(object)) { throw new InvalidOperationException("Type is already set."); }
                this._Type = value;
            }
        }
        [JsonProperty(Order = 2,NullValueHandling = NullValueHandling.Ignore)]
        public string TypeName {
            get {
                if (this._Type == typeof(string) && this._Value != null) { return null; }
                if (this._Type == typeof(int) && this._Value != null) { return null; }
                return this._Type.FullName + ", " + this._Type.Assembly.GetName().Name;
            }
            set {
                var type = System.Type.GetType(value);
                if (type == this._Type) { return; }
                if (this._Type != typeof(object)) {
                    if (type.IsAssignableFrom(this._Type)) {
                        // ok
                    } else {
                        throw new InvalidOperationException("Type is already set.");
                    }
                }
                this._Type = type;
            }
        }

        [JsonIgnore]
        public object Value {
            get {
                return this._Value;
            }
            set {
                if (value == null) {
                } else if (this._Type.IsAssignableFrom(value.GetType())) {
                    this._Value = value;
                }
                this.OnPropertyChanged(nameof(this.Value));
            }
        }

        [JsonProperty(Order = 3, DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public string ValueString {
            get {
                try {
                    return (this._Type == typeof(string)) ? (string)this._Value : null;
                } catch {
                    return null;
                }
            }
            set {
                if (value != null) {
                    this._Type = typeof(string);
                    this._Value = value;
                    this.OnPropertyChanged(nameof(this.ValueString));
                }
            }
        }
        [JsonProperty(Order = 4, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int ValueInt {
            get {
                try {
                    return (this._Type == typeof(int)) ? (int)this._Value : 0;
                } catch {
                    return 0;
                }
            }
            set {
                this._Type = typeof(int);
                this._Value = value;
                this.OnPropertyChanged(nameof(this.ValueString));
            }
        }

        [JsonProperty(Order = 5, TypeNameHandling = TypeNameHandling.All, DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public object ValueNonString {
            get {
                return ((this._Type == typeof(string)) || (this._Type == typeof(int))) ? null : this._Value;
            }
            set {
                if (this._Type == null) {
                    if (value != null) {
                        this._Type = value.GetType();
                    }
                } else {
                }
                this._Value = value;
                this.OnPropertyChanged(nameof(this.ValueString));
            }
        }

        [JsonIgnore]
        public string ValueDisplay {
            get {
                if (this._Value == null) { return null; }
                if (this._Type == typeof(string)) { return (string)this._Value; }
                if (this._Type == typeof(DateTime)) { return ((DateTime)this._Value).ToString(); }
                return this._Value.ToString();
            }
        }

        public static TNDProperty Create<T>(string name, T value) { return new TNDProperty(name, typeof(T), value); }

        public override string ToString() {
#pragma warning disable SA1012 // Opening braces must be spaced correctly
            return $"{this._Name} = {this._Type} - {this.Value}";
#pragma warning restore SA1012 // Opening braces must be spaced correctly
        }

        public override int GetHashCode() {
            return this._Name.GetHashCode()
                ^ this._Type.GetHashCode()
                ^ ((this._Value == null) ? 0 : this._Value.GetHashCode());
        }

        public int? GetValueAsInt() {
            if (this._Type == typeof(int)) { return (int)this._Value; }
            if (this._Type == typeof(int?)) { return (int?)this._Value; }
            return null;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(this, obj)) { return true; }
            var other = obj as TNDProperty;
            if (ReferenceEquals(other, null)) { return false; }
            if (!string.Equals(this.Name, other.Name, StringComparison.Ordinal)) { return false; }
            if (this.Type != other.Type) { return false; }
            if (ReferenceEquals(this.Value, other.Value)) { return true; }
            if (ReferenceEquals(this.Value, null)) { return false; }
            return this.Value.Equals(other.Value);
        }
    }
}
