using Simple.Admin.Domain.Shared.Models;

namespace Simple.Admin.Domain.DataAccess
{
    /// <summary>
    /// IDapperRepository
    /// </summary>
    public interface IDapperRepository
    {
        Task<List<T>> QueryAsync<T>(string sql, object? param = default);

        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object? param = default);

        Task<T> ExecuteScalarAsync<T>(string sql, object? param = default);

        Task<int> ExecuteAsync(string sql, object? param = default);

        Task<PagingModel<T>> QueryPagedAsync<T>(string sql, int page, int size, string? orderBy = default, object? param = default) where T : class, new();
    }
}