namespace TrackingTool.Wpf
{
    using MaterialDesignThemes.Wpf;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using TrackingTool.Wpf.Domain;
    using System.Windows.Controls.Primitives;
    using TrackingTool.Data;
    using System.ComponentModel;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Snackbar Snackbar;

        public MainWindow()
        {
            Initializer.InitDb();
            InitializeComponent();
            OpenGreetingPopUp();
            DataContext = new MainWindowViewModel(MainSnackbar.MessageQueue);
        }
        private void CommandBinding_CanExecute_1(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBinding_Executed_1(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        private void CommandBinding_Executed_3(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void OpenGreetingPopUp()
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(2500);
            }).ContinueWith(t =>
            {
                //note you can use the message queue from any thread, but just for the demo here we 
                //need to get the message queue from the snackbar, so need to be on the dispatcher
                MainSnackbar.MessageQueue.Enqueue("Welcome!");
            }, TaskScheduler.FromCurrentSynchronizationContext());

            Snackbar = this.MainSnackbar;
        }

        private void UIElement_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //until we had a StaysOpen glag to Drawer, this will help with scroll bars
            var dependencyObject = Mouse.Captured as DependencyObject;
            while (dependencyObject != null)
            {
                if (dependencyObject is ScrollBar) return;
                dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            }

            MenuToggleButton.IsChecked = false;
        }

        //private async void MenuPopupButton_OnClick(object sender, RoutedEventArgs e)
        //{
        //    var sampleMessageDialog = new SampleMessageDialog
        //    {
        //        Message = { Text = ((ButtonBase)sender).Content.ToString() }
        //    };

        //    await DialogHost.Show(sampleMessageDialog, "RootDialog");
        //}

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
                this.Hide();

            base.OnStateChanged(e);
        }


        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void MyNotifyIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            this.Show();
            this.WindowState = WindowState.Normal;
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            this.Hide();
            Task.WaitAll();
            base.OnClosing(e);
        }
    }
}
