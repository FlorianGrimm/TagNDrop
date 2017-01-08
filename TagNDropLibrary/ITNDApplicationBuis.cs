using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagNDropLibrary {
    public interface ITNDApplicationBuis {
        TNDApplicationModel ApplicationModel { get; }

        Exception FatalException { get; }

        ITNDApplicationBuis Init(TNDApplicationModel applicationModel, ITNDNotifyIconBuis notifyIconBuis);

        TNDConfiguration LoadConfiguration(bool forceReload);

        void RegisterDrop(ITNDDropBuis dropBuis);

        ITNDDataAccess GetDataAccess();

        void ShowDropWindow();

        void HideDropWindow();

        void ShowSearchWindow(TNDTargetModel[] lst);

        void OnFatalException(string title, Exception exc);

        void Exit();

        TNDFavorite[] LoadFavorites();

        void AddToFavorites(TNDFavorite[] items);

        Task SaveFavoritesAsync();

    }
}
