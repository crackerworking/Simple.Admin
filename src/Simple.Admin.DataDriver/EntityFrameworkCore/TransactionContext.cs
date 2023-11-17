using Simple.Admin.Domain.DataAccess;

namespace Simple.Admin.DataDriver.EntityFrameworkCore
{
    internal class TransactionContext : ITransactionContext
    {
        private readonly MiDbContext _miDbContext;

        public TransactionContext(MiDbContext miDbContext)
        {
            _miDbContext = miDbContext;
        }

        public void Begin()
        {
            _miDbContext.Database.BeginTransaction();
        }

        public void Commit()
        {
            _miDbContext.Database.CommitTransaction();
        }

        public void Rollback()
        {
            _miDbContext.Database.RollbackTransaction();
        }
    }
}