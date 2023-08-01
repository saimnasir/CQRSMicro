namespace CQRSMicro.Domain.DbContexts.Interfaces.UnitOfWork
{
    public interface IUnitOfWorkHostInterface : IUnitOfWorkHostEvents
    {
        object DbContext { get; }
    }
}