using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using TagNDropLibrary;

namespace TagNDropBuis {
    public class TNDDropBuis : ITNDDropBuis {
        private readonly Timer _Timer;
        private TNDDropModel _DropModel;
        private ITNDApplicationBuis _ApplicationBuis;
        private ITNDDataAccess _DataAccess;
        private TNDFavorite[] _FavoriteCache;

        public TNDDropBuis() {
            this._Timer = new Timer();
            this._Timer.Interval = 30000;
            this._Timer.Enabled = false;
            //
            this._Timer.Elapsed += ((object sender, ElapsedEventArgs e) => {
                if (this._Timer.Enabled) {
                    this._Timer.Enabled = false;
                    this.Deactivated();
                }
            });

        }

        public ITNDDropBuis Init(TNDDropModel dropModel) {
            if (this._DropModel == null) {
                this._DropModel = dropModel;
                dropModel.Init(this);
            }
            return this;
        }

        public void Activating() {
            this._Timer.Enabled = false;
            this.showFavorites();
        }
        public void Deactivating() {
            this._Timer.Enabled = true;
        }
        public void Deactivated() {
            this._ApplicationBuis.HideDropWindow();
        }

        public void ApplyConfiguration(ITNDApplicationBuis applicationBuis) {
            this._ApplicationBuis = applicationBuis;
            this._DataAccess = applicationBuis.GetDataAccess();
            //
            // Load fav data
            this.showFavorites();
        }

        private void showFavorites() {
            var srcFavorite = this._ApplicationBuis.LoadFavorites();
            if (ReferenceEquals(this._FavoriteCache, srcFavorite)) { return; }
            this._FavoriteCache = srcFavorite;
            var now = System.DateTime.Now;
            System.DateTime loDate = System.DateTime.MaxValue;
            System.DateTime hiDate = System.DateTime.MinValue;
            int loUsed = 10000;
            int hiUsed = 0;
            var cfg = this._ApplicationBuis.ApplicationModel.Configuration;
            foreach (var fav in srcFavorite) {
                fav.MetaEntity = cfg.FindMetaEntity(fav.MetaEntityName);
                if (loDate > fav.LastUsed) { loDate = fav.LastUsed; }
                if (hiDate < fav.LastUsed) { hiDate = fav.LastUsed; }
                if (loUsed > fav.Used) { loUsed = fav.Used; }
                if (hiUsed < fav.Used) { hiUsed = fav.Used; }
            }
            if (loDate > now) { loDate = now; }
            if (hiDate < now) { hiDate = now; }
            var dayDiff = (hiDate - loDate).TotalDays;
            if (dayDiff <= 1.0d) { dayDiff = 1.0d; }
            var usedDiff = (double)((hiUsed - loUsed) + 1);
            // and sort
            {
                //
                // order
                Func<int, int, int> intCompare = Comparer<int>.Default.Compare;
                Func<string, string, int> stringCompare = StringComparer.InvariantCultureIgnoreCase.Compare;
                Array.Sort<TNDFavorite>(srcFavorite, (a, b) => {
                    int result;
                    result = intCompare(a.Used, b.Used);
                    if (result != 0) { return -result; }
                    //
                    result = intCompare((a.MetaEntity?.MetaEntityLevel).GetValueOrDefault(), (b.MetaEntity?.MetaEntityLevel).GetValueOrDefault());
                    if (result != 0) { return result; }
                    //
                    return stringCompare(a.Name, b.Name);
                });
                //
                // reorder
                int order = 0;
                foreach (var fav in srcFavorite) { fav.Order = order++; }
#if false
                int length = srcFavorite.Length;
                int length1 = length - 1;
                for (int aIdx = (length - 1) / 2; aIdx >= 0; aIdx -= 3) {
                    int bIdx = length1 - aIdx;
                    if (aIdx != bIdx) {
                        var aOrder = srcFavorite[aIdx].Order;
                        var bOrder = srcFavorite[bIdx].Order;
                        srcFavorite[aIdx].Order = bOrder;
                        srcFavorite[bIdx].Order = aOrder;
                    }
                }
                //
                for (int aIdx = length - 9; aIdx >= 0; aIdx -= 7) {
                    int bIdx = aIdx + 8;
                    var aOrder = srcFavorite[aIdx].Order;
                    var bOrder = srcFavorite[bIdx].Order;
                    srcFavorite[aIdx].Order = bOrder;
                    srcFavorite[bIdx].Order = aOrder;
                }
#endif
                //
                //Array.Sort<TNDFavorite>(srcFavorite, (a, b) => intCompare(a.Order, b.Order));
                double lengthA1 = srcFavorite.Length + 1;
                foreach (var fav in srcFavorite) {
                    //fav.OrderPercent = ((double)fav.Order + 1) / lengthA1;
                    fav.OrderPercent = ((double)(fav.Used - loUsed + 1)) / usedDiff;
                    fav.LastUsedPercent = (fav.LastUsed - loDate).TotalDays / dayDiff;
                }
            }
            //
            //{
            //    var dstFavorite = this._DropModel.Favorite;
            //    var dst = dstFavorite.ToDictionary(_ => _.IdOrName, StringComparer.InvariantCultureIgnoreCase);
            //    foreach (var fav in srcFavorite) {
            //        bool added = false;
            //        if (dst.ContainsKey(fav.IdOrName)) {
            //            for (var idx = 0; idx < dst.Count; idx++) {
            //                if (string.Equals(dstFavorite[idx].IdOrName, fav.IdOrName, StringComparison.InvariantCultureIgnoreCase)) {
            //                    dstFavorite.RemoveAt(idx);
            //                    if (fav.Order < dstFavorite.Count) {
            //                        dstFavorite.Insert(fav.Order, fav);
            //                    } else {
            //                        dstFavorite.Add(fav);
            //                    }
            //                    added = true;
            //                    break;
            //                }
            //            }
            //        }
            //        if (!added) {
            //            if (fav.Order < dstFavorite.Count) {
            //                dstFavorite.Insert(fav.Order, fav);
            //            } else {
            //                dstFavorite.Add(fav);
            //            }
            //        }
            //    }
            //}
            {
                var dstFavorite = this._DropModel.Favorite;
                dstFavorite.Replace(srcFavorite);
            }
            // dstFavorite.Clear();
            // applicationModel.Configuration
        }

        public void DoDragEnter(TNDFavorite favorite, IDataObject data) {
            // data
        }

        public void DoGiveFeedback(TNDFavorite favorite, GiveFeedbackEventArgs e) {
            e.Handled = true;
        }

        public void DoDrop(TNDFavorite favorite, IDataObject data) {
            this.handleDataObject(favorite, data);
        }

        public void DoPaste(TNDFavorite favorite, IDataObject data) {
            this.handleDataObject(favorite, data);
        }

        public void HandleDrops(TNDFavorite favorite, TNDDropItem[] drops) {
            this._DropModel.Drops.AddRange(drops);
            var lst = new List<TNDTargetModel>();
            foreach (var dropItem in drops) {
                if (dropItem.Content != null && dropItem.LastException == null) {
                    TNDTargetModel targetModel = new TNDTargetModel();
                    targetModel.DropItem = dropItem;
                    targetModel.Favorite = favorite;
                    targetModel.Init(this._DataAccess, this._ApplicationBuis);
                    lst.Add(targetModel);
                    targetModel.PrepareSearch();
                    //this._DataAccess.InitalSearch(favorite, dropItem);
                }
            }
            this._ApplicationBuis.ShowSearchWindow(lst.ToArray());
        }

        public void UpdateFormatDescription() {
            try {
                var status = this.GetFormatDescription(System.Windows.Clipboard.GetDataObject());
                if (status == null) {
                    status = "Drag n Drop oder Copy / Paste von Outlook oder Windows File Explorer.";
                } else {
                    // status = $"{status} ";
                }
                this._DropModel.Status = status;
            } catch {
            }
        }

        public string GetFormatDescription(IDataObject data) {
            if (data == null) { return null; }
            var formats = data.GetFormats();
            var hsFormats = new HashSet<string>(formats);
            if (IsOutlook(hsFormats)) { return "Outlook"; }
            if (IsFileDrop(hsFormats)) { return "Datei"; }
            return null;
        }

        private static bool IsFileDrop(HashSet<string> hsFormats) {
            return hsFormats.Contains("FileDrop");
        }

        private static bool IsOutlook(HashSet<string> hsFormats) {
            return hsFormats.Contains("RenPrivateMessages") && hsFormats.Contains("FileGroupDescriptorW") && hsFormats.Contains("FileContents");
        }

        private void handleDataObject(TNDFavorite favorite, IDataObject data) {
            if (data == null) { return; }
            var formats = data.GetFormats();
            var drops = new List<TNDDropItem>();
            var hsFormats = new HashSet<string>(formats);

            // is it Outlook?
            bool done = false;
            if (IsOutlook(hsFormats)) {
                var sd = new TNDClipboardSourceData(data);
                var fileContents = sd.ReadFileContents(true);
                if (fileContents != null) {
                    drops.AddRange(fileContents);
                    foreach (var dropItem in drops) {
                        System.Diagnostics.Debug.WriteLine(dropItem.Name);
                        //System.IO.File.WriteAllBytes(System.IO.Path.Combine(@"c:\temp", fc.Name), fc.Content);
                        if (dropItem.Content != null && dropItem.LastException == null) {
                            TNDMetaDataExtractor.ConvertFileContents(dropItem);
                        }
                    }
                    done = true;
                }
            }
            // is it Windows Explorer?
            if (!done && IsFileDrop(hsFormats)) {
                var sd = new TNDClipboardSourceData(data);
                var fileDrops = sd.ReadFileDrop();
                if (fileDrops != null) {
                    drops.AddRange(fileDrops);
                    foreach (var dropItem in drops) {
                        System.Diagnostics.Debug.WriteLine(dropItem.Name);
                        //System.IO.File.WriteAllBytes(System.IO.Path.Combine(@"c:\temp", fc.Name), fc.Content);
                        if (dropItem.Content != null && dropItem.LastException == null) {
                            TNDMetaDataExtractor.ConvertFileContents(dropItem);
                        }
                    }
                    done = true;
                }
            }
            //System.Diagnostics.Debug.WriteLine(formats.Length);
            if (drops.Count > 0) {
                this.HandleDrops(favorite, drops.ToArray());
            } else {
                //
            }
        }
    }
}
