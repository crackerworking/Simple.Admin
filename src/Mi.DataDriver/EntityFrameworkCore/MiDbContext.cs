using Mi.Domain.Entities.System;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Mi.DataDriver.EntityFrameworkCore
{
    internal class MiDbContext : DbContext
    {
        public MiDbContext()
        { }

        public MiDbContext(DbContextOptions<MiDbContext> contextOptions) : base(contextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region System

            modelBuilder.Entity<SysDict>().HasQueryFilter(x => x.IsDeleted == 0);
            modelBuilder.Entity<SysFunction>().HasQueryFilter(x => x.IsDeleted == 0);
            modelBuilder.Entity<SysRole>().HasQueryFilter(x => x.IsDeleted == 0);
            modelBuilder.Entity<SysRoleFunction>().HasQueryFilter(x => x.IsDeleted == 0);
            modelBuilder.Entity<SysUser>().HasQueryFilter(x => x.IsDeleted == 0);
            modelBuilder.Entity<SysUserRole>().HasQueryFilter(x => x.IsDeleted == 0);
            modelBuilder.Entity<SysMessage>().HasQueryFilter(x => x.IsDeleted == 0);
            modelBuilder.Entity<SysLoginLog>().HasQueryFilter(x => x.IsDeleted == 0);
            modelBuilder.Entity<SysLog>().HasQueryFilter(x => x.IsDeleted == 0);
            modelBuilder.Entity<SysTask>().HasQueryFilter(x => x.IsDeleted == 0);

            #endregion System
        }
    }

    public static class MiDbContextSetup
    {
        public static void AddMiDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<MiDbContext>(options =>
            {
                options.UseSqlite(connectionString);
            }, ServiceLifetime.Scoped);
        }
    }
}