namespace TrackingTool.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamedProcessEntity : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Processes", newName: "DesktopProcesses");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.DesktopProcesses", newName: "Processes");
        }
    }
}
