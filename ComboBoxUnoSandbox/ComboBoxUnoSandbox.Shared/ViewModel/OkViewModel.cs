using System;
using System.Collections.Generic;
using System.Text;

namespace ComboBoxUnoSandbox.Shared.ViewModel
{
    public class OkViewModel : BusyOkCancelViewModelBase
    {
        public OkViewModel()
        {
            OkText = "Ok";
        }

        public override bool OK(object window)
        {
            return true;
        }
    }
}
