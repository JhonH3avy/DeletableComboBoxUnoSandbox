using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.System;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace TheHub.UI.Controls
{
    [Windows.UI.Xaml.Data.Bindable]
    public class DeletableComboBox : ComboBox
    {
        public DeletableComboBox()
        {
            KeyDown += DeletableComboBoxKeyDown;
            IsSynchronizedWithCurrentItem = false;
            DefaultStyleKey = typeof(DeletableComboBox);
        }

        public event EventHandler<CancelEventArgs> Deleting;

        protected virtual void OnDeleting(CancelEventArgs e)
        {
            var handler = Deleting;
            if (handler != null) handler(this, e);
        }

        public static readonly DependencyProperty DeletableProperty = DependencyProperty.Register(
            "Deletable", typeof(bool), typeof(DeletableComboBox), new PropertyMetadata(true));

        public bool Deletable
        {
            get { return (bool)GetValue(DeletableProperty); }
            set { SetValue(DeletableProperty, value); }
        }

        void DeletableComboBoxKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Delete || e.Key == VirtualKey.Back)
            {
                var args = new CancelEventArgs();
                OnDeleting(args);
                if (args.Cancel) return;
                SelectedItem = null;
            }
        }
    }
}