using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagNDropLibrary;

namespace TagNDropBuis {
    public class TNDApplicationBuis1 : ITNDApplicationBuis, IDisposable {
        protected ITNDNotifyIconBuis _NotifyIconBuis;
        protected TNDApplicationModel _ApplicationModel;
        protected ITNDDropBuis _DropBuis;
        protected ITNDDataAccess _DataAccess;
        protected bool _FavoritesNeedSaving;
        protected Dictionary<string, TNDFavorite> _Favorites;
        protected TNDFavorite[] _LoadFavoritesCache;

        public TNDApplicationBuis1() { }

        public TNDApplicationModel ApplicationModel => this._ApplicationModel;

        public Exception FatalException {
            get {
                var model = this._ApplicationModel;
                return (model == null) ? null : model.FatalException;
            }
            set {
                var model = this._ApplicationModel;
                if (model == null) {
                    model.FatalException = value;
                }
            }
        }

        public virtual ITNDApplicationBuis Init(TNDApplicationModel applicationModel, ITNDNotifyIconBuis notifyIconBuis) {
            this._ApplicationModel = applicationModel;
            this._NotifyIconBuis = notifyIconBuis;
            this._NotifyIconBuis.ShowMessage("Starting...");
            return this;
        }

        public virtual void RegisterDrop(ITNDDropBuis dropBuis) {
            this._DropBuis = dropBuis;
            if (this._ApplicationModel.Configuration != null) {
                this._DropBuis.ApplyConfiguration(this);
            }
        }

        public virtual TNDConfiguration LoadConfiguration(bool forceReload) {
            if (!forceReload) {
                if (this._ApplicationModel.Configuration != null) { return this._ApplicationModel.Configuration; }
            }
            var configurationSource = this._ApplicationModel.ConfigurationSource;
            if (string.IsNullOrEmpty(configurationSource)) {
                var fn = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConstsLibrary.TagNDropConfigurationJson);
                if (System.IO.File.Exists(fn)) { configurationSource = fn; }
            }
            if (string.IsNullOrEmpty(configurationSource)) {
                var appdata = System.Environment.ExpandEnvironmentVariables("%APPDATA%");
                var fn = System.IO.Path.Combine(appdata, ConstsLibrary.Company, ConstsLibrary.TagNDropConfigurationJson);
                if (System.IO.File.Exists(fn)) { configurationSource = fn; }
            }
            // ThinkOf: ALLUSERSPROFILE?
            // ThinkOf: registry ?
            bool configurationSourceIsValid = !string.IsNullOrEmpty(configurationSource);
            if (configurationSourceIsValid) {
                configurationSourceIsValid = System.IO.File.Exists(configurationSource);
            }
            if (!configurationSourceIsValid) {
                this._NotifyIconBuis.ShowError($"Configuration not found ({ configurationSource ?? ConstsLibrary.TagNDropConfigurationJson }).");
                return null;
            }
            //
            try {
                var configurationContent = System.IO.File.ReadAllText(configurationSource);
                var cfg = Newtonsoft.Json.JsonConvert.DeserializeObject<TNDConfiguration>(configurationContent);
                this.NormalizeConfiguration(cfg);
                this._ApplicationModel.Configuration = cfg;
                if (this._DropBuis != null) {
                    this._DropBuis.ApplyConfiguration(this);
                }
                return cfg;
            } catch (Exception exc) {
                this.OnFatalException("Load Configuration", exc);
                return null;
            }
        }

        public void OnFatalException(string title, Exception exc) {
            this.FatalException = exc;
            var result = System.Windows.MessageBox.Show(
                exc.ToString(),
                $"{ ConstsLibrary.ProductName } { title }",
                System.Windows.MessageBoxButton.OKCancel
                );
            if (result == System.Windows.MessageBoxResult.OK) {
                throw exc;
            }
        }

        public virtual ITNDDataAccess GetDataAccess() {
            var model = this._ApplicationModel;
            var cfg = model.Configuration;
            if (cfg == null) { this.LoadConfiguration(false); }
            return this._DataAccess ?? (this._DataAccess = PoorMansDI.Instance.Create<ITNDDataAccess>().Init(this));
        }

        public virtual TNDFavorite[] LoadFavorites() {
            if (this._Favorites == null) {
                var favorites = new Dictionary<string, TNDFavorite>();
                this._Favorites = favorites;
                lock (this._Favorites) {
                    var loadedFavorites = this.GetDataAccess().LoadFavorites();
                    if (loadedFavorites != null) {
                        foreach (var fav in loadedFavorites) {
                            var key = fav.MetaEntityName + "@" + fav.Id;
                            favorites[key] = fav;
                        }
                    }
                }
            }
            if (this._LoadFavoritesCache == null) {
                lock (this._Favorites) {
                    this._LoadFavoritesCache = this._Favorites.Values.ToArray();
                }
            }
            return this._LoadFavoritesCache;
        }

        public void AddToFavorites(TNDFavorite[] items) {
            lock (this._Favorites) {
                foreach (var fav in items) {
                    var key = fav.MetaEntityName + "@" + fav.Id;
                    TNDFavorite existing;
                    if (this._Favorites.TryGetValue(key, out existing)) {
                        existing.Merge(fav);
                    } else {
                        this._Favorites[key] = fav;
                    }
                }
                this._LoadFavoritesCache = null;
                this._FavoritesNeedSaving = true;
            }
        }

        public async Task SaveFavoritesAsync() {
            if (this._FavoritesNeedSaving) {
                TNDFavorite[] favorites;
                lock (this._Favorites) {
                    var limit = System.DateTime.Now.AddMonths(-3);
                    favorites = this._Favorites.Values
                        .Where(_ => _.LastUsed > limit)
                        .ToArray();
                }
                var taskSave = this.GetDataAccess().SaveFavorites(favorites);
                try {
                    await taskSave;
                } catch (Exception exception) {
                    this.OnFatalException("SaveFavoritesAsync", exception);
                }
                this._FavoritesNeedSaving = false;
            }
        }

        public virtual void ShowDropWindow() {
            // TNDApplicationBuis2 does this
        }

        public virtual void HideDropWindow() {
            // TNDApplicationBuis2 does this
        }

        public virtual void ShowSearchWindow(TNDTargetModel[] lst) {
            // TNDApplicationBuis2 does this
        }

        public virtual void Exit() { }

        public void Dispose() { this.Dispose(true); GC.SuppressFinalize(this); }

        protected virtual void Dispose(bool disposing) {
            using (var ni = this._NotifyIconBuis as IDisposable) {
                if (disposing) {
                    this._NotifyIconBuis = null;
                }
            }
        }

        protected void NormalizeConfiguration(TNDConfiguration cfg) {
            cfg.Normalize();
            //
            foreach (var metaSource in cfg.MetaSources) {
                metaSource.SourceBuis = this.GetSourceBuis(metaSource);
            }
            foreach (var metaStorage in cfg.MetaStorages) {
                metaStorage.StorageBuis = this.GetStorageBuis(metaStorage);
            }
        }

        protected ITNDSourceBuis GetSourceBuis(TNDMetaSource metaSource) {
            var assemblyQualifiedName = metaSource.AssemblyQualifiedName;
            ITNDSourceBuis result;
            if (assemblyQualifiedName.IsEmptyOrDefaultString()) {
                result = PoorMansDI.Instance.Create<ITNDSourceBuis>().Init(this, metaSource);
            } else {
                Type typeSource = System.Type.GetType(assemblyQualifiedName, false);
                var sourceObject = typeSource.GetConstructor(Type.EmptyTypes).Invoke(null);
                var sourceBuis = (ITNDSourceBuis)sourceObject;
                result = sourceBuis.Init(this, metaSource);
            }
            return result;
        }

        protected ITNDStorageBuis GetStorageBuis(TNDMetaStorage metaStorage) {
            string assemblyQualifiedName = metaStorage.AssemblyQualifiedName;
            ITNDStorageBuis result;
            if (assemblyQualifiedName.IsEmptyOrDefaultString()) {
                result = PoorMansDI.Instance.Create<ITNDStorageBuis>().Init(this, metaStorage);
            } else {
                Type typeSource = System.Type.GetType(assemblyQualifiedName, false);
                var sourceObject = typeSource.GetConstructor(Type.EmptyTypes).Invoke(null);
                var storageBuis = (ITNDStorageBuis)sourceObject;
                result = storageBuis.Init(this, metaStorage);
            }
            return result;
        }

        // private ITNDSourceBuis GetSourceBuisByType(string assemblyQualifiedName)
        // {
        //    ITNDSourceBuis result;
        //    if (assemblyQualifiedName.IsEmptyOrDefaultString())
        //    {
        //        result = PoorMansDI.Instance.Create<ITNDSourceBuis>().Init(this);
        //    }
        //    else
        //    {
        //        Type typeSource = System.Type.GetType(assemblyQualifiedName, false);
        //        var sourceObject = typeSource.GetConstructor(Type.EmptyTypes).Invoke(null);
        //        var sourceBuis = (ITNDSourceBuis)sourceObject;
        //        result = sourceBuis;
        //    }
        //    return result;
        // }
        // private ITNDStorageBuis GetStorageBuisByType(string assemblyQualifiedName)
        // {
        //    ITNDStorageBuis result;
        //    if (assemblyQualifiedName.IsEmptyOrDefaultString())
        //    {
        //        result = PoorMansDI.Instance.Create<ITNDStorageBuis>().Init(this);
        //    }
        //    else
        //    {
        //        Type typeSource = System.Type.GetType(assemblyQualifiedName, false);
        //        var sourceObject = typeSource.GetConstructor(Type.EmptyTypes).Invoke(null);
        //        var storageBuis = (ITNDStorageBuis)sourceObject;
        //        result = storageBuis;
        //    }
        //    return result;
        // }

        ~TNDApplicationBuis1() { this.Dispose(false); }
    }
}
