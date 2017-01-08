using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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

namespace TagNDropUI {
    /// <summary>
    /// Interaction logic for DropControl.xaml
    /// </summary>
    public partial class DropControl : UserControl {
        public DropControl() {
            this.InitializeComponent();
        }

        // [Bindable(true), Category("Content")]
        // [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        // public TNDDropModel Model
        // {
        //    get { return (TNDDropModel)GetValue(ModelProperty); }
        //    set { SetValue(ModelProperty, value); }
        // }

        // public static readonly DependencyProperty ModelProperty =
        //    DependencyProperty.Register("Model", typeof(TNDDropModel), typeof(DropControl), new PropertyMetadata(null));

        private void setDnDTarget(object sender) {
            var model = this.DataContext as TNDDropModel;
            if (model != null) {
                var dc = ((FrameworkElement)sender).DataContext;
                TNDFavorite favorite = null;
                if (dc is TNDFavorite) { favorite = (TNDFavorite)dc; } else if (dc is TNDDropModel) { favorite = model.EmptyFavorite; }
                model.DnDTarget = favorite;
            }
        }
        private void clearDnDTarget(object sender) {
            var model = this.DataContext as TNDDropModel;
            if (model != null) {
                var dc = ((FrameworkElement)sender).DataContext;
                TNDFavorite favorite = null;
                if (dc is TNDFavorite) { favorite = (TNDFavorite)dc; } else if (dc is TNDDropModel) { favorite = model.EmptyFavorite; }
                if (ReferenceEquals(model.DnDTarget, favorite)) {
                    model.DnDTarget = null;
                }
            }
        }
        private void favorite_DragEnter(object sender, DragEventArgs e) {
            this.setDnDTarget(sender);
            var model = this.DataContext as TNDDropModel;
            if (model != null) {
                var data = e.Data;
                model.DoDragEnter(model.DnDTarget, e.Data);
            }
            e.Handled = true;
        }

        private void favorite_DragLeave(object sender, DragEventArgs e) {
            this.clearDnDTarget(sender);
            e.Handled = true;
        }

        private void favorite_DragOver(object sender, DragEventArgs e) {
            this.setDnDTarget(sender);
            e.Handled = true;
        }

        private void favorite_Drop(object sender, DragEventArgs e) {
            this.clearDnDTarget(sender);
            var favorite = ((FrameworkElement)sender).DataContext as TNDFavorite;
            var model = this.DataContext as TNDDropModel;
            if (model != null) {
                var data = e.Data;
                model.DoDrop(favorite, e.Data);
            }
            e.Handled = true;
        }

        private void favorite_GiveFeedback(object sender, GiveFeedbackEventArgs e) {
            var favorite = ((FrameworkElement)sender).DataContext as TNDFavorite;
            var model = this.DataContext as TNDDropModel;
            if (model != null) {
                model.DoGiveFeedback(favorite, e);
            }
            e.Handled = true;
        }

        private void UserControl_DragEnter(object sender, DragEventArgs e) {
            this.setDnDTarget(sender);
            var model = this.DataContext as TNDDropModel;
            if (model != null) {
                var data = e.Data;
                model.DoDragEnter(model.DnDTarget, e.Data);
            }
            e.Handled = true;
        }

        private void UserControl_DragLeave(object sender, DragEventArgs e) {
            this.clearDnDTarget(sender);
            e.Handled = true;
        }

        private void UserControl_DragOver(object sender, DragEventArgs e) {
            this.setDnDTarget(sender);
            e.Handled = true;
        }

        private void UserControl_Drop(object sender, DragEventArgs e) {
            this.clearDnDTarget(sender);
            var model = this.DataContext as TNDDropModel;
            if (model != null) {
                var data = e.Data;
                model.DoDrop(null, e.Data);
            }
            e.Handled = true;
        }

        private void UserControl_GiveFeedback(object sender, GiveFeedbackEventArgs e) {
            var favorite = ((FrameworkElement)sender).DataContext as TNDFavorite;
            var model = this.DataContext as TNDDropModel;
            if (model != null) {
                model.DoGiveFeedback(favorite, e);
            }
            e.Handled = true;
        }

        private void CommandBinding_Paste_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
#if false
            var favorite = ((FrameworkElement)sender).DataContext as TNDFavorite;
            var model = this.DataContext as TNDDropModel;
            if (model != null) {
                model.DoGiveFeedback(favorite, e);
            }
#endif
            e.CanExecute = true;
            e.Handled = true;
        }

        private void CommandBinding_Paste_Executed(object sender, ExecutedRoutedEventArgs e) {
            var favorite = ((FrameworkElement)sender).DataContext as TNDFavorite;
            var model = this.DataContext as TNDDropModel;
            var iDataObject = Clipboard.GetDataObject();
            if (model != null) {
                model.DoPaste(favorite, iDataObject);
            }
            e.Handled = true;
        }

    }
}
