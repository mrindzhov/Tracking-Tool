namespace TrackingTool.Data
{
    public static class Initializer
    {
        public static void InitDb()
        {
            new TrackingToolContext().Database.Initialize(true);
        }
    }
}
