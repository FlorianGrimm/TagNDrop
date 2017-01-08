using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagNDropLibrary;

namespace TagNDropBuis {
    public class SortHelper {
        internal TNDEntityItem Item;
        internal SortHelper Parent;
        internal List<SortHelper> Children;
        internal int MatchIndex;
        internal int Match;
        internal TNDMetaEntity MetaEntity;
        internal object Key;
        internal TNDMetaEntity MetaEntityParent;
        internal object ParentKey;

        public static TNDEntityItem[] Sort(TNDEntityItem[] items) {
            if (items == null || items.Length == 0) { return new TNDEntityItem[0]; }
            var len = items.Length;
            SortHelper[] shs = new SortHelper[len];
            int matchMax = 0;
            int matchMin = int.MaxValue;
            for (int idx = 0; idx < len; idx++) {
                var sh = new SortHelper();
                var item = items[idx];
                shs[idx] = sh;
                sh.Item = item;
                var match = ((item.GetProperty("Match"))?.GetValueAsInt()).GetValueOrDefault(0);
                sh.Match = match;
                if (match < matchMin) { matchMin = match; }
                if (matchMax < match) { matchMax = match; }
                sh.MatchIndex = idx;
            }
            double matchDif = (matchMax - matchMin) + 1;

            Array.Sort(shs, (a, b) => {
                if (ReferenceEquals(a, b)) { return 0; }
                //
                int result = Comparer<int>.Default.Compare(b.Match, a.Match);
                if (result != 0) { return result; }
                //
                result = Comparer<int>.Default.Compare(a.Item.MetaEntity.MetaEntityOrder, b.Item.MetaEntity.MetaEntityOrder);
                if (result != 0) { return result; }
                //
                result = StringComparer.InvariantCultureIgnoreCase.Compare(a.Item.Name, b.Item.Name);
                if (result != 0) { return result; }
                //
                result = Comparer<int>.Default.Compare(a.MatchIndex, b.MatchIndex);
                return result;
            });
            var shByKey = new Dictionary<TNDMetaEntity, Dictionary<object, SortHelper>>();
            for (int idx = 0; idx < len; idx++) {
                var sh = shs[idx];
                sh.Item.MatchPercent = (sh.Match + 1) / matchDif;
                sh.MatchIndex = idx;
                sh.MetaEntity = sh.Item.MetaEntity;
                sh.Key = sh.Item.GetKeyValue();
                sh.MetaEntityParent = sh.Item.MetaEntity.MetaEntityParent;
                if (sh.MetaEntityParent != null) {
                    sh.ParentKey = sh.Item.GetParentKeyValue();
                }
                if (sh.Key != null) {
                    Dictionary<object, SortHelper> d;
                    if (shByKey.TryGetValue(sh.MetaEntity, out d)) {
                        d[sh.Key] = sh;
                    } else {
                        d = new Dictionary<object, SortHelper>();
                        d[sh.Key] = sh;
                        shByKey[sh.MetaEntity] = d;
                    }
                }
            }
            for (int idx = 0; idx < len; idx++) {
                var sh = shs[idx];
                if (sh.MetaEntityParent != null && sh.ParentKey != null) {
                    Dictionary<object, SortHelper> d;
                    if (shByKey.TryGetValue(sh.MetaEntityParent, out d)) {
                        SortHelper shParent;
                        if (d.TryGetValue(sh.ParentKey, out shParent)) {
                            sh.Parent = shParent;
                        }
                    }
                }
            }
            for (int idx = 0; idx < len; idx++) {
                var sh = shs[idx];
                if (sh.MetaEntityParent != null && sh.ParentKey != null) {
                    var shParent = sh.Parent;
                    if ((object)shParent != null) {
                        if ((object)shParent.Children == null) {
                            shParent.Children = new List<SortHelper>();
                        }
                        shParent.Children.Add(sh);
                    }
                }
            }
            var s = new List<TNDEntityItem>();
            for (int idx = 0; idx < len; idx++) {
                var sh = shs[idx];
                if ((object)sh.Parent == null) {
                    addRec(s, sh);
                }
            }
            return s.ToArray();
        }

        private static void addRec(List<TNDEntityItem> lst, SortHelper sh) {
            lst.Add(sh.Item);
            if ((object)sh.Children != null) {
                for (int idx = 0; idx < sh.Children.Count; idx++) {
                    addRec(lst, sh.Children[idx]);
                }
            }
        }
    }
}
