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
using Microsoft.Toolkit.Uwp.UI.Converters;
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
        //private static BoolToVisibilityConverter boolConverter;
        //private static Style textBlockStyle;

        //static MainPage()
        //{
        //    boolConverter = new BoolToVisibilityConverter();
        //    textBlockStyle = new Style();
        //    textBlockStyle.TargetType = typeof(TextBlock);
        //    textBlockStyle.Setters.Add(new Setter
        //    {
        //        Property = TextBlock.TextAlignmentProperty,
        //        Value = TextAlignment.Right
        //    });
        //    textBlockStyle.Setters.Add(new Setter
        //    {
        //        Property = TextBlock.VerticalAlignmentProperty,
        //        Value = VerticalAlignment.Center
        //    });
        //    textBlockStyle.Setters.Add(new Setter
        //    {
        //        Property = TextBlock.MarginProperty,
        //        Value = "0,0,6,0"
        //    });
        //}

        public MainPage()
        {
            this.InitializeComponent();
            DataContext = this;
            //Resources["CNVBoolToVisibility"] = boolConverter;
            //Resources["fieldLabel"] = textBlockStyle;
            ComboDelBox.Items.Add("Item1");
            ComboDelBox.Items.Add("Item2");
        }

        private void XDropDownGlyph_OnClick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("X dropdown");
        }

        private void ComboBox_OnDropDownOpened(object sender, object e)
        {
            Console.WriteLine("ComboBox_OnDropDownOpened");
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
    }
}
