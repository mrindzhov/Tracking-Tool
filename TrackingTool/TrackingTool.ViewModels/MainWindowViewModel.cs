namespace TrackingTool.ViewModels
{
    using MaterialDesignThemes.Wpf;
    using System;
    using TrackingTool.Models.Domain;

    public class MainWindowViewModel
    {
        public MainWindowViewModel(ISnackbarMessageQueue snackbarMessageQueue)
        {
            if (snackbarMessageQueue == null) throw new ArgumentNullException(nameof(snackbarMessageQueue));
        }

        public NavigationItem[] DemoItems { get; set; }
    }
}