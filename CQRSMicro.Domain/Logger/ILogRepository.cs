using Patika.Framework.Shared.Entities;

namespace CQRSMicro.Domain.Logger
{
    public interface ILogRepository
    {
        Task<LogDetail> AddDetail(Guid logId, LogDetail logDetail);
    }
}
