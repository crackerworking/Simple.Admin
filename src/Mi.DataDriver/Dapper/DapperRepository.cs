using Mi.DataDriver.EntityFrameworkCore;
using System.Data;

using Mi.Domain.DataAccess;
using Microsoft.EntityFrameworkCore;
using Dapper;

namespace Mi.DataDriver.Dapper
{
    internal class DapperRepository : IDapperRepository
    {
        private readonly IDbConnection _conn;

        public DapperRepository(MiDbContext dbContext)
        {
            _conn = dbContext.Database.GetDbConnection();
        }

        public Task<int> ExecuteAsync(string sql, object? param = null)
        {
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
    }
}