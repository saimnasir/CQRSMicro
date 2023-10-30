using CQRSMicro.Product.CQRS.Commands.Request;
using MediatR;
using Patika.Framework.Shared.DTO;
using Patika.Framework.Shared.Services;

namespace CQRSMicro.Product.CQRS.Handlers
{
    public class SendOTPCommandHandler : CoreService, IRequestHandler<SendOTPCommandRequest, DTO>
    { 
        public SendOTPCommandHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        { 
        }
        public async Task<DTO> Handle(SendOTPCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {                
                return new DTO
                {
                    LogId = request.LogId
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
