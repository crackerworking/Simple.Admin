namespace Mi.Domain.Entities.Field
{
    public interface IPrimaryKey<T>
    {
        T Id { get; set; }
    }
}