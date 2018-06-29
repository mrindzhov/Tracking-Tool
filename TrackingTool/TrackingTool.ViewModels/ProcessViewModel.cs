namespace TrackingTool.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class ProcessViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public int Id { get; set; }

        private string name;

        private DateTime startDate;

        private double minutes;

        private TimeSpan totalTime;

        public TimeSpan TotalTime
        {
            get => this.totalTime;

            set
            {
                if (this.totalTime == value) return;
                this.totalTime = value;
                OnPropertyChanged();
            }
        }

        public double Minutes
        {
            get => this.minutes;

            set
            {
                if (this.minutes == value) return;
                this.minutes = value;
                this.TotalTime = TimeSpan.FromMinutes(this.minutes);
                OnPropertyChanged();
            }
        }

        public DateTime StartDate
        {
            get => this.startDate;
            set
            {
                if (this.startDate == value) return;
                this.startDate = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => this.name;
            set
            {
                if (this.name == value) return;
                this.name = value;

                OnPropertyChanged();
            }
        }
    }
}