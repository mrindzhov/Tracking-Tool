namespace TrackingTool.Services
{
    using System;
    using System.Linq;
    using TrackingTool.Contracts;
    using TrackingTool.Models;
    using TrackingTool.Data.Repositories;
    using TrackingTool.Data;

    public class ProcessesServices : IProcessesServices
    {
        private readonly IRepository<MyProcess> processesRepository;
        public ProcessesServices()
        {
            this.processesRepository = new GenericRepository<MyProcess>(new TrackingToolContext());
        }
        public ProcessesServices(IRepository<MyProcess> repository)
        {
            this.processesRepository = repository;
        }


        public void CreateEntry(MyProcess process)
        {
            this.processesRepository.Add(process);
            this.processesRepository.SaveChanges();
        }

        public bool HasProcess(string name)
        {
            return this.processesRepository.All().Any(p => p.Name == name);
        }

        public MyProcess GetByName(string name)
        {
            return this.processesRepository.All().FirstOrDefault(p => p.Name == name);
        }

        public IQueryable<MyProcess> GetAll()
        {
            return this.processesRepository.All().ToList().AsQueryable();
        }

        public IQueryable<MyProcess> GetTop(int count)
        {
            return this.processesRepository.All().OrderBy(p => p.Minutes).Take(count);
        }

        public MyProcess GetById(int id)
        {
            return this.processesRepository.All().FirstOrDefault(p => p.Id == id);
        }

        public void DeleteEntry(MyProcess process)
        {
            this.processesRepository.Delete(process);
            this.processesRepository.SaveChanges();
        }

        public void UpdateMinutes(MyProcess process, double minutes)
        {
            MyProcess pr = this.processesRepository.All().FirstOrDefault(p => p.Id == process.Id);
            pr.Minutes += minutes;
            this.processesRepository.SaveChanges();
        }

        public void Create(string name)
        {
            MyProcess process = new MyProcess
            {
                Name = name,
                Minutes = 0,
                StartDate = DateTime.Now
            };
            this.processesRepository.Add(process);
            this.processesRepository.SaveChanges();
        }

        public MyProcess UpdateStartDate(MyProcess process, DateTime date)
        {
            MyProcess pr = this.processesRepository.All().FirstOrDefault(p => p.Id == process.Id);
            pr.StartDate = date;
            this.processesRepository.SaveChanges();
            return pr;
        }

        public void RestartExplorer()
        {
            Utility.RestartExplorer.Run();
        }
    }
}
