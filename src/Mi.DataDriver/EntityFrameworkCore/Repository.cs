﻿using System.Linq.Expressions;

using Mi.Domain.DataAccess;
using Mi.Domain.Entities;
using Mi.Domain.User;

using Microsoft.EntityFrameworkCore;

namespace Mi.DataDriver.EntityFrameworkCore
{
    internal class Repository<T> : IRepository<T> where T : EntityBase, new()
    {
        private readonly MiDbContext _dbContext;
        private readonly ICurrentUser _currentUser;

        internal Repository(MiDbContext dbContext, ICurrentUser currentUser)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
        }

        public async Task<int> AddAsync(T model)
        {
            if (model.CreatedBy == 0)
                model.CreatedBy = _currentUser.UserId;
            if (model.CreatedOn.Equals(new DateTime()))
                model.CreatedOn = DateTime.Now;

            _dbContext.Add(model);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> AddRangeAsync(IEnumerable<T> models)
        {
            await _dbContext.AddRangeAsync(models);
            return await _dbContext.SaveChangesAsync();
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>>? expression = null)
        {
            expression ??= x => x.IsDeleted == 0;
            return _dbContext.Set<T>().AnyAsync(expression);
        }

        public Task<int> CountAsync(Expression<Func<T, bool>>? expression = null)
        {
            expression ??= x => x.IsDeleted == 0;
            return _dbContext.Set<T>().CountAsync(expression);
        }

        public async Task<int> DeleteAsync(long id)
        {
            var model = await GetAsync(x => x.Id == id);
            if (model == null) return 0;
            _dbContext.Remove(model);
            return await _dbContext.SaveChangesAsync();
        }

        public Task<int> DeleteAsync(T model)
        {
            _dbContext.Remove(model);
            return _dbContext.SaveChangesAsync();
        }

        public Task<int> DeleteRangeAsync(IEnumerable<T> models)
        {
            _dbContext.RemoveRange(models);
            return _dbContext.SaveChangesAsync();
        }

        public Task<T?> GetAsync(Expression<Func<T, bool>>? expression = null)
        {
            expression ??= x => x.IsDeleted == 0;
            return _dbContext.Set<T>().FirstOrDefaultAsync(expression);
        }

        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>>? expression = null)
        {
            expression ??= x => x.IsDeleted == 0;
            return await _dbContext.Set<T>().Where(expression).ToListAsync();
        }

        public Task<int> UpdateAsync(T model)
        {
            if (model.ModifiedBy.GetValueOrDefault() == 0)
                model.ModifiedBy = _currentUser.UserId;
            if (!model.ModifiedOn.HasValue)
                model.ModifiedOn = DateTime.Now;

            _dbContext.Update(model);
            return _dbContext.SaveChangesAsync();
        }

        public Task<int> UpdateRangeAsync(IEnumerable<T> models)
        {
            _dbContext.UpdateRange(models);
            return _dbContext.SaveChangesAsync();
        }
    }
}