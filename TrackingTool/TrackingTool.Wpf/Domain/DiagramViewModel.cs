namespace TrackingTool.Wpf.Domain
{
    using AutoMapper;
    using LiveCharts;
    using LiveCharts.Wpf;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TrackingTool.Data;
    using TrackingTool.Data.Repositories;
    using TrackingTool.Models;
    using TrackingTool.Models.ViewModels;
    using TrackingTool.Services;

    public class DiagramViewModel
    {
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }


        private readonly ProcessesServices processesServices;


        public DiagramViewModel()
        {
            this.processesServices = new ProcessesServices(new GenericRepository<MyProcess>(new TrackingToolContext()));
            FillSeriesCollection();
        }

        private List<ProcessViewModel> GetData()
        {
            List<MyProcess> list = this.processesServices.GetAll().ToList();
            List<ProcessViewModel> processes = Mapper.Map<List<MyProcess>, List<ProcessViewModel>>(list);
            return processes;
        }

        private void FillSeriesCollection()
        {
            List<ProcessViewModel> processes = GetData();
            IEnumerable<double> values = processes.OrderByDescending(p => p.Minutes).Take(5).Select(p => p.Minutes);
            IEnumerable<string> labels = processes.OrderByDescending(p => p.Minutes).Take(5).Select(p => p.Name);
            ColumnSeries columnSeries = new ColumnSeries
            {
                Title = "All time",
                Values = new ChartValues<double>(values),
                MaxWidth = 10
            };
            SeriesCollection = new SeriesCollection();
            SeriesCollection.Add(columnSeries);
            Labels = labels.ToArray();
            Formatter = value => TimeSpan.FromMinutes(value).ToString();
        }
    }
}
