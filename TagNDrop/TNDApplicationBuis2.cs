using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagNDropBuis;
using TagNDropLibrary;

namespace TagNDrop {
    public class TNDApplicationBuis2 : TNDApplicationBuis1 {
        public TNDApplicationBuis2()
            : base() {
        }

        public override ITNDApplicationBuis Init(TNDApplicationModel applicationModel, ITNDNotifyIconBuis notifyIconBuis) {
            // read from settings
            try {
                if (applicationModel.ConfigurationSource == null) {
                    var configurationSource = TagNDrop.Properties.Settings.Default?.ConfigurationSource;
                    applicationModel.ConfigurationSource = configurationSource;
                }
                return base.Init(applicationModel, notifyIconBuis);
            } catch (Exception exception) {
                App.ApplicationBuis.OnFatalException("TNDApplicationBuis2.Init", exception);
                return this;
            }
        }

        public override void ShowDropWindow() {
            if (App.Current.Dispatcher.CheckAccess()) {
                try {
                    base.ShowDropWindow();
                    var app = (App)System.Windows.Application.Current;
                    var dropWindow = getDropWindow(app);
                    var searchWindow = getSearchWindow(app);
                    if (dropWindow == null) {
                        dropWindow = new DropWindow();
                        dropWindow.Init();
                    }
                    if (dropWindow != null) {
                        if (searchWindow != null) {
                            searchWindow.Hide();
                        }
                        dropWindow.Topmost = true;
                        dropWindow.Show();
                        dropWindow.Activate();
                        dropWindow.Topmost = false;
                        //var wih = new System.Windows.Interop.WindowInteropHelper(dropWindow);
                        //wih.Handle
                    }
                } catch (Exception exception) {
                    App.ApplicationBuis.OnFatalException("TNDApplicationBuis2.ShowTag", exception);
                }
            } else {
                App.Current.Dispatcher.Invoke(this.ShowDropWindow);
            }
        }

        public override void HideDropWindow() {
            var app = (App)System.Windows.Application.Current;
            if (app.Dispatcher.CheckAccess()) {
                try {
                    base.HideDropWindow();
                    var dropWindow = getDropWindow(app);
                    if (dropWindow != null) {
                        dropWindow.Hide();
                    }
                } catch (Exception exception) {
                    App.ApplicationBuis.OnFatalException("TNDApplicationBuis2.ShowTag", exception);
                }
            } else {
                app.Dispatcher.Invoke(() => { this.HideDropWindow(); });
            }
        }

        public override void ShowSearchWindow(TNDTargetModel[] lst) {
            try {
                base.ShowSearchWindow(lst);
#if false
            if (lst == null) { return; }
            if (!lst.Any()) { return; }
#else
                if (lst == null) { lst = new TNDTargetModel[0]; }
#endif
                //
                var app = (App)System.Windows.Application.Current;
                var dropWindow = getDropWindow(app);
                var searchWindow = getSearchWindow(app);
                //
                if (searchWindow == null) {
                    searchWindow = new SearchWindow();
                    searchWindow.Init();
                }
                if (searchWindow != null) {
                    TNDSearchModel model = (TNDSearchModel)searchWindow.DataContext;
                    if (model != null) {
                        model.AddTargets(lst);
                    }
                    if (dropWindow != null) {
                        dropWindow.Hide();
                    }
                    searchWindow.Show();
                }
                //
            } catch (Exception exception) {
                App.ApplicationBuis.OnFatalException("TNDApplicationBuis2.ShowSearch", exception);
            }
        }

        public override void Exit() {
            base.Exit();
            var app = (App)System.Windows.Application.Current;
            if (this.ApplicationModel.Exiting) { return; }
            var searchWindow = getSearchWindow(app);
            if (searchWindow != null) {
                var model = searchWindow.DataContext as TNDSearchModel;
                if (model.IsSaving()) { return; }
            }
            this.ApplicationModel.Exiting = true;
            app.Shutdown(0);
        }
        private static DropWindow getDropWindow(System.Windows.Application app) {
            DropWindow dropWindow = null;
            foreach (var window in app.Windows) {
                if (window is DropWindow) {
                    dropWindow = (DropWindow)window;
                    break;
                }
            }
            return dropWindow;
        }
        private static SearchWindow getSearchWindow(System.Windows.Application app) {
            SearchWindow searchWindow = null;
            foreach (var window in app.Windows) {
                if (window is SearchWindow) {
                    searchWindow = (SearchWindow)window;
                    break;
                }
            }
            return searchWindow;
        }
    }
}
