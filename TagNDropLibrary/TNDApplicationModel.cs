using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagNDropLibrary {
    public class TNDApplicationModel : TNDNotifyPropertyChanged {
        private ITNDApplicationBuis _ApplicationBuis;
        //
        private string _ConfigurationSource;
        private TNDConfiguration _Configuration;
        private Exception _FatalException;
        private string _FavoritesPath;
        public bool Exiting;

        public TNDApplicationModel() { }

        public string ConfigurationSource { get { return this._ConfigurationSource; } set { this._ConfigurationSource = value; this.OnPropertyChanged(nameof(this.ConfigurationSource)); } }

        public string FavoritesPath { get { return this._FavoritesPath; } set { this._FavoritesPath = value; this.OnPropertyChanged(nameof(this.FavoritesPath)); } }

        public TNDConfiguration Configuration { get { return this._Configuration; } set { this._Configuration = value; this.OnPropertyChanged(nameof(this.Configuration)); } }

        public Exception FatalException { get { return this._FatalException; } set { this._FatalException = value; this.OnPropertyChanged(nameof(this.FatalException)); } }

        public void Init(ITNDApplicationBuis applicationBuis) {
            if (this._ApplicationBuis == null) {
                this._ApplicationBuis = applicationBuis;
            }
        }

        public void LoadConfiguration(bool forceReload) {
            this._ApplicationBuis?.LoadConfiguration(forceReload);
        }

        public void ShowDropWindow() {
            this._ApplicationBuis?.ShowDropWindow();
        }
    }
}
