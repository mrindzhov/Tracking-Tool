namespace TrackingTool.Wpf.Domain
{
    using AutoMapper;
    using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using TrackingTool.Data;
    using TrackingTool.Data.Repositories;
    using TrackingTool.Models;
    using TrackingTool.Models.ViewModels;
    using TrackingTool.Services;

    public class ProcessesViewModel : INotifyPropertyChanged
    {
        /// <summary>
        public MyProcess CurrentProcess { get; set; }
        private ActiveWindow activeWindow = new ActiveWindow();
        /// <summary>

        private readonly ObservableCollection<ProcessViewModel> _items;
        private readonly ProcessesServices processesServices;
        public ObservableCollection<ProcessViewModel> Items => _items;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ProcessesViewModel()
        {
            Mapper.Initialize(cfg => { cfg.CreateMap<MyProcess, ProcessViewModel>(); });
            this.processesServices = new ProcessesServices(new GenericRepository<MyProcess>(new TrackingToolContext()));
            _items = GetData();
            AttachEvents();
        }

        private ObservableCollection<ProcessViewModel> GetData()
        {
            List<MyProcess> list = this.processesServices.GetAll().ToList();
            List<ProcessViewModel> processes = Mapper.Map<List<MyProcess>, List<ProcessViewModel>>(list);
            return new ObservableCollection<ProcessViewModel>(processes);
        }

        private void AttachEvents()
        {
            Application.Current.MainWindow.Closing += new CancelEventHandler(MainWindow_Closing);

            activeWindow.OnActiveWindowChanged += new ActiveWindow.ActiveWindowChangedHandler(AppChangeHandler);
            activeWindow.OnWindowRestored += new ActiveWindow.ActiveWindowChangedHandler(AppChangeHandler);
            activeWindow.OnWindowMinimized += new ActiveWindow.ActiveWindowChangedHandler(AppChangeHandler);
            activeWindow.Start();
            SystemEvents.PowerModeChanged += new PowerModeChangedEventHandler(PowerModeChangeHandler);
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            activeWindow = null;

            if (this.CurrentProcess != null)
            {
                this.UpdateMinutes();
            }
        }

        private void PowerModeChangeHandler(object sender, PowerModeChangedEventArgs e)
        {
            if (e.Mode == PowerModes.Suspend)
            {
                UpdateCurrentProcess();
            }
        }

        private void AppChangeHandler(object sender, string windowHeader, IntPtr hwnd)
        {
            UpdateCurrentProcess();
        }

        private void UpdateCurrentProcess()
        {
            if (this.CurrentProcess != null)
            {
                UpdateMinutes();
            }
            string window = Utilities.GetActiveWindowTitle();
            if (window != null) // some bug occures sometimes
            {
                DefineApplication(window);
            }
        }

        private void DefineApplication(string name)
        {

            if (!this.processesServices.HasProcess(name))
            {
                MyProcess process = new MyProcess
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

                Application.Current.Dispatcher.Invoke((Action)(() => this.Items.Add(processViewModel)));
            }
        }

        private ProcessViewModel MapFrom(MyProcess process)
        {
            return Mapper.Map<MyProcess, ProcessViewModel>(process);
        }

        private void UpdateMinutes()
        {
            TimeSpan minutes = (DateTime.Now - this.CurrentProcess.StartDate);
            this.processesServices.UpdateMinutes(this.CurrentProcess, minutes.TotalMinutes);
            this.CurrentProcess = null;
        }
    }
}