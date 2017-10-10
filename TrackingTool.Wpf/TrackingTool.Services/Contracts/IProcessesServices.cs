namespace TrackingTool.Services.Contracts
{
    using System.Linq;
    using TrackingTool.Models;

    interface IProcessesServices
    {
        IQueryable<MyProcess> GetAll();
        IQueryable<MyProcess> GetTop(int count);
        MyProcess GetById(int id);
        MyProcess GetByName(string name);
        void DeleteEntry(MyProcess process);
        void UpdateMinutes(MyProcess process, double minutes);
        void CreateEntry(MyProcess process);
        void Create(string name);
        bool HasProcess(string name);
    }
}