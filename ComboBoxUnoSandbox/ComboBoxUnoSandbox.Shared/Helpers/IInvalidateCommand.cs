using System.Windows.Input;

namespace ComboBoxUnoSandbox.Shared.Helpers
{
    public interface IInvalidateCommand : ICommand
    {
        void InvalidateCanExecute();
    }
}
