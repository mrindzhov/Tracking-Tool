namespace TrackingTool.Data
{
    public class Initializer
    {
        public static void InitDb()
        {
            new TrackingToolContext().Database.Initialize(true);
        }
    }
}
