namespace TrackingTool.Views
{
    using System.Windows;
    using System.Windows.Controls;
    using TrackingTool.Services;

    /// <summary>
    /// Interaction logic for TaskbarFix.xaml
    /// </summary>
    public partial class TaskbarFix : UserControl
    {
        private readonly ProcessesServices processesServices;
        public TaskbarFix()
        {
            InitializeComponent();
            this.processesServices = new ProcessesServices();
        }

        private void RestartExplorer_Click(object sender, RoutedEventArgs e)
        {
            this.processesServices.RestartExplorer();
        }
    }
}
