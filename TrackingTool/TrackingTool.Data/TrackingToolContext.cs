namespace TrackingTool.Data
{
    using System.Data.Entity;
    using TrackingTool.Models.Entities;

    public class TrackingToolContext : DbContext, ITrackingToolContext
    {
        public TrackingToolContext()
            : base("name=TrackingToolContext")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<TrackingToolContext>());
        }

        public virtual IDbSet<DesktopProcess> DesktopProcesses { get; set; }
    }
}