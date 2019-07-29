using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ComboBoxUnoSandbox.Shared.Helpers;
using ComboBoxUnoSandbox.Shared.Models;
using ComboBoxUnoSandbox.Shared.ViewModel;
using Microsoft.Toolkit.Uwp.UI.Converters;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using TheHub.UI.Controls;
#if __WASM__
using Uno.Extensions;
using ComboBoxUnoSandbox.Wasm.Annotations;
#endif

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ComboBoxUnoSandbox
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        ReportViewModel viewModel;

        IEnumerable<ReportParameterHolder> reportParameters;

        public MainPage()
        {
            this.InitializeComponent();
            viewModel = new ReportViewModel();
            DataContext = viewModel;
            DialogService.Instance.Dialog = overlayContentDialog;
        }

        private void XDropDownGlyph_OnClick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("ComboBox_OnDropDownOpened");
            var button = (Button)sender;
            var holder = button.Tag as ReportParameterHolder;
            holder.Value = null;
            var dcb = button.Parent.FindDescendant<DeletableComboBox>(); //TODO: Delete this when the Value change in a reactive way the SelectedIndex
            dcb.SelectedIndex = -1;
        }

        private void ComboBox_OnDropDownOpened(object sender, object e)
        {
            Console.WriteLine("X dropdown");
            var cb = sender as ComboBox;
            if (cb != null)
            {
                Console.WriteLine("items:" + cb.Items.Count);
            }
        }

        private string _prompt;

        public string Prompt
        {
            get => _prompt;
            set
            {
                _prompt = Value;
                OnPropertyChanged();
            }
        }

        private string _value;
        public string Value
        {
            get => Value;
            set
            {
                _value = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

#if __WASM__
        [NotifyPropertyChangedInvocator]
#endif
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public IEnumerable<ReportParameterHolder> ReportParameters
        {
            get { return reportParameters; }
            set
            {
                reportParameters = value;
                OnPropertyChanged();
            }
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("ReportButton was click");
            viewModel.ReportButtonClick(sender as Button, paramPanel);
        }
    }
}
