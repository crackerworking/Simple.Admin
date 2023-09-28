namespace Mi.Domain.Shared.Models.UI
{
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
    public class PaConfigModel
    {
        public PaConfigLogo logo { get; set; }
        public PaConfigMenu menu { get; set; }
        public PaConfigTab tab { get; set; }
        public PaConfigTheme theme { get; set; }
        public List<PaConfigColor> colors { get; set; }
        public PaConfigOther other { get; set; }
        public PaConfigHeader header { get; set; }
    }
    public class PaConfigColor
    {
        public string id { get; set; }
        public string color { get; set; }
        public string second { get; set; }
    }

    public class PaConfigHeader
    {
        public string message { get; set; }
    }

    public class Index
    {
        public string id { get; set; }
        public string href { get; set; }
        public string title { get; set; }
    }

    public class PaConfigLogo
    {
        public string title { get; set; }
        public string image { get; set; }
    }

    public class PaConfigMenu
    {
        public string data { get; set; }
        public string method { get; set; }
        public bool accordion { get; set; }
        public bool collapse { get; set; }
        public bool control { get; set; }
        public int controlWidth { get; set; }
        public string select { get; set; }
        public bool async { get; set; }
    }

    public class PaConfigOther
    {
        public string keepLoad { get; set; }
        public bool autoHead { get; set; }
        public bool footer { get; set; }
    }

    public class PaConfigTab
    {
        public bool enable { get; set; }
        public bool keepState { get; set; }
        public bool session { get; set; }
        public bool preload { get; set; }
        public string max { get; set; }
        public Index index { get; set; }
    }

    public class PaConfigTheme
    {
        public string defaultColor { get; set; }
        public string defaultMenu { get; set; }
        public string defaultHeader { get; set; }
        public bool allowCustom { get; set; }
        public bool banner { get; set; }
    }


}
