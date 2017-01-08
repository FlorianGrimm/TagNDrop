using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagNDropLibrary;

namespace TagNDropBuis {
    public class TNDNotifyIconBuis : ITNDNotifyIconBuis, IDisposable {
        private readonly TNDApplicationModel _ApplicationModel;
        private System.Windows.Forms.NotifyIcon NIcon;

        public TNDNotifyIconBuis() {
        }

        public TNDNotifyIconBuis(TNDApplicationModel applicationModel) {
            this._ApplicationModel = applicationModel;
            this.NIcon = new System.Windows.Forms.NotifyIcon();
            var msIcon = this.GetType().Assembly.GetManifestResourceStream("TagNDropBuis.Task.ico");
            this.NIcon.Icon = new System.Drawing.Icon(msIcon);
            this.NIcon.Click += this.NIcon_Click;
            this.NIcon.Text = ConstsLibrary.ProductName;
            this.NIcon.BalloonTipTitle = ConstsLibrary.ProductName;
        }

        public ITNDNotifyIconBuis Init(TNDApplicationModel applicationModel) {
            var that = new TNDNotifyIconBuis(applicationModel);
            that.NIcon.Visible = true;
            return that;
        }

        private void NIcon_Click(object sender, EventArgs e) {
            this._ApplicationModel.ShowDropWindow();
        }

        public void ShowError(string message) {
            this.NIcon.ShowBalloonTip(1500, ConstsLibrary.ProductName, message, System.Windows.Forms.ToolTipIcon.Error);
        }

        public void ShowMessage(string message) {
            this.NIcon.ShowBalloonTip(1500, ConstsLibrary.ProductName, message, System.Windows.Forms.ToolTipIcon.Info);
        }

        public void Dispose() { this.Dispose(true); System.GC.SuppressFinalize(this); }

        ~TNDNotifyIconBuis() { try { this.Dispose(false); } catch { } }

        private void Dispose(bool disposing) {
            using (var icon = this.NIcon) {
                if (icon != null) {
                    try {
                        icon.Visible = false;
                    } catch { }
                    if (disposing) {
                        this.NIcon = null;
                    }
                }
            }
        }
    }
}
