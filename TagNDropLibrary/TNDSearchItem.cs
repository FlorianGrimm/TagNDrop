using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagNDropLibrary {
    public class TNDSearchItem : TNDNotifyPropertyChanged {
        private string _MetaEntityName;
        private string _Search;
        //public TNDMetaEntity MetaEntity;

        public TNDSearchItem() { }

        public string MetaEntityName { get { return this._MetaEntityName; } set { this._MetaEntityName = value; this.OnPropertyChanged(nameof(this.MetaEntityName)); } }

        public string Search {
            get {
                return this._Search;
            }
            set {
                if (string.Equals(this._Search, value, StringComparison.Ordinal)) { return; }
                this._Search = value;
                this.SearchTerm = null;
                this.Emails = null;
                this.EmailDomains = null;
                this.OnPropertyChanged(nameof(this.Search));
            }
        }

        public string SearchTerm { get; set; }
        public string Emails { get; set; }
        public string EmailDomains { get; set; }
        public TNDMetaEntity MetaEntity { get; internal set; }

        public void SetProperties(string searchTerm, string emails, string emailDomains) {
            if (emails == null) { emails = string.Empty; }
            var arrEmails = emails
                        .Split(" \t\\/,;<>".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                        ;
            if (emailDomains == null) {
                emailDomains = string.Join(
                    " ",
                    arrEmails
                        .Select(_ => _.Split('@').LastOrDefault())
                        .Distinct()
                    )
                    ;
            }
            string normalizedEmail = string.Join(
                " ",
                arrEmails.Select(_ => (_.StartsWith(ConstsLibrary.SearchEmailPrefix, StringComparison.OrdinalIgnoreCase) ? _ : $"{ConstsLibrary.SearchEmailPrefix}{_}")));
            // Search has side effects und must be the fist one
            this.Search = $"{normalizedEmail} {searchTerm}";
            this.SearchTerm = searchTerm;
            this.Emails = emails;
            this.EmailDomains = emailDomains;
        }

        public void ParseSearch() {
            if (this.SearchTerm != null) {
                return;
            }
            var search = (this.Search ?? string.Empty).Trim();
            if (search == string.Empty) {
                this.SearchTerm = null;
                this.Emails = null;
                this.EmailDomains = null;
                return;
            }
            var searchTerms = new List<string>();
            var emails = new List<string>();
            foreach (var p in search.Split(" \t\\/,;<>".ToCharArray())) {
                if (string.IsNullOrEmpty(p)) { continue; }
                if (p.StartsWith(ConstsLibrary.SearchEmailPrefix, StringComparison.OrdinalIgnoreCase)) {
                    emails.Add(p.Substring(ConstsLibrary.SearchEmailPrefix.Length));
                } else if (p.Contains("@")) {
                    emails.Add(p);
                } else {
                    searchTerms.Add(p);
                }
            }
            this.SearchTerm = string.Join(" ", searchTerms.Select(_ => makeLike(_)));
            this.Emails = string.Join(" ", emails.Where(_ => !_.TrimStart('%').StartsWith("@")).Select(_ => makeLike(_)));
            this.EmailDomains = string.Join(" ", emails.Select(_ => _.Split("@".ToCharArray()).LastOrDefault()).Select(_ => makeLikeStart(_).Distinct()));
        }

        private static string makeLikeStart(string term) {
            if (string.IsNullOrEmpty(term)) { return string.Empty; }
            if (term.Contains("%")) { return term; }
            return ("%" + term);
        }

        private static string makeLike(string term) {
            if (string.IsNullOrEmpty(term)) { return string.Empty; }
            if (term.Contains("%")) { return term; }
            return ("%" + term + "%");
        }
    }
}
