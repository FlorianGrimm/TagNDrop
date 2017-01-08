using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using TagNDropBuis;
using TagNDropLibrary;

namespace TagNDrop {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        private TNDApplicationModel _ApplicationModel;
        private ITNDApplicationBuis _ApplicationBuis;
        private ITNDKeyboardHook _KeyboardHook;

        public App() {
            this.InitDI();
        }

        //public static App CurrentApplication => (System.Windows.Application.Current as App);

        //public static TNDApplicationModel ApplicationModel => (System.Windows.Application.Current as App)?._ApplicationModel;

        public static ITNDApplicationBuis ApplicationBuis => (System.Windows.Application.Current as App)?._ApplicationBuis;

        private void InitDI() {
            PoorMansDI.Instance.Add<ITNDApplicationBuis, TNDApplicationBuis2>();
            PoorMansDI.Instance.Add<ITNDDataAccess, TNDDataAccess>();
            PoorMansDI.Instance.Add<ITNDDropBuis, TNDDropBuis>();
            PoorMansDI.Instance.Add<ITNDNotifyIconBuis, TNDNotifyIconBuis>();
            PoorMansDI.Instance.Add<ITNDKeyboardHook, TNDKeyboardHook>();
            PoorMansDI.Instance.Add<ITNDSearchBuis, TNDSearchBuis2>();
            PoorMansDI.Instance.Add<ITNDSourceBuis, TNDDefaultSourceBuis>();
            PoorMansDI.Instance.Add<ITNDStorageBuis, TNDDefaultStorageBuis>();
            PoorMansDI.Instance.Add<ITNDTargetBuis, TNDTargetBuis>();
        }

        private void Application_Startup(object sender, StartupEventArgs e) {
            try {
                this.initApplication();
                this._ApplicationModel?.LoadConfiguration(false);
                if (!System.Diagnostics.Debugger.IsAttached) {
                    this._KeyboardHook?.Subscribe();
                }
                App.ApplicationBuis.ShowDropWindow();
            } catch (Exception exception) {
                App.ApplicationBuis.OnFatalException("App.Application_Startup", exception);
            }
        }

        private void Application_Activated(object sender, EventArgs e) {
        }

        private void Application_Deactivated(object sender, EventArgs e) {
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e) {
            //e.Handled = true;
            System.Diagnostics.Debug.WriteLine(e.Exception.ToString());
            //System.Windows.MessageBox.Show(e.Exception.ToString());
        }

        private void Application_SessionEnding(object sender, SessionEndingCancelEventArgs e) {
            //if (e.ReasonSessionEnding == ReasonSessionEnding.
        }

        private void Application_Exit(object sender, ExitEventArgs e) {
            using (var ni = this._ApplicationBuis as IDisposable) {
                using (var kh = this._KeyboardHook as IDisposable) {
                    this._KeyboardHook = null;
                    this._ApplicationBuis = null;
                    this._ApplicationModel = null;
                }
            }
        }

        private void initApplication() {
            if (this._ApplicationModel == null) {
                var applicationModel = this.TryFindResource("ApplicationModel") as TNDApplicationModel;
                var notifyIconBuis = PoorMansDI.Instance.Create<ITNDNotifyIconBuis>().Init(applicationModel);
                var applicationBuis = PoorMansDI.Instance.Create<ITNDApplicationBuis>().Init(applicationModel, notifyIconBuis);
                var keyboardHook = PoorMansDI.Instance.Create<ITNDKeyboardHook>().Init(applicationModel);
                applicationModel.Init(applicationBuis);
                this._ApplicationModel = applicationModel;
                this._ApplicationBuis = applicationBuis;
                this._KeyboardHook = keyboardHook;
                this.Resources.Add("ApplicationBuis", applicationBuis);
            }
        }
    }
}
