namespace Simple.Admin.Domain.Shared.Options
{
    public class Option
    {
        /// <summary>
        /// 名
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string? Value { get; set; }

        public Option()
        { }

        public Option(string? name, string? value)
        {
            Name = name;
            Value = value;
        }
    }
}