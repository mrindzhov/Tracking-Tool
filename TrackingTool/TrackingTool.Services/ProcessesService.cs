namespace TrackingTool.Services
{
    using System;
    using System.Linq;
    using TrackingTool.Contracts;
    using TrackingTool.Data.Repositories;
    using TrackingTool.Data;
    using TrackingTool.Models.Entities;

    public class ProcessesService : IProcessesService
    {
        private readonly IRepository<DesktopProcess> processesRepository;

        public ProcessesService()
        {
            this.processesRepository = new GenericRepository<DesktopProcess>(new TrackingToolContext());
        }

        public ProcessesService(IRepository<DesktopProcess> repository)
        {
            this.processesRepository = repository;
        }

        public void CreateEntry(DesktopProcess process)
        {
            this.processesRepository.Add(process);
            this.processesRepository.SaveChanges();
        }

        public bool HasProcess(string name) =>
            processesRepository.All().Any(p => p.Name == name);

        public DesktopProcess GetByName(string name) =>
            this.processesRepository.All().FirstOrDefault(p => p.Name == name);

        public IQueryable<DesktopProcess> GetAll() =>
            this.processesRepository.All().AsQueryable();

        public IQueryable<DesktopProcess> GetTop(int count) =>
            this.processesRepository.All().OrderBy(p => p.Minutes).Take(count);

        public DesktopProcess GetById(int id) =>
            this.processesRepository.All().FirstOrDefault(p => p.Id == id);

        public void DeleteEntry(DesktopProcess process)
        {
            this.processesRepository.Delete(process);
            this.processesRepository.SaveChanges();
        }

        public void UpdateMinutes(DesktopProcess process, double minutes)
        {
            DesktopProcess pr = this.processesRepository.All().FirstOrDefault(p => p.Id == process.Id);
            pr.Minutes += minutes;
            this.processesRepository.SaveChanges();
        }

        public void Create(string name)
        {
            DesktopProcess process = new DesktopProcess
            {
                Name = name,
                Minutes = 0,
                StartDate = DateTime.Now
            };
            this.processesRepository.Add(process);
            this.processesRepository.SaveChanges();
        }

        public DesktopProcess UpdateStartDate(DesktopProcess process, DateTime date)
        {
            DesktopProcess pr = this.processesRepository.All().FirstOrDefault(p => p.Id == process.Id);
            pr.StartDate = date;
            this.processesRepository.SaveChanges();
            return pr;
        }

        public void RestartExplorer() => RestartExplorerService.Run();
    }
}
