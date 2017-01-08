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

namespace TagNDropUI {
    /// <summary>
    /// Interaction logic for TargetControl.xaml
    /// </summary>
    public partial class TargetControl : UserControl {
        public TargetControl() {
            this.InitializeComponent();
        }

        private void TextBox_Search_GotFocus(object sender, RoutedEventArgs e) {
            var model = (TNDSearchModel)this.DataContext;
            if (model != null) {
                var searchItem = (e.OriginalSource as FrameworkElement)?.DataContext as TNDSearchItem;
                if (searchItem != null) {
                    var selectedTarget = model.SelectedTarget;
                    if (selectedTarget != null) {
                        selectedTarget.CurrentSearchItem = searchItem;
                    }
                }
            }
        }

        private async void TextBox_Search_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Return || e.Key == Key.Enter) {
                var model = (TNDSearchModel)this.DataContext;
                if (model != null) {
                    var searchItem = (e.OriginalSource as FrameworkElement)?.DataContext as TNDSearchItem;
                    if (searchItem != null) {
                        var selectedTarget = model.SelectedTarget;
                        if (selectedTarget != null) {
                            if (searchItem != null) {
                                selectedTarget.CurrentSearchItem = searchItem;
                                var taskSearch = model.Search(selectedTarget, searchItem);
                                try {
                                    await taskSearch;
                                } catch (Exception exception) {
                                    model.ApplicationBuis.OnFatalException("TextBox_Search_KeyDown", exception);
                                }
                            }
                        }
                    }
                }
            }
        }

        private async void Button_Search_Click(object sender, RoutedEventArgs e) {
            var model = (TNDSearchModel)this.DataContext;
            if (model != null) {
                var searchItem = (e.OriginalSource as FrameworkElement)?.DataContext as TNDSearchItem;
                if (searchItem != null) {
                    var selectedTarget = model.SelectedTarget;
                    if (selectedTarget != null) {
                        if (searchItem != null) {
                            selectedTarget.CurrentSearchItem = searchItem;
                            var taskSearch = model.Search(selectedTarget, searchItem);
                            try {
                                await taskSearch;
                            } catch (Exception exception) {
                                model.ApplicationBuis.OnFatalException("Button_Search_Click", exception);
                            }
                        }
                    }
                }
            }
        }

        private void CheckBox_SearchResult_Checked(object sender, RoutedEventArgs e) {
            var model = (TNDSearchModel)this.DataContext;
            if (model != null) {
                var entityItem = (e.OriginalSource as FrameworkElement)?.DataContext as TNDEntityItem;
                if (entityItem != null) {
                    try {
                        model.AddCheckedItem(entityItem);
                    } catch (Exception exception) {
                        model.ApplicationBuis.OnFatalException("CheckBox_SearchResult_Checked", exception);
                    }
                }
            }
        }

        private void CheckBox_SearchResult_Unchecked(object sender, RoutedEventArgs e) {
            var model = (TNDSearchModel)this.DataContext;
            if (model != null) {
                var entityItem = (e.OriginalSource as FrameworkElement)?.DataContext as TNDEntityItem;
                if (entityItem != null) {
                    try {
                        model.RemoveCheckedItem(entityItem);
                    } catch (Exception exception) {
                        model.ApplicationBuis.OnFatalException("CheckBox_SearchResult_Unchecked", exception);
                    }
                }
            }
        }

        private void Button_DropsClear_Click(object sender, RoutedEventArgs e) {
            var model = (TNDSearchModel)this.DataContext;
            if (model != null) {
                try {
                    model.ClearDrops();
                } catch (Exception exception) {
                    model.ApplicationBuis.OnFatalException("CheckBox_SearchResult_Unchecked", exception);
                }
            }
        }

        private void Button_SearchClear_Click(object sender, RoutedEventArgs e) {
            var model = (TNDSearchModel)this.DataContext;
            if (model != null) {
                try {
                    model.ClearSearchItems();
                } catch (Exception exception) {
                    model.ApplicationBuis.OnFatalException("CheckBox_SearchResult_Unchecked", exception);
                }
            }
        }

        private void Button_CheckedClear_Click(object sender, RoutedEventArgs e) {
            var model = (TNDSearchModel)this.DataContext;
            if (model != null) {
                try {
                    model.ClearCheckedItems();
                } catch (Exception exception) {
                    model.ApplicationBuis.OnFatalException("CheckBox_SearchResult_Unchecked", exception);
                }
            }
        }

        private async void Button_Save_Click(object sender, RoutedEventArgs e) {
            var model = (TNDSearchModel)this.DataContext;
            if (model != null) {
                TNDStoreItem[] items = null;
                try {
                    items = await model.Save();
                } catch (Exception exception) {
                    model.ApplicationBuis.OnFatalException("Button_Search_Click", exception);
                }
            }
        }

        private async void Button_Expand_Parent_Click(object sender, RoutedEventArgs e) {
            var model = (TNDSearchModel)this.DataContext;
            if (model != null) {
                try {
                    var entityItem = (e.OriginalSource as FrameworkElement)?.DataContext as TNDEntityItem;
                    if (entityItem != null) {
                        var taskQuery = model.Query(model.SelectedTarget, entityItem, true, false, false);
                        await taskQuery.ConfigureAwait(true);
                    }
                } catch (Exception exception) {
                    model.ApplicationBuis.OnFatalException("Button_Expand_Parent_Click", exception);
                }
            }
        }

        private async void Button_Expand_Children_Click(object sender, RoutedEventArgs e) {
            var model = (TNDSearchModel)this.DataContext;
            if (model != null) {
                try {
                    var entityItem = (e.OriginalSource as FrameworkElement)?.DataContext as TNDEntityItem;
                    if (entityItem != null) {
                        var taskQuery = model.Query(model.SelectedTarget, entityItem, false, false, true);
                        await taskQuery.ConfigureAwait(true);
                    }
                } catch (Exception exception) {
                    model.ApplicationBuis.OnFatalException("Button_Expand_Children_Click", exception);
                }
            }
        }

        private void treeViewSearchResult_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeView x=this.treeViewSearchResult;
            TreeViewItem y;
        }
    }
}
