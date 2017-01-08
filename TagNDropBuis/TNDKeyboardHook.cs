using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gma.System.MouseKeyHook;
using TagNDropLibrary;

namespace TagNDropBuis {
    public class TNDKeyboardHook : ITNDKeyboardHook, IDisposable {
        private TNDApplicationModel _ApplicationModel;
        private IKeyboardMouseEvents _GlobalEvents;

        public TNDKeyboardHook() {
        }

        public ITNDKeyboardHook Init(TNDApplicationModel applicationModel) {
            this._ApplicationModel = applicationModel;
            return this;
        }

        public void Subscribe() {
            var cfg = this._ApplicationModel.Configuration;
            if (this._GlobalEvents == null) {
                this._GlobalEvents = Gma.System.MouseKeyHook.Hook.GlobalEvents();
                //this._GlobalEvents.KeyPress += this.globalEvents_KeyPress;
                this._GlobalEvents.KeyDown += this.globalEvents_KeyDown;
            }
        }

        public void Unsubscribe() {
            if (this._GlobalEvents != null) {
                try {
                    using (var ge = this._GlobalEvents as IDisposable) {
                        this._GlobalEvents.KeyDown -= this.globalEvents_KeyDown;
                        //this._GlobalEvents.KeyPress -= this.globalEvents_KeyPress;
                        this._GlobalEvents = null;
                    }
                } catch (Exception exception) {
                }
            }
        }

        public void Dispose() {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing) {
            if (this._GlobalEvents != null) {
                this.Unsubscribe();
                if (disposing) {
                    this._GlobalEvents = null;
                }
            }
        }

        private void globalEvents_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {
            if (e.KeyCode == System.Windows.Forms.Keys.O) {
                bool showDropWindow = false;
                var lwin = Native.GetAsyncKeyState((int)System.Windows.Forms.Keys.LWin);
                System.Diagnostics.Debug.WriteLine("lwin:" + lwin);
                //if (lwin == -32767) {
                if (lwin != 0) {
                    showDropWindow = true;
                } else {
                    var rwin = Native.GetAsyncKeyState((int)System.Windows.Forms.Keys.RWin);
                    if (rwin != 0) {
                        showDropWindow = true;
                    }
                }
                //
                if (showDropWindow) {
                    this._ApplicationModel?.ShowDropWindow();
                }
            }
            //if ((e.KeyData & System.Windows.Forms.Keys.KeyCode) == System.Windows.Forms.Keys.F2) {
            //}
            //GetAsyncKeyState
            //System.Diagnostics.Debug.WriteLine("m:" + e.Modifiers);
            //System.Diagnostics.Debug.WriteLine("v:" + e.KeyValue);
            //System.Diagnostics.Debug.WriteLine("d:" + e.KeyData);
            //System.Diagnostics.Debug.WriteLine("c:" + e.KeyCode);
        }

        //private void globalEvents_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e) {
        //    //if (e.KeyChar==
        //}

        ~TNDKeyboardHook() { this.Dispose(false); }

        private class Native {
            [System.Runtime.InteropServices.DllImport("User32.dll")]
            public static extern short GetAsyncKeyState(System.Int32 vKey);
        }
    }
}
