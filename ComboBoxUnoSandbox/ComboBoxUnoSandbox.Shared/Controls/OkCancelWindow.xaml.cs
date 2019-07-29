using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using ComboBoxUnoSandbox.Shared.Helpers;
using ComboBoxUnoSandbox.Shared.ViewModel;


namespace ComboBoxUnoSandbox.Shared.Controls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class OkCancelWindow
    {
        ScrollViewer windowMessage;
        bool firstTime = true;
        /// <summary>
        /// 
        /// </summary>
        public OkCancelWindow()
        {
            InitializeComponent();
            CancelCommand = new HseDelegateCommand(o => Close());
            GotFocus += OkCancelWindowGotFocus;
        }

        //TODO: this class needs a rethink now that its a ContentDialog
        public void Close()
        {
            DialogService.Instance.CloseDialog();
        }

        // attempt to get a simpler interface
        public OkCancelWindow(object content, IOKCancelViewModel viewModel)
        {
            // copied from default ctor due to https://github.com/nventive/Uno/issues/61
            InitializeComponent();
            CancelCommand = new HseDelegateCommand(o => Close());
            GotFocus += OkCancelWindowGotFocus;
            Debug.WriteLine("okcancel ctor");

            OkCommand = new HseDelegateCommand(_ =>
            {
                Debug.WriteLine("ok command");
                if (viewModel.OK(this))
                {
                    Close();
                    viewModel.Dispose();
                }
            },
                                                viewModel.OkCanExecute);
            CancelCommand = new HseDelegateCommand(_ =>
            {
                if (viewModel.Cancel())
                {
                    Close();
                    viewModel.Dispose();
                }
            },
                                                viewModel.CancelCanExecute);
            viewModel.OkCommand = OkCommand;
            viewModel.CancelCommand = CancelCommand;
            WindowMessage = content;
            SetBinding(OkTextProperty, new Binding { Path = new PropertyPath("OkText") });
            SetBinding(CancelTextProperty, new Binding { Path = new PropertyPath("CancelText") });
            DataContext = viewModel;
            viewModel.View = this;
        }

        void OkCancelWindowGotFocus(object sender, RoutedEventArgs e)
        {
            if (firstTime)
            {
                try
                {
                    var hasInitialFocus = WindowMessage as IHasInitialFocus;
                    if (hasInitialFocus != null)
                    {
                        hasInitialFocus.SetInitialFocus();
                    }
                }
                // ReSharper disable EmptyGeneralCatchClause
                catch
                // ReSharper restore EmptyGeneralCatchClause
                {
                    // Swallow exception; not worth bothering user if focus isn't set correctly
                }
                finally
                {
                    firstTime = false;
                }
            }
        }

        public object WindowMessage
        {
            get { return GetValue(WindowMessageProperty); }
            set { SetValue(WindowMessageProperty, value); }
        }

        public static readonly DependencyProperty WindowMessageProperty =
            DependencyProperty.Register(
                "WindowMessage", typeof(object), typeof(OkCancelWindow),
                new PropertyMetadata(null, WindowMessageChanged));

        static void WindowMessageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as OkCancelWindow;
            if (window == null) return;
            if (e.OldValue != null)
            {
                if (e.OldValue is UIElement) window.grdContainer.Children.Remove(e.OldValue as UIElement);
                if (window.windowMessage != null) window.grdContainer.Children.Remove(window.windowMessage);
            }

            if (e.NewValue is string)
            {
                window.windowMessage = new ScrollViewer
                {
                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                    Content = new TextBlock
                    {
                        Text = e.NewValue.ToString(),
                        TextWrapping = TextWrapping.Wrap
                    }
                };
                window.windowMessage.SetValue(Grid.RowProperty, 1);
                window.grdContainer.Children.Add(window.windowMessage);
                return;
            }
            var uiElement = e.NewValue as UIElement;
            if (uiElement != null)
            {
                uiElement.SetValue(Grid.RowProperty, 1);
                window.grdContainer.Children.Add(uiElement);
                //				var hasInitialFocus = uiElement as IHasInitialFocus;
                //				if(hasInitialFocus != null)
                //				{
                //					hasInitialFocus.SetInitialFocus();
                //				}
                return;
            }
            throw new Exception("Window message must either be of type UIElemnet or string");
        }

        public Grid GrdContainer { get { return grdContainer; } }
        public Button OkButton { get { return null /*okButton*/; } }

        public string OkText
        {
            get { return (string)GetValue(OkTextProperty); }
            set { SetValue(OkTextProperty, value); }
        }

        public static readonly DependencyProperty OkTextProperty =
            DependencyProperty.Register(
                "OkText", typeof(string), typeof(OkCancelWindow),
                new PropertyMetadata("Ok"));


        public string CancelText
        {
            get { return (string)GetValue(CancelTextProperty); }
            set { SetValue(CancelTextProperty, value); }
        }

        public static readonly DependencyProperty CancelTextProperty =
            DependencyProperty.Register(
                "CancelText", typeof(string), typeof(OkCancelWindow),
                new PropertyMetadata("Cancel"));


        public IInvalidateCommand OkCommand
        {
            get { return (IInvalidateCommand)GetValue(OkCommandProperty); }
            set { SetValue(OkCommandProperty, value); }
        }

        public static readonly DependencyProperty OkCommandProperty =
            DependencyProperty.Register(
                "OkCommand", typeof(IInvalidateCommand), typeof(OkCancelWindow),
                null);

        public Visibility OkVisibility
        {
            get { return (Visibility)GetValue(OkVisibilityProperty); }
            set { SetValue(OkVisibilityProperty, value); }
        }

        public static readonly DependencyProperty OkVisibilityProperty =
            DependencyProperty.Register("OkVisibility", typeof(Visibility), typeof(OkCancelWindow), new PropertyMetadata(Visibility.Visible));


        public IInvalidateCommand CancelCommand
        {
            get { return (IInvalidateCommand)GetValue(CancelCommandProperty); }
            set { SetValue(CancelCommandProperty, value); }
        }

        public Visibility CancelVisibility
        {
            get { return (Visibility)GetValue(CancelVisibilityProperty); }
            set { SetValue(CancelVisibilityProperty, value); }
        }

        public static readonly DependencyProperty CancelVisibilityProperty =
            DependencyProperty.Register("CancelVisibility", typeof(Visibility), typeof(OkCancelWindow), new PropertyMetadata(Visibility.Visible));

        public static readonly DependencyProperty CancelCommandProperty =
            DependencyProperty.Register("CancelCommand", typeof(IInvalidateCommand), typeof(OkCancelWindow), null);

        public static void Show(string message)
        {
            new OkCancelWindow(message, new OkViewModel()) { CancelVisibility = Visibility.Collapsed }.ShowAsync();
        }

        public void ShowAsync()
        {

        }
    }
}
