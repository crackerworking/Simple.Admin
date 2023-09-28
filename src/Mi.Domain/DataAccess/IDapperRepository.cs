namespace Mi.Domain.DataAccess
{
    public interface IDapperRepository
    {
        Task<List<T>> QueryAsync<T>(string sql, object? param = default);

        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object? param = default);

        Task<T> ExecuteScalarAsync<T>(string sql, object? param = default);

        Task<int> ExecuteAsync(string sql, object? param = default);
    }
}