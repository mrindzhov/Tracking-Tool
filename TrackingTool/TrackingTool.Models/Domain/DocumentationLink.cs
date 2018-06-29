namespace TrackingTool.Models.Domain
{
    using System.Windows.Forms;
    using System.Windows.Input;
    using TrackingTool.Enums;

    public class DocumentationLink
    {
        private static readonly string baseUrl = "githubUrl";

        public string Label { get; }
        public string Url { get; }
        public DocumentationLinkType Type { get; }
        public ICommand Open { get; }

        public DocumentationLink(DocumentationLinkType type, string url) : this(type, url, null)
        {
        }

        public DocumentationLink(DocumentationLinkType type, string url, string label)
        {
            Label = label ?? type.ToString();
            Url = url;
            Type = type;
            Open = new AnotherCommandImplementation(Execute);
        }

        private void Execute(object o) => System.Diagnostics.Process.Start(Url);

        public static DocumentationLink DemoPageLink<TDemoPage>() => DemoPageLink<TDemoPage>(null);

        public static DocumentationLink DemoPageLink<TDemoPage>(string label) => DemoPageLink<TDemoPage>(label, null);

        public static DocumentationLink WikiLink(string page, string label) =>
            new DocumentationLink(DocumentationLinkType.Wiki, $"{baseUrl}/wiki/" + page, label);

        public static DocumentationLink StyleLink(string nameChunk) =>
            new DocumentationLink(
                    DocumentationLinkType.StyleSource,
                    $"{baseUrl}/blob/master/MaterialDesignThemes.Wpf/Themes/MaterialDesignTheme.{nameChunk}.xaml",
                    nameChunk);

        public static DocumentationLink ApiLink<TClass>(string subNamespace)
        {
            var typeName = typeof(TClass).Name;

            return new DocumentationLink(
                DocumentationLinkType.ControlSource,
                $"{baseUrl}/blob/master/MaterialDesignThemes.Wpf/{subNamespace}/{typeName}.cs",
                typeName);
        }


        public static DocumentationLink ApiLink<TClass>()
        {
            var typeName = typeof(TClass).Name;

            return new DocumentationLink(
                DocumentationLinkType.ControlSource,
                $"{baseUrl}/blob/master/MaterialDesignThemes.Wpf/{typeName}.cs",
                typeName);
        }

        public static DocumentationLink DemoPageLink<TDemoPage>(string label, string nameSpace)
        {
            var ext = typeof(UserControl).IsAssignableFrom(typeof(TDemoPage)) ? "xaml" : "cs";
            string vnameSpace = string.IsNullOrWhiteSpace(nameSpace) ? "" : $"/{nameSpace}/";

            return new DocumentationLink(
                DocumentationLinkType.DemoPageSource,
                $"{baseUrl}/blob/master/MainDemo.Wpf/{vnameSpace}{typeof(TDemoPage).Name}.{ext}",
                label ?? typeof(TDemoPage).Name);
        }
    }
}