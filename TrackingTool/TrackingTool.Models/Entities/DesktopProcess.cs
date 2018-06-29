namespace TrackingTool.Models.Entities
{
    using System;

    public class DesktopProcess
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public double Minutes { get; set; }
    }
}