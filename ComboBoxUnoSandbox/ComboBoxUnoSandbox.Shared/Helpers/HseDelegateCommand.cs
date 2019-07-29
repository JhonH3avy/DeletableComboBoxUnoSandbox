using System;
using System.Collections.Generic;
using System.Text;

namespace ComboBoxUnoSandbox.Shared.Helpers
{
    public class HseDelegateCommand : IInvalidateCommand
    {
        readonly Action<object> execute;
        readonly Predicate<object> canExecute;

        public HseDelegateCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object o)
        {
            if (canExecute == null) return true;
            return canExecute(o);
        }

        public void Execute(object o)
        {
            execute?.Invoke(o);
        }

        public void InvalidateCanExecute()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler CanExecuteChanged;
    }
}
