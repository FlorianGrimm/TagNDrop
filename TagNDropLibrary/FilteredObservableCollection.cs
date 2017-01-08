using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;

namespace TagNDropLibrary {
    public class FilteredObservableCollection<T> : IList, ICollection, IEnumerable, IList<T>, ICollection<T>, IEnumerable<T>, INotifyCollectionChanged {
        private IList<T> _underlyingList;

        private bool _isFiltering;

        private Predicate<T> _filterPredicate;

        private List<T> _filteredList = new List<T>();

        public FilteredObservableCollection(IList<T> underlyingList) {
            if (underlyingList == null) {
                throw new ArgumentNullException("underlyingList");
            }
            if (!(underlyingList is INotifyCollectionChanged)) {
                throw new ArgumentException("Underlying collection must implement INotifyCollectionChanged", "underlyingList");
            }
            if (!(underlyingList is IList)) {
                throw new ArgumentException("Underlying collection must implement IList", "underlyingList");
            }
            this._underlyingList = underlyingList;
            ((INotifyCollectionChanged)this._underlyingList).CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnUnderlyingList_CollectionChanged);
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public bool IsFixedSize => false;

        public bool IsSynchronized => false;

        public object SyncRoot => (this._isFiltering)
                    ? ((ICollection)this._filteredList).SyncRoot
                    : ((IList)this._underlyingList).SyncRoot;

        public int Count =>
            (this._isFiltering)
            ? this._filteredList.Count
            : this._underlyingList.Count;

        public bool IsReadOnly => true;

        public T this[int index] {
            get {
                if (this._isFiltering) {
                    return this._filteredList[index];
                }
                return this._underlyingList[index];
            }
            set {
                throw new InvalidOperationException("FilteredObservableCollections are read-only");
            }
        }

        object IList.this[int index] {
            get {
                return ((IList<T>)this)[index];
            }
            set {
                throw new InvalidOperationException("FilteredObservableCollections are read-only");
            }
        }

        public int Add(object value) {
            throw new InvalidOperationException("FilteredObservableCollections are read-only");
        }

        public bool Contains(object value) {
            return ((ICollection<T>)this).Contains((T)((object)value));
        }

        public int IndexOf(object value) {
            return ((IList<T>)this).IndexOf((T)((object)value));
        }

        public void Insert(int index, object value) {
            throw new InvalidOperationException("FilteredObservableCollections are read-only");
        }

        public void Remove(object value) {
            throw new InvalidOperationException("FilteredObservableCollections are read-only");
        }

        public void CopyTo(Array array, int index) {
            if (this._isFiltering) {
                if (array.Length - index < this.Count) {
                    throw new ArgumentException("Array not big enough", "array");
                }
                int num = index;
                using (List<T>.Enumerator enumerator = this._filteredList.GetEnumerator()) {
                    while (enumerator.MoveNext()) {
                        T current = enumerator.Current;
                        array.SetValue(current, num);
                        num++;
                    }
                    return;
                }
            }
            ((IList)this._underlyingList).CopyTo(array, index);
        }

        public int IndexOf(T item) {
            if (this._isFiltering) {
                return this._filteredList.IndexOf(item);
            }
            return this._underlyingList.IndexOf(item);
        }

        public void Insert(int index, T item) {
            throw new InvalidOperationException("FilteredObservableCollections are read-only");
        }

        public void RemoveAt(int index) {
            throw new InvalidOperationException("FilteredObservableCollections are read-only");
        }

        public void Add(T item) {
            throw new InvalidOperationException("FilteredObservableCollections are read-only");
        }

        public void Clear() {
            throw new InvalidOperationException("FilteredObservableCollections are read-only");
        }

        public bool Contains(T item) {
            if (this._isFiltering) {
                return this._filteredList.Contains(item);
            }
            return this._underlyingList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex) {
            if (this._isFiltering) {
                this._filteredList.CopyTo(array, arrayIndex);
                return;
            }
            this._underlyingList.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item) {
            throw new InvalidOperationException("FilteredObservableCollections are read-only");
        }

        public IEnumerator<T> GetEnumerator() {
            if (this._isFiltering) {
                return this._filteredList.GetEnumerator();
            }
            return this._underlyingList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            if (this._isFiltering) {
                return ((IEnumerable)this._filteredList).GetEnumerator();
            }
            return this._underlyingList.GetEnumerator();
        }

        public void Filter(Predicate<T> filterPredicate) {
            if (filterPredicate == null) {
                throw new ArgumentNullException("filterPredicate");
            }
            this._filterPredicate = filterPredicate;
            this._isFiltering = true;
            this.UpdateFilteredItems();
            this.RaiseCollectionChanged();
        }

        public void StopFiltering() {
            if (this._isFiltering) {
                this._filterPredicate = null;
                this._isFiltering = false;
                this.UpdateFilteredItems();
                this.RaiseCollectionChanged();
            }
        }

        private void OnUnderlyingList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            this.UpdateFilteredItems();
            this.RaiseCollectionChanged();
        }

        private void RaiseCollectionChanged() {
            this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        private void UpdateFilteredItems() {
            this._filteredList.Clear();
            if (this._isFiltering) {
                foreach (T current in this._underlyingList) {
                    if (this._filterPredicate(current)) {
                        this._filteredList.Add(current);
                    }
                }
            }
        }
    }
}
