using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagNDropLibrary {
    public interface ITNDKeyboardHook {
        ITNDKeyboardHook Init(TNDApplicationModel applicationModel);

        void Subscribe();

        void Unsubscribe();
    }
}
