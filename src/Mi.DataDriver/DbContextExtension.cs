using Microsoft.EntityFrameworkCore;

namespace Mi.DataDriver
{
    public static class DbContextExtension
    {
        /// <summary>
        /// 不跟踪
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        public static IQueryable<T> SetNoTracking<T>(this DbContext dbContext) where T : class
        {
            return dbContext.Set<T>().AsNoTracking();
        }
    }
}