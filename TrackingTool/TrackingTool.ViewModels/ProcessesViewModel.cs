namespace TrackingTool.ViewModels
{
    using AutoMapper;
    using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using TrackingTool.Data;
    using TrackingTool.Data.Repositories;
    using TrackingTool.Models.Entities;
    using TrackingTool.Models.Domain;
    using TrackingTool.Services;

    public class ProcessesViewModel : INotifyPropertyChanged
    {
        private readonly ProcessesService processesServices;

        public DesktopProcess CurrentProcess { get; set; }
        public ObservableCollection<ProcessViewModel> Items { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        private ActiveWindow activeWindow = new ActiveWindow();

        public ProcessesViewModel()
        {
            Mapper.Initialize(cfg => { cfg.CreateMap<DesktopProcess, ProcessViewModel>(); });
            this.processesServices = new ProcessesService(new GenericRepository<DesktopProcess>(new TrackingToolContext()));
            Items = GetData();
            AttachEvents();
        }

        private ObservableCollection<ProcessViewModel> GetData()
        {
            List<DesktopProcess> list = this.processesServices.GetAll().ToList();
            List<ProcessViewModel> processes = Mapper.Map<List<DesktopProcess>, List<ProcessViewModel>>(list);
            return new ObservableCollection<ProcessViewModel>(processes);
        }

        private void AttachEvents()
        {
            Application.Current.MainWindow.Closing += new CancelEventHandler(MainWindow_Closing);

            AttachEventsToActiveWindow();

            SystemEvents.PowerModeChanged += new PowerModeChangedEventHandler(PowerModeChangeHandler);
        }

        private void AttachEventsToActiveWindow()
        {
            activeWindow.OnActiveWindowChanged += new ActiveWindow.ActiveWindowChangedHandler(AppChangeHandler);
            activeWindow.OnWindowRestored += new ActiveWindow.ActiveWindowChangedHandler(AppChangeHandler);
            activeWindow.OnWindowMinimized += new ActiveWindow.ActiveWindowChangedHandler(AppChangeHandler);
            activeWindow.Start();
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            activeWindow = null;
            if (CurrentProcess != null)
                UpdateMinutes();
        }

        private void PowerModeChangeHandler(object sender, PowerModeChangedEventArgs e)
        {
            if (e.Mode == PowerModes.Suspend)
                UpdateCurrentProcess();
        }

        private void AppChangeHandler(object sender, string windowHeader, IntPtr hwnd) =>
            UpdateCurrentProcess();

        private void UpdateCurrentProcess()
        {
            if (CurrentProcess != null)
                UpdateMinutes();

            string window = WindowService.GetActiveWindowTitle();
            if (window != null) // some bug occures sometimes
                DefineApplication(window);
        }

        private void DefineApplication(string name)
        {
            if (!this.processesServices.HasProcess(name))
            {
                DesktopProcess process = new DesktopProcess
                {
                    Name = name,
                    StartDate = DateTime.Now,
                    Minutes = 0
                };
                this.processesServices.CreateEntry(process);
                this.CurrentProcess = process;
                ProcessViewModel processViewModel = MapFrom(this.CurrentProcess);

                Items.Add(processViewModel);
            }
            else
            {
                this.CurrentProcess = this.processesServices.GetByName(name);
                ProcessViewModel processViewModel = this.Items.FirstOrDefault(i => i.Id == this.CurrentProcess.Id);
                Application.Current.Dispatcher.Invoke((Action)(() => this.Items.Remove(processViewModel)));

                this.CurrentProcess = this.processesServices.UpdateStartDate(this.CurrentProcess, DateTime.Now);
                processViewModel = MapFrom(this.CurrentProcess);

                Application.Current.Dispatcher.Invoke(() => this.Items.Add(processViewModel));
            }
        }

        private ProcessViewModel MapFrom(DesktopProcess process) =>
            Mapper.Map<DesktopProcess, ProcessViewModel>(process);

        private void UpdateMinutes()
        {
            TimeSpan minutes = (DateTime.Now - this.CurrentProcess.StartDate);
            this.processesServices.UpdateMinutes(this.CurrentProcess, minutes.TotalMinutes);
            this.CurrentProcess = null;
        }
    }
}