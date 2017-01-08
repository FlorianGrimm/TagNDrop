using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Threading;

namespace TagNDropLibrary {
    public class BulkObservableCollection<T> : ObservableCollection<T> {
        private static readonly PropertyChangedEventArgs _countChanged = new PropertyChangedEventArgs("Count");

        private static readonly PropertyChangedEventArgs _indexerChanged = new PropertyChangedEventArgs("Item[]");

        private static readonly NotifyCollectionChangedEventArgs _resetChange = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);

        private int _RangeOperationCount;

        private bool _CollectionChangedDuringRangeOperation;

        private Dispatcher _Dispatcher;

        private ReadOnlyObservableCollection<T> _ReadOnlyAccessor;

        public BulkObservableCollection() {
            this._Dispatcher = Dispatcher.CurrentDispatcher;
        }

        private delegate void AddRangeCallback(IList<T> items);

        private delegate void SetItemCallback(int index, T item);

        private delegate void RemoveItemCallback(int index);

        private delegate void ClearItemsCallback();

        private delegate void InsertItemCallback(int index, T item);

        private delegate void MoveItemCallback(int oldIndex, int newIndex);

        public void Replace(IEnumerable<T> items) {
            if (items == null || !items.Any<T>()) {
                return;
            }
            if (this._Dispatcher.CheckAccess()) {
                try {
                    this.BeginBulkOperation();
                    this._CollectionChangedDuringRangeOperation = true;
                    if (this.Count > 0) {
                        base.ClearItems();
                    }
                    using (IEnumerator<T> enumerator = items.GetEnumerator()) {
                        while (enumerator.MoveNext()) {
                            T current = enumerator.Current;
                            base.Items.Add(current);
                        }
                        return;
                    }
                } finally {
                    this.EndBulkOperation();
                }
            } else {
                this._Dispatcher.BeginInvoke(DispatcherPriority.Send, new BulkObservableCollection<T>.AddRangeCallback(this.Replace), items);
            }
        }
        public void AddRange(IEnumerable<T> items) {
            if (items == null || !items.Any<T>()) {
                return;
            }
            if (this._Dispatcher.CheckAccess()) {
                try {
                    this.BeginBulkOperation();
                    this._CollectionChangedDuringRangeOperation = true;
                    using (IEnumerator<T> enumerator = items.GetEnumerator()) {
                        while (enumerator.MoveNext()) {
                            T current = enumerator.Current;
                            base.Items.Add(current);
                        }
                        return;
                    }
                } finally {
                    this.EndBulkOperation();
                }
            } else {
                this._Dispatcher.BeginInvoke(DispatcherPriority.Send, new BulkObservableCollection<T>.AddRangeCallback(this.AddRange), items);
            }
        }

        public void BeginBulkOperation() {
            this._RangeOperationCount++;
            this._CollectionChangedDuringRangeOperation = false;
        }

        public void EndBulkOperation() {
            if (this._RangeOperationCount > 0) {
                int num = this._RangeOperationCount - 1;
                this._RangeOperationCount = num;
                if (num == 0 && this._CollectionChangedDuringRangeOperation) {
                    this.OnPropertyChanged(BulkObservableCollection<T>._countChanged);
                    this.OnPropertyChanged(BulkObservableCollection<T>._indexerChanged);
                    this.OnCollectionChanged(BulkObservableCollection<T>._resetChange);
                }
            }
        }

        public ReadOnlyObservableCollection<T> AsReadOnly() {
            if (this._ReadOnlyAccessor == null) {
                this._ReadOnlyAccessor = new ReadOnlyObservableCollection<T>(this);
            }
            return this._ReadOnlyAccessor;
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e) {
            if (this._RangeOperationCount == 0) {
                base.OnPropertyChanged(e);
                return;
            }
            this._CollectionChangedDuringRangeOperation = true;
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e) {
            if (this._RangeOperationCount == 0) {
                base.OnCollectionChanged(e);
                return;
            }
            this._CollectionChangedDuringRangeOperation = true;
        }

        protected override void SetItem(int index, T item) {
            if (this._Dispatcher.CheckAccess()) {
                base.SetItem(index, item);
                return;
            }
            this._Dispatcher.BeginInvoke(
                DispatcherPriority.Send,
                new BulkObservableCollection<T>.SetItemCallback(this.SetItem),
                index,
                new object[] { item });
        }

        protected override void InsertItem(int index, T item) {
            if (!this._Dispatcher.CheckAccess()) {
                this._Dispatcher.BeginInvoke(
                    DispatcherPriority.Send,
                    new BulkObservableCollection<T>.InsertItemCallback(this.InsertItem),
                    index,
                    new object[] { item });
                return;
            }
            if (this._RangeOperationCount == 0) {
                base.InsertItem(index, item);
                return;
            }
            base.Items.Insert(index, item);
            this._CollectionChangedDuringRangeOperation = true;
        }

        protected override void MoveItem(int oldIndex, int newIndex) {
            if (this._Dispatcher.CheckAccess()) {
                base.MoveItem(oldIndex, newIndex);
                return;
            }
            this._Dispatcher.BeginInvoke(
                DispatcherPriority.Send,
                new BulkObservableCollection<T>.MoveItemCallback(this.MoveItem),
                oldIndex,
                new object[] { newIndex });
        }

        protected override void RemoveItem(int index) {
            if (this._Dispatcher.CheckAccess()) {
                base.RemoveItem(index);
                return;
            }
            this._Dispatcher.BeginInvoke(DispatcherPriority.Send, new BulkObservableCollection<T>.RemoveItemCallback(this.RemoveItem), index);
        }

        protected override void ClearItems() {
            if (this._Dispatcher.CheckAccess()) {
                if (this.Count > 0) {
                    base.ClearItems();
                    return;
                }
            } else {
                this._Dispatcher.BeginInvoke(DispatcherPriority.Send, new BulkObservableCollection<T>.ClearItemsCallback(this.ClearItems));
            }
        }
    }
}
