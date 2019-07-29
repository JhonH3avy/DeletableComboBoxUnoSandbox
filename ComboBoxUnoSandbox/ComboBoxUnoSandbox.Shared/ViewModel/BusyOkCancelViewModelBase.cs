using ComboBoxUnoSandbox.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComboBoxUnoSandbox.Shared.ViewModel
{
    public abstract class BusyOkCancelViewModelBase : ViewModelBase, IOKCancelViewModel
    {
        bool isBusy;

        protected BusyOkCancelViewModelBase()
        {
            OkText = "OK";
            CancelText = "Cancel";
            Title = "Ok?";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="window"></param>
        /// <returns>true to indicate the window can be closed</returns>
		public abstract bool OK(object window);

        public virtual bool Cancel()
        {
            return true;
        }

        public virtual bool OkCanExecute(object obj)
        {
            return !IsBusy;
        }

        public virtual bool CancelCanExecute(object obj)
        {
            return !IsBusy;
        }

        public IInvalidateCommand OkCommand { get; set; }
        public IInvalidateCommand CancelCommand { get; set; }

        string okText;
        public string OkText
        {
            get { return okText; }
            set { SetAndRaiseChanged(ref okText, value); }
        }

        public string CancelText { get; set; }

        public object View { get; set; }

        public string Title { get; set; }

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                if (value != isBusy)
                {
                    isBusy = value;
                    RaiseChanged();
                    if (OkCommand != null) OkCommand.InvalidateCanExecute();
                    if (CancelCommand != null) CancelCommand.InvalidateCanExecute();
                }
            }
        }

        public virtual void Dispose()
        {
        }
    }
}
