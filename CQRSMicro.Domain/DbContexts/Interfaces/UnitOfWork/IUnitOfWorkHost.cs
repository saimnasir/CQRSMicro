namespace CQRSMicro.Domain.DbContexts.Interfaces.UnitOfWork
{
    public interface IUnitOfWorkHost
    {
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        void BeginTransaction();
        Task CommitAsync(CancellationToken cancellationToken = default);
        void Commit();
        Task RollbackAsync(CancellationToken cancellationToken = default);
        void Rollback();
    }
}