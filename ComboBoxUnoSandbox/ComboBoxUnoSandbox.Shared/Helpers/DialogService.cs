using ComboBoxUnoSandbox.Shared.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComboBoxUnoSandbox.Shared.Helpers
{
    public class DialogService
    {
        public static readonly DialogService Instance = new DialogService();

        DialogService()
        {
        }

        public OverlayContentDialog Dialog { get; set; }

        public void Show(object dialogContent)
        {
            Dialog.DialogContent = dialogContent;
            Dialog.ShowDialog();
        }

        public void CloseDialog()
        {
            Dialog.DialogContent = null;
            Dialog.CloseDialog();
        }
    }
}
