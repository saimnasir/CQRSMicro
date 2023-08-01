namespace CQRSMicro.Domain.DbContexts.Interfaces.UnitOfWork
{
    public interface IUnitOfWorkHostEvents
    {
        event EventHandler Committed;
        event EventHandler RollBacked;
    }
}