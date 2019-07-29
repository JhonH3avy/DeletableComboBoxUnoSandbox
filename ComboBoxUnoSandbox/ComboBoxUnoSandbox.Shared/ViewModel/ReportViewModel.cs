using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI.Xaml.Controls;
using ComboBoxUnoSandbox.Shared.Controls;
using ComboBoxUnoSandbox.Shared.Helpers;
using ComboBoxUnoSandbox.Shared.Models;

namespace ComboBoxUnoSandbox.Shared.ViewModel
{
    public class ReportViewModel
    {
        public ReportViewModel()
        {
            Model = new ReportModel();
        }


        public ReportModel Model { get; private set; }

        public void ReportButtonClick(Button button, StackPanel paramPanel)
        {
            if (button.Tag is int)
            {
                Console.WriteLine("ReportButton Tag is integer");
                int key = (int)button.Tag;
                var paramCapture = new ReportParametersViewModel();
                DialogService.Instance.Show(new OkCancelWindow(new ReportParametersView(), paramCapture));
            }
            else
            {
                Console.WriteLine("ReportButton Tag is not an integer");
            }
        }
    }
}
