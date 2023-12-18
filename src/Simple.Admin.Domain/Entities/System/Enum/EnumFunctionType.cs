namespace Simple.Admin.Domain.Entities.System.Enum
{
    /// <summary>
    /// 功能类型 Menu = 10,Button = 20
    /// </summary>
    public enum EnumFunctionType
    {
        Default = 0,

        /// <summary>
        /// 菜单
        /// </summary>
        Menu = 10,

        /// <summary>
        /// 按钮
        /// </summary>
        Button = 20,

        /// <summary>
        /// 资源
        /// </summary>
        Resource = 30,

        /// <summary>
        /// 功能，不借助按钮给用户操作
        /// </summary>
        Function = 40
    }
}