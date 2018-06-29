namespace TrackingTool.Views.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using TrackingTool.Services;

    /// <summary>
    /// Interaction logic for TaskbarFix.xaml
    /// </summary>
    public partial class TaskbarFix : UserControl
    {
        private readonly ProcessesService processesServices;
        public TaskbarFix()
        {
            InitializeComponent();
            this.processesServices = new ProcessesService();
        }

        private void RestartExplorer_Click(object sender, RoutedEventArgs e)
        {
            this.processesServices.RestartExplorer();
        }
    }
}
