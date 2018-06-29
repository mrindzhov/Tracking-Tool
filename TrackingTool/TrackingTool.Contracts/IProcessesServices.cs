namespace TrackingTool.Contracts
{
    using System;
    using System.Linq;
    using TrackingTool.Models.Entities;

    public interface IProcessesServices
    {
        IQueryable<DesktopProcess> GetAll();
        IQueryable<DesktopProcess> GetTop(int count);
        DesktopProcess GetById(int id);
        DesktopProcess GetByName(string name);
        void DeleteEntry(DesktopProcess process);
        DesktopProcess UpdateStartDate(DesktopProcess process, DateTime date);
        void UpdateMinutes(DesktopProcess process, double minutes);
        void CreateEntry(DesktopProcess process);
        void Create(string name);
        bool HasProcess(string name);
        void RestartExplorer();
    }
}