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
using System.Windows.Shapes;
using TagNDropLibrary;

namespace TagNDrop {
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window {
        public SearchWindow() {
            this.InitializeComponent();
        }

        public void Init() {
            var model = (TNDSearchModel)this.DataContext;
            if (model.ApplicationBuis == null) {
                model.Init(App.ApplicationBuis);
                var cfg = App.ApplicationBuis.LoadConfiguration(false);
                model.ApplyConfiguration(cfg);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            try {
                this.Init();
            } catch (Exception exception) {
                App.ApplicationBuis.OnFatalException("SearchWindow.Window_Loaded", exception);
            }
        }

        private void Window_Closed(object sender, EventArgs e) {
            try {
                App.ApplicationBuis.ShowDropWindow();
            } catch (Exception exception) {
                App.ApplicationBuis.OnFatalException("SearchWindow.Window_Closed", exception);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            try {
                var model = (TNDSearchModel)this.DataContext;
                if (model.IsSaving()) {
                    e.Cancel = true;
                }
            } catch (Exception exception) {
                App.ApplicationBuis.OnFatalException("SearchWindow.Window_Closed", exception);
            }
        }
    }
}
