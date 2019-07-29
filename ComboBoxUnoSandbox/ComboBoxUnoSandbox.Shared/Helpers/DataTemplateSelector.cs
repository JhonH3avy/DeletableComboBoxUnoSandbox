using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ComboBoxUnoSandbox.Shared.Models;

namespace ComboBoxUnoSandbox.Shared.Helpers
{
    public class ParameterTemplateSelector : DataTemplateSelector
    {
        public static readonly DependencyProperty MultiSelectStandardComboHolderProperty = DependencyProperty.Register("MultiSelectStandardComboHolder", typeof(DataTemplate), typeof(ParameterTemplateSelector), new PropertyMetadata(default(DataTemplate)));

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var element = container as FrameworkElement;

            if (element != null && item != null && item is ReportParameterHolder)
            {
                var holder = item as ReportParameterHolder;
                if (holder.MultiValue) return holder.ItemsSource != null && holder.ItemsSource.Contains("{") ? MultiSelectStandardComboHolder : MultiSelectComboHolder;
                if (!string.IsNullOrEmpty(holder.ItemsSource))
                {
                    return holder.ItemsSource.Contains("{") ? ComboStandardHolder : ComboHolder;
                }
                if (holder.ParameterType == HseParameterType.Boolean) return BoolHolder;
                if (holder.ParameterType == HseParameterType.Integer) return IntHolder;
                if (holder.ParameterType == HseParameterType.DateTime) return DateHolder;
                return StringHolder;
            }

            return null;
        }

        public DataTemplate DateHolder { get; set; }
        public DataTemplate ComboHolder { get; set; }
        public DataTemplate ComboStandardHolder { get; set; }
        public DataTemplate MultiSelectComboHolder { get; set; }
        public DataTemplate BoolHolder { get; set; }
        public DataTemplate StringHolder { get; set; }
        public DataTemplate IntHolder { get; set; }
        public DataTemplate MultiSelectStandardComboHolder { get; set; }
    }
}
