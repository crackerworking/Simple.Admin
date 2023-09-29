namespace Mi.Domain.Shared.Core
{
    public interface IFactory
    {
        T Create<T>();
    }
}