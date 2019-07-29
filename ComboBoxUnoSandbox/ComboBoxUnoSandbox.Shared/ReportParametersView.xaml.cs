using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ComboBoxUnoSandbox.Shared.Models;
using ComboBoxUnoSandbox.Shared.ViewModel;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using TheHub.UI.Controls;

namespace ComboBoxUnoSandbox.Shared
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ReportParametersView
    {
        public ReportParametersView()
        {
            this.InitializeComponent();
            var viewModel = new ReportParametersViewModel();
            DataContext = viewModel;
        }

        void ComboBox_OnDropDownOpened(object sender, object e)
        {
            var cb = sender as ComboBox;
            if (cb != null)
            {
                Console.WriteLine("items:" + cb.Items.Count);
            }
        }

        private void XDropDownGlyph_OnClick(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var holder = button.Tag as ReportParameterHolder;
            holder.Value = null;
            var dcb = button.Parent.FindDescendant<DeletableComboBox>(); //TODO: Delete this when the Value change in a reactive way the SelectedIndex
            dcb.SelectedIndex = -1;
        }
    }
}
