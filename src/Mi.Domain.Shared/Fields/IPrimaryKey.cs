namespace Mi.Domain.Shared.Fields
{
    public interface IPrimaryKey<T>
    {
        T Id { get; set; }
    }
}