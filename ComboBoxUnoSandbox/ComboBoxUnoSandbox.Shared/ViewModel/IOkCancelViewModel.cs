using System;
using System.Collections.Generic;
using System.Text;
using ComboBoxUnoSandbox.Shared.Helpers;

namespace ComboBoxUnoSandbox.Shared.ViewModel
{
    public interface IOKCancelViewModel : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="window"></param>
        /// <returns>true to indicate the window should be closed</returns>
        bool OK(object window);

        /// <summary>
        /// 
        /// </summary>
        /// <returns>true to indicate the window should be closed</returns>
        bool Cancel();

        bool OkCanExecute(object obj);
        bool CancelCanExecute(object obj);
        IInvalidateCommand OkCommand { get; set; }
        IInvalidateCommand CancelCommand { get; set; }
        string OkText { get; }
        string CancelText { get; }
        object View { get; set; }
        string Title { get; }
    }
}
