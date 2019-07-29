using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace ComboBoxUnoSandbox.Shared.Controls
{
    public class OverlayContentDialog : ContentControl
    {
        public object DialogContent
        {
            get { return GetValue(DialogContentProperty); }
            set { SetValue(DialogContentProperty, value); }
        }

        public static readonly DependencyProperty DialogContentProperty =
            DependencyProperty.Register("DialogContent", typeof(object), typeof(OverlayContentDialog), null);

        public void ShowDialog()
        {
            var c = FindVisualChildren<FrameworkElement>(this).FirstOrDefault(ch => ch.Name == "dialogGrid");
            if (c != null)
            {
                c.Visibility = Visibility.Visible;
            }
            else throw new Exception("Attempt to show dialog but none was found in visual children");
        }

        public void CloseDialog()
        {
            var c = FindVisualChildren<FrameworkElement>(this).FirstOrDefault(ch => ch.Name == "dialogGrid");
            if (c != null)
            {
                c.Visibility = Visibility.Collapsed;
            }
            else throw new Exception("Attempt to show dialog but none was found in visual children");
        }

        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}
