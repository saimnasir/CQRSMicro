using Patika.Framework.Shared.Entities;
using Patika.Framework.Shared.Enums;
using Patika.Framework.Shared.Interfaces;
using Patika.Framework.Shared.Services;

namespace CQRSMicro.Domain.Logger
{
    public class LogWriter : CoreService, ILogWriter
    {
        ILogRepository LogRepository { get; }

        public LogWriter(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            LogRepository = GetService<ILogRepository>();
        }

        public async Task AddLogDetail(Guid logId, LogDetail detail)
        {
            await Task.CompletedTask;
        }

        public async Task<Log> CreateLog(string applicationName, Guid? userId = null, LogStatusEnum intialStatus = LogStatusEnum.Started)
        {
            await Task.CompletedTask;

            return new Log();
        }

        public async Task FinishLog(Guid logId, LogStatusEnum finalStatus = LogStatusEnum.Success)
        {

            await Task.CompletedTask;
        }

    }
}
