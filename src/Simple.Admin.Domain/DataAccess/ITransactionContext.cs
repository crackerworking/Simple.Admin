namespace Simple.Admin.Domain.DataAccess
{
    public interface ITransactionContext
    {
        void Begin();

        void Commit();

        void Rollback();
    }
}