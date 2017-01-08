using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TagNDropLibrary;

namespace TagNDrop {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DropWindow : Window {
        private ITNDDropBuis _DropBuis;
        private TNDDropModel _DropModel;

        public DropWindow() {
            this.InitializeComponent();
        }

        public void Init() {
            if (this._DropBuis == null) {
                this._DropModel = this.DataContext as TagNDropLibrary.TNDDropModel;
                this._DropBuis = PoorMansDI.Instance.Create<ITNDDropBuis>().Init(this._DropModel);
                App.ApplicationBuis.RegisterDrop(this._DropBuis);
                this._DropBuis?.UpdateFormatDescription();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            try {
                this.Init();
            } catch (Exception exception) {
                App.ApplicationBuis.OnFatalException("DropWindow.Window_Loaded", exception);
            }
        }

        private void Window_Activated(object sender, EventArgs e) {
            try {
                this._DropBuis?.Activating();
                this._DropBuis?.UpdateFormatDescription();
            } catch (Exception exception) {
                App.ApplicationBuis.OnFatalException("DropWindow.Window_Activated", exception);
            }
        }

        private void Window_Deactivated(object sender, EventArgs e) {
            try {
                this._DropBuis?.Deactivating();
            } catch (Exception exception) {
                App.ApplicationBuis.OnFatalException("DropWindow.Window_Activated", exception);
            }
        }

        private void CommandBinding_Paste_Executed(object sender, ExecutedRoutedEventArgs e) {
            var data = System.Windows.Clipboard.GetDataObject();
            this._DropBuis?.DoPaste(null, data);
        }

        private void Window_Closed(object sender, EventArgs e) {
            System.Diagnostics.Debug.WriteLine("Window_Closed");
            App.ApplicationBuis.Exit();
        }
    }
}
