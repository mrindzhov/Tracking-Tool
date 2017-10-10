namespace TrackingTool.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using TrackingTool.Models;


    public interface ITrackingToolContext : IDisposable
    {
        int SaveChanges();
        IDbSet<MyProcess> Processes { get; set; }
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}