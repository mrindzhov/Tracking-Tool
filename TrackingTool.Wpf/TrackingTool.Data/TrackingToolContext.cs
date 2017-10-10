namespace TrackingTool.Data
{
    using System.Data.Entity;
    using TrackingTool.Models;

    public class TrackingToolContext : DbContext, ITrackingToolContext
    {
        public TrackingToolContext()
            : base("name=TrackingToolContext")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<TrackingToolContext>());
        }


        public virtual IDbSet<MyProcess> Processes { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<MyProcess>().ToTable("Processes");
        }
    }
    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}