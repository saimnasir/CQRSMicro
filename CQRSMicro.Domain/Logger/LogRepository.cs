using Patika.Framework.Shared.Entities;

namespace CQRSMicro.Domain.Logger
{
    public class LogRepository : ILogRepository
    {

        public async Task<LogDetail> AddDetail(Guid logId, LogDetail logDetail)
        {
            return logDetail;
        }

    }
}
