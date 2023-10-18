namespace Mi.Domain.Shared.Options
{
    public class Option
    {
        public string? Name { get; set; }
        public string? Value { get; set; }

        public Option() { }

        public Option(string? name, string? value)
        {
            Name = name;
            Value = value;
        }
    }
}