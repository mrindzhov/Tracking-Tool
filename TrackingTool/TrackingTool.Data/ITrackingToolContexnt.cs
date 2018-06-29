namespace TrackingTool.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using TrackingTool.Models.Entities;

    public interface ITrackingToolContext : IDisposable
    {
        int SaveChanges();
        IDbSet<DesktopProcess> DesktopProcesses { get; set; }
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}