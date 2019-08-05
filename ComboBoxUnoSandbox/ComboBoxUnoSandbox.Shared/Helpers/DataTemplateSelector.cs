using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ComboBoxUnoSandbox.Shared.Models;

namespace ComboBoxUnoSandbox.Shared.Helpers
{
    public class ParameterTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            return ComboHolder;
        }

        public DataTemplate ComboHolder { get; set; }
    }
}
