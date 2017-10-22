namespace TrackingTool.Services
{
    using System;
    using System.Linq;
    using TrackingTool.Services.Contracts;
    using TrackingTool.Models;
    using TrackingTool.Data.Repositories;
    using TrackingTool.Data;

    public class ProcessesServices : IProcessesServices
    {
        private IRepository<MyProcess> Processes;
        public ProcessesServices()
        {
            this.Processes = new GenericRepository<MyProcess>(new TrackingToolContext());
        }
        public ProcessesServices(IRepository<MyProcess> repository)
        {
            this.Processes = repository;
        }


        public void CreateEntry(MyProcess process)
        {
            this.Processes.Add(process);
            this.Processes.SaveChanges();
        }

        public bool HasProcess(string name)
        {
            return this.Processes.All().Any(p => p.Name == name);
        }

        public MyProcess GetByName(string name)
        {
            return this.Processes.All().FirstOrDefault(p => p.Name == name);
        }

        public IQueryable<MyProcess> GetAll()
        {
            return this.Processes.All().ToList().AsQueryable();
        }

        public IQueryable<MyProcess> GetTop(int count)
        {
            return this.Processes.All().OrderBy(p => p.Minutes).Take(count);
        }

        public MyProcess GetById(int id)
        {
            return this.Processes.All().FirstOrDefault(p => p.Id == id);
        }

        public void DeleteEntry(MyProcess process)
        {
            this.Processes.Delete(process);
            this.Processes.SaveChanges();
        }

        public void UpdateMinutes(MyProcess process, double minutes)
        {
            MyProcess pr = this.Processes.All().FirstOrDefault(p => p.Id == process.Id);
            pr.Minutes += minutes;
            this.Processes.SaveChanges();
        }

        public void Create(string name)
        {
            MyProcess process = new MyProcess
            {
                Name = name,
                Minutes = 0,
                StartDate = DateTime.Now
            };
            this.Processes.Add(process);
            this.Processes.SaveChanges();
        }

        public MyProcess UpdateStartDate(MyProcess process, DateTime date)
        {
            MyProcess pr = this.Processes.All().FirstOrDefault(p => p.Id == process.Id);
            pr.StartDate = date;
            this.Processes.SaveChanges();
            return pr;
        }

        public void RestartExplorer()
        {
            Utility.RestartExplorer.Run();
        }
    }
}
