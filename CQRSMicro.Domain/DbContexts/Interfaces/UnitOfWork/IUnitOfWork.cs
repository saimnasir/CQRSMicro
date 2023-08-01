namespace CQRSMicro.Domain.DbContexts.Interfaces.UnitOfWork
{
    public interface IUnitOfWork<IDbContext>
    {
        IDbContext DbContext { get; set; }
    }
}