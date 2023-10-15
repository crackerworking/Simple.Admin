using System.Data;

using Dapper;

using Mi.DataDriver.EntityFrameworkCore;
using Mi.Domain.DataAccess;
using Mi.Domain.Shared.Core;
using Mi.Domain.Shared.Models;

using Microsoft.EntityFrameworkCore;

namespace Mi.DataDriver.Dapper
{
    internal class DapperRepository : IDapperRepository
    {
        private readonly IDbConnection _conn;
        private readonly ICurrentUser _currentUser;

        public DapperRepository(MiDbContext dbContext,ICurrentUser currentUser)
        {
            _conn = dbContext.Database.GetDbConnection();
            _currentUser = currentUser;
        }

        public Task<int> ExecuteAsync(string sql, object? param = null)
        {
            Demo.ThrowExceptionForDBWriteAction(_currentUser);
            return _conn.ExecuteAsync(sql, param);
        }

        public Task<T> ExecuteScalarAsync<T>(string sql, object? param = null)
        {
            return _conn.ExecuteScalarAsync<T>(sql, param);
        }

        public async Task<List<T>> QueryAsync<T>(string sql, object? param = null)
        {
            return (await _conn.QueryAsync<T>(sql, param)).ToList();
        }

        public Task<T> QueryFirstOrDefaultAsync<T>(string sql, object? param = null)
        {
            return _conn.QueryFirstOrDefaultAsync<T>(sql, param);
        }

        public async Task<PagingModel<T>> QueryPagedAsync<T>(string sql, int page, int size, string? orderBy = null, object? param = null) where T : class, new()
        {
            var querySql = $"select * from ({sql}) as m ";
            var total = await ExecuteScalarAsync<int>($"select count(*) from ({sql}) m ", param);
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                querySql += $" order by {orderBy} ";
            }
            if (page > 0 && size > 0)
            {
                querySql += $" limit {(page - 1) * size},{size} ";
            }

            var model = new PagingModel<T>
            {
                Total = total,
                Rows = await QueryAsync<T>(querySql, param)
            };

            return model;
        }
    }
}