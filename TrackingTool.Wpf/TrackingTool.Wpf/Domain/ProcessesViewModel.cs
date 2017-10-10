namespace TrackingTool.Wpf.Domain
{
    using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using TrackingTool.Models.ViewModels;
    using TrackingTool.Services;

    public class ProcessesViewModel : INotifyPropertyChanged
    {
        /// <summary>
        public Dictionary<string, ProcessViewModel> data = new Dictionary<string, ProcessViewModel>();
        public ProcessViewModel CurrentProcess { get; set; }
        private int counter;
        private ActiveWindow activeWindow = new ActiveWindow();
        /// <summary>

        private readonly ObservableCollection<ProcessViewModel> _items;
        private readonly ProcessesServices processesServices;

        public ProcessesViewModel()
        {
            this.processesServices = new ProcessesServices();
            counter = 0;
            _items = new ObservableCollection<ProcessViewModel>();
            AttachEvents();
        }

        private void OnPowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            if (e.Mode == PowerModes.Suspend)
            {
                UpdateCurrentProcess();
            }
        }

        public ObservableCollection<ProcessViewModel> Items => _items;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AttachEvents()
        {
            activeWindow.OnActiveWindowChanged += new ActiveWindow.ActiveWindowChangedHandler(AppChangeHandler);
            activeWindow.OnWindowRestored += new ActiveWindow.ActiveWindowChangedHandler(AppChangeHandler);
            activeWindow.OnWindowMinimized += new ActiveWindow.ActiveWindowChangedHandler(AppChangeHandler);
            activeWindow.Start();
            SystemEvents.PowerModeChanged += new PowerModeChangedEventHandler(OnPowerModeChanged);
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
            //if (this.processesServices.HasProcess(name))
            //{
            //    Console.WriteLine("asd");
            //}
            if (!this.data.ContainsKey(name))
            {
                ProcessViewModel process = new ProcessViewModel
                {
                    Id = counter++,
                    Name = name,
                    StartDate = DateTime.Now,
                    Minutes = 0
                };
                //ProcessesServices.CreateEntry(process);
                data.Add(name, process);
                this.CurrentProcess = process;
                Items.Add(this.CurrentProcess);
            }
            else
            {
                //this.CurrentProcess = ProcessesServices.GetByName(name);
                this.CurrentProcess = data[name];
                Application.Current.Dispatcher.Invoke((Action)(() => this.Items.Remove(this.CurrentProcess)));
                this.CurrentProcess.StartDate = DateTime.Now;
                Application.Current.Dispatcher.Invoke((Action)(() => this.Items.Add(this.CurrentProcess)));
            }
        }

        private void UpdateMinutes()
        {
            TimeSpan minutes = (DateTime.Now - this.CurrentProcess.StartDate);
            //ProcessesServices.UpdateEntry(CurrentProcess, minutes.TotalMinutes);
            this.CurrentProcess.Minutes += minutes.TotalMinutes;
            this.CurrentProcess = null;
        }
    }
}