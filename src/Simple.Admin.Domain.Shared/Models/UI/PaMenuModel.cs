namespace Simple.Admin.Domain.Shared.Models.UI
{
    /// <summary>
    /// pearadmin的侧边menu菜单
    /// </summary>
    public class PaMenuModel
    {
        /// <summary>
        /// 唯一Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string? Icon { get; set; }

        /// <summary>
        /// 0目录 1菜单
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 当 type 为 1 时，openType 生效，_iframe 正常打开 _blank 新建浏览器标签页
        /// </summary>
        public string? OpenType { get; set; }

        /// <summary>
        /// 菜单类型下访问的页面
        /// </summary>
        public string? Href { get; set; }

        /// <summary>
        /// 目录类型下，该目录下菜单的数组数据
        /// </summary>
        public List<PaMenuModel>? Children { get; set; }

        public PaMenuModel()
        {
        }

        public PaMenuModel(int type, string? title, string? href)
        {
            Id = 0;
            Type = type;
            Title = title;
            Href = href;
            OpenType = type == 1 ? "_iframe" : "";
        }

        public PaMenuModel(int type, string? title, string? href, List<PaMenuModel> children)
        {
            Id = 0;
            Type = type;
            Title = title;
            Href = href;
            OpenType = type == 1 ? "_iframe" : "";
            Children = children;
        }

        public PaMenuModel(long id, int type, string? title, string? href, List<PaMenuModel> children)
        {
            Id = id;
            Type = type;
            Title = title;
            Href = href;
            OpenType = type == 1 ? "_iframe" : "";
            Children = children;
        }

        public PaMenuModel(long id, int type, string? title, string? href, string? icon, List<PaMenuModel> children)
        {
            Id = id;
            Type = type;
            Title = title;
            Href = href;
            Icon = icon;
            OpenType = type == 1 ? "_iframe" : "";
            Children = children;
        }
    }
}