namespace TrackingTool.Services
{
    using System;
    using System.Linq;
    using TrackingTool.Services.Contracts;
    using TrackingTool.Models;

    public class ProcessesServices : IProcessesServices
    {
        //public void CreateEntry(MyProcess process)
        //{
        //    using (var ctx = new TrackingToolContext())
        //    {
        //        ctx.Processes.Add(process);
        //        ctx.SaveChanges();
        //    }
        //}

        //public bool HasProcess(string name)
        //{
        //    using (var ctx = new TrackingToolContext())
        //    {
        //        return ctx.Processes.Any(p => p.Name == name);
        //    }
        //}

        //public MyProcess GetByName(string name)
        //{
        //    using (var ctx = new TrackingToolContext())
        //    {
        //        return ctx.Processes.FirstOrDefault(p => p.Name == name);
        //    }
        //}

        //public IQueryable<MyProcess> GetAll()
        //{
        //    using (var ctx = new TrackingToolContext())
        //    {
        //        return ctx.Processes.ToList().AsQueryable();
        //    }
        //}

        //public IQueryable<MyProcess> GetTop(int count)
        //{
        //    using (var ctx = new TrackingToolContext())
        //    {
        //        return ctx.Processes.OrderBy(p => p.Minutes).Take(count);
        //    }
        //}

        //public MyProcess GetById(int id)
        //{
        //    using (var ctx = new TrackingToolContext())
        //    {
        //        return ctx.Processes.FirstOrDefault(p => p.Id == id);
        //    }
        //}

        //public void DeleteEntry(MyProcess process)
        //{
        //    using (var ctx = new TrackingToolContext())
        //    {
        //        ctx.Processes.Remove(process);
        //        ctx.SaveChanges();
        //    }
        //}

        //public void UpdateMinutes(MyProcess process, double minutes)
        //{
        //    using (var ctx = new TrackingToolContext())
        //    {
        //        MyProcess pr = ctx.Processes.FirstOrDefault(p => p.Id == process.Id);
        //        pr.Minutes += minutes;
        //        ctx.SaveChanges();
        //    }
        //}

        //public void Create(string name)
        //{
        //    using (var ctx = new TrackingToolContext())
        //    {
        //        MyProcess process = new MyProcess
        //        {
        //            Name = name,
        //            Minutes = 0,
        //            StartDate = DateTime.Now
        //        };
        //        ctx.Processes.Add(process);
        //        ctx.SaveChanges();
        //    }
        //}
        public void Create(string name)
        {
            throw new NotImplementedException();
        }

        public void CreateEntry(MyProcess process)
        {
            throw new NotImplementedException();
        }

        public void DeleteEntry(MyProcess process)
        {
            throw new NotImplementedException();
        }

        public IQueryable<MyProcess> GetAll()
        {
            throw new NotImplementedException();
        }

        public MyProcess GetById(int id)
        {
            throw new NotImplementedException();
        }

        public MyProcess GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public IQueryable<MyProcess> GetTop(int count)
        {
            throw new NotImplementedException();
        }

        public bool HasProcess(string name)
        {
            throw new NotImplementedException();
        }

        public void UpdateMinutes(MyProcess process, double minutes)
        {
            throw new NotImplementedException();
        }
    }
}
