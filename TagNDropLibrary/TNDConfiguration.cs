using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;

#pragma warning disable SA1402 // File may only contain a single class

namespace TagNDropLibrary {
    [JsonObject(MemberSerialization.OptIn)]
    public class TNDConfiguration : TNDNotifyPropertyChanged {
        private TNDMetaEntity[] _MetaEntities;
        private TNDMetaStorage[] _MetaStorages;
        private TNDMetaSource[] _MetaSources;

        public TNDConfiguration() { }

        [JsonProperty]
        public TNDMetaEntity[] MetaEntities { get { return this._MetaEntities; } set { this._MetaEntities = value; this.OnPropertyChanged(nameof(this.MetaEntities)); } }

        [JsonProperty]
        public TNDMetaStorage[] MetaStorages { get { return this._MetaStorages; } set { this._MetaStorages = value; this.OnPropertyChanged(nameof(this.MetaStorages)); } }

        [JsonProperty]
        public TNDMetaSource[] MetaSources { get { return this._MetaSources; } set { this._MetaSources = value; this.OnPropertyChanged(nameof(this.MetaSources)); } }

        public TNDMetaStorage FindMetaStorage(string metaStorageName) {
            if (this.MetaStorages == null) { return null; }
            if (string.IsNullOrEmpty(metaStorageName)) { metaStorageName = ConstsLibrary.Default; }
            return this.MetaStorages.FirstOrDefault(_ => string.Equals(_.MetaStorageName, metaStorageName, StringComparison.OrdinalIgnoreCase));
        }

        public TNDMetaEntity FindMetaEntity(string metaEntityName) {
            if (this.MetaEntities == null || string.IsNullOrEmpty(metaEntityName)) { return null; }
            return this.MetaEntities.FirstOrDefault(_ => string.Equals(_.MetaEntityName, metaEntityName, StringComparison.OrdinalIgnoreCase));
        }

        public TNDMetaSource FindMetaSource(string metaSourceName) {
            if (this.MetaSources == null) { return null; }
            if (string.IsNullOrEmpty(metaSourceName)) { metaSourceName = ConstsLibrary.Default; }
            return this.MetaSources.FirstOrDefault(_ => string.Equals(_.MetaSourceName, metaSourceName, StringComparison.OrdinalIgnoreCase));
        }

        public void Normalize() {
            if (this.MetaEntities == null) { this.MetaEntities = new TNDMetaEntity[0]; }
            if (this.MetaSources == null) { this.MetaSources = new TNDMetaSource[0]; }
            if (this.MetaStorages == null) { this.MetaStorages = new TNDMetaStorage[0]; }
            //
            var defaultMetaSource = this.MetaSources.FirstOrDefault(_ => _.MetaSourceName == ConstsLibrary.Default);
            var defaultMetaStorage = this.MetaStorages.FirstOrDefault(_ => _.MetaStorageName == ConstsLibrary.Default);
            if (defaultMetaSource == null) {
                defaultMetaSource = new TNDMetaSource();
                defaultMetaSource.MetaSourceName = ConstsLibrary.Default;
                this.MetaSources = this.MetaSources.AddTo(defaultMetaSource);
            }
            if (defaultMetaStorage == null) {
                defaultMetaStorage = new TNDMetaStorage();
                defaultMetaStorage.MetaStorageName = ConstsLibrary.Default;
                this.MetaStorages = this.MetaStorages.AddTo(defaultMetaStorage);
            }
            //
            for (int idx = 0; idx < this.MetaEntities.Length; idx++) {
                var metaEntity = this.MetaEntities[idx];
                metaEntity.MetaEntityOrder = idx;
                if (string.IsNullOrEmpty(metaEntity.MetaEntityName)) {
                    throw new InvalidOperationException("Name is empty.");
                }
                var metaSource = this.FindMetaSource(metaEntity.MetaSourceName);
                var metaStorage = this.FindMetaStorage(metaEntity.MetaStorageName);
                metaEntity.MetaSource = metaSource;
                metaEntity.MetaStorage = metaStorage;
                metaEntity.MetaEntityParent = this.FindMetaEntity(metaEntity.MetaEntityParentName);
                if (metaEntity.MetaEntityParent == null) {
                    metaEntity.MetaEntityLevel = 0;
                } else {
                    metaEntity.MetaEntityLevel = metaEntity.MetaEntityParent.MetaEntityLevel + 1;
                    metaEntity.MetaEntityParent.HasChildren = true;
                }
            }
            for (int iWatchDog = this.MetaEntities.Length; iWatchDog >= 0; iWatchDog--) {
                bool modified = false;
                foreach (var metaEntity in this.MetaEntities) {
                    if (metaEntity.MetaEntityParent != null) {
                        var level = metaEntity.MetaEntityParent.MetaEntityLevel + 1;
                        if (metaEntity.MetaEntityLevel < level) {
                            metaEntity.MetaEntityLevel = level;
                            modified = true;
                        }
                    }
                }
                if (!modified) { break; }
            }
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class TNDMetaEntity : TNDNotifyPropertyChanged {
        public TNDMetaSource MetaSource;
        public TNDMetaStorage MetaStorage;
        public TNDMetaEntity MetaEntityParent;
        public int MetaEntityLevel;
        public int MetaEntityOrder;
        //
        private string _MetaEntityName;
        private string _ParentMetaEntityName;
        private string _MetaEntityParentNamePropertyName;
        private string _MetaSourceName;
        private string _MetaStorageName;
        private string _MetaEntityNamePropertyName;
        private string _MetaEntityIdPropertyName;
        private string _MetaEntityParentIdPropertyName;

        public TNDMetaEntity() {
            this._MetaSourceName = ConstsLibrary.Default;
            this._MetaStorageName = ConstsLibrary.Default;
        }

        [JsonProperty("Name")]
        public string MetaEntityName { get { return this._MetaEntityName; } set { this._MetaEntityName = value; this.OnPropertyChanged(nameof(this.MetaEntityName)); } }

        [JsonProperty("IdProperty")]
        public string MetaEntityIdPropertyName { get { return this._MetaEntityIdPropertyName; } set { this._MetaEntityIdPropertyName = value; this.OnPropertyChanged(nameof(this.MetaEntityIdPropertyName)); } }

        [JsonProperty("NameProperty")]
        public string MetaEntityNamePropertyName { get { return this._MetaEntityNamePropertyName; } set { this._MetaEntityNamePropertyName = value; this.OnPropertyChanged(nameof(this.MetaEntityNamePropertyName)); } }

        [JsonProperty("ParentName")]
        public string MetaEntityParentName { get { return this._ParentMetaEntityName; } set { this._ParentMetaEntityName = value; this.OnPropertyChanged(nameof(this.MetaEntityParentName)); } }

        [JsonProperty("ParentNameProperty")]
        public string MetaEntityParentNamePropertyName { get { return this._MetaEntityParentNamePropertyName; } set { this._MetaEntityParentNamePropertyName = value; this.OnPropertyChanged(nameof(this.MetaEntityParentNamePropertyName)); } }

        [JsonProperty("ParentIdProperty")]
        public string MetaEntityParentIdPropertyName { get { return this._MetaEntityParentIdPropertyName; } set { this._MetaEntityParentIdPropertyName = value; this.OnPropertyChanged(nameof(this.MetaEntityParentIdPropertyName)); } }

        [JsonProperty("MetaStorageName")]
        [DefaultValue(ConstsLibrary.Default)]
        public string MetaStorageName { get { return this._MetaStorageName; } set { this._MetaStorageName = value; this.OnPropertyChanged(nameof(this.MetaStorageName)); } }

        [JsonProperty("MetaSourceName", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(ConstsLibrary.Default)]
        public string MetaSourceName { get { return this._MetaSourceName; } set { this._MetaSourceName = value; this.OnPropertyChanged(nameof(this.MetaSourceName)); } }

        [JsonIgnore]
        public bool HasParent { get { return this.MetaEntityLevel > 0; } }

        [JsonIgnore]
        public bool HasChildren { get; set; }

        public string GetIdOrNamePropertyName() {
            var propertyName = this.MetaEntityIdPropertyName;
            if (string.IsNullOrEmpty(propertyName)) { propertyName = this.MetaEntityNamePropertyName; }
            if (string.IsNullOrEmpty(propertyName)) { propertyName = this.MetaEntityName; }
            return propertyName;
        }

        public string GetParentIdOrNamePropertyName() {
            var propertyName = this.MetaEntityParentIdPropertyName;
            if (string.IsNullOrEmpty(propertyName)) { propertyName = this.MetaEntityParentNamePropertyName; }
            if (string.IsNullOrEmpty(propertyName)) { propertyName = this.MetaEntityParentName; }
            return propertyName;
        }

        public override string ToString() => this.MetaEntityName ?? string.Empty;
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class TNDMetaStorage : TNDNotifyPropertyChanged {
        public ITNDStorageBuis StorageBuis;
        //
        private string _MetaStorageName;
        private string _AssemblyQualifiedName;
        private string _RootPath;
        private string _FileName;

        public TNDMetaStorage() { }

        [JsonProperty]
        public string MetaStorageName { get { return this._MetaStorageName; } set { this._MetaStorageName = value; this.OnPropertyChanged(nameof(this.MetaStorageName)); } }

        [JsonProperty]
        public string AssemblyQualifiedName { get { return this._AssemblyQualifiedName; } set { this._AssemblyQualifiedName = value; this.OnPropertyChanged(nameof(this.AssemblyQualifiedName)); } }

        [JsonProperty]
        public string RootPath { get { return this._RootPath; } set { this._RootPath = value; this.OnPropertyChanged(nameof(this.RootPath)); } }

#warning here
        [JsonProperty]
        public string FileName { get { return this._FileName; } set { this._FileName = value; this.OnPropertyChanged(nameof(this.FileName)); } }

        public override string ToString() => this.MetaStorageName ?? string.Empty;
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class TNDMetaSource : TNDNotifyPropertyChanged {
        public ITNDSourceBuis SourceBuis;
        //
        private string _MetaSourceName;
        private string _AssemblyQualifiedName;
        private string _Configuration;

        public TNDMetaSource() { }

        [JsonProperty]
        public string MetaSourceName { get { return this._MetaSourceName; } set { this._MetaSourceName = value; this.OnPropertyChanged(nameof(this.MetaSourceName)); } }

        [JsonProperty]
        public string AssemblyQualifiedName { get { return this._AssemblyQualifiedName; } set { this._AssemblyQualifiedName = value; this.OnPropertyChanged(nameof(this.AssemblyQualifiedName)); } }

        [JsonProperty]
        public string Configuration { get { return this._Configuration; } set { this._Configuration = value; this.OnPropertyChanged(nameof(this.Configuration)); } }

        public override string ToString() => this.MetaSourceName ?? string.Empty;
    }
}
