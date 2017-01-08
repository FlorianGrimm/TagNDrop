using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagNDropLibrary
{
    public interface ITNDNotifyIconBuis
    {
        ITNDNotifyIconBuis Init(TNDApplicationModel applicationModel);

        void ShowError(string message);

        void ShowMessage(string message);
    }
}
